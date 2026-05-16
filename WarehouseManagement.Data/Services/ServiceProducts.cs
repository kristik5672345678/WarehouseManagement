using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Enums;
using WarehouseManagement.Data.Interfaces;
using WarehouseManagement.Data.Models;

namespace WarehouseManagement.Data.Services
{
    public class ServiceProducts : IService<Product>
    {
        private ApplicationContext _context;
        private Warehouse _whs { get; set; }

        public ServiceProducts(ApplicationContext context, Warehouse whs)
        {
            if (context != null && whs != null)
            {
                _context = context;
                _whs = whs;
            }    
        }

        public ObservableCollection<Product> GetAll()
        {
            return new ObservableCollection<Product>(_context.Prdcts
                .Where(x => x.WhsId == _whs.Id)
                .ToList());
        }

        public ObservableCollection<Product> GetAll(string filterRaw)
        {
            string filter = filterRaw.Trim().ToLower();

            if (string.IsNullOrEmpty(filter))
                return GetAll();

            return new ObservableCollection<Product>(_context.Prdcts
                .Where(x => x.WhsId == _whs.Id 
                && (x.Name.ToLower().Contains(filter) 
                || (x.Manufacturer?.Name ?? string.Empty).ToLower().Contains(filter)
                || (x.Supplier?.Name ?? string.Empty).ToLower().Contains(filter)
                || (x.Category?.Name ?? string.Empty).ToLower().Contains(filter)))
                .ToList());
        }

        public bool Add(Product product)
        {
            if (product == null 
                || string.IsNullOrWhiteSpace(product.Name)
                || _context.Prdcts.Any(p => p.Equals(product))
                || product.WhsId != _whs.Id) 
                return false;

            _context.Prdcts.Add(product);
            return true;
        }

        public bool Edit(Product product)
        {
            if (product == null
                || string.IsNullOrWhiteSpace(product.Name)
                || product.WhsId != _whs.Id)
                return false;

            int index = _context.Prdcts
                .ToList()
                .FindIndex(p => p.Equals(product));

            if (index == -1)
                return false;

            _context.Prdcts[index] = product;
            return true;
        }

        public bool Delete(Product product)
        {
            if (product == null
                || string.IsNullOrWhiteSpace(product.Name)
                || product.WhsId != _whs.Id)
                return false;

            Product? target = _context.Prdcts
                        .FirstOrDefault(p => p.Equals(product));

            if (target == null)
                return false;

            _context.Prdcts.Remove(target);
            return true;
        }

        public bool Add(InvoiceItem item, InvoiceType type)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                return false;

            Product? product = _context.Prdcts
                    .FirstOrDefault(x => x.Equals(item, _whs.Id));

            if (type == InvoiceType.Outgoing)
            {
                if (product == null)
                    return false;
                product.StockQuantity -= item.Quantity;

                if (product.StockQuantity > 0)
                    return Edit(product);
                else
                    return Delete(product);
            }
            else
            {
                if (product == null)
                {
                    if (item.Quantity <= 0)
                        return false;
                    product = new Product()
                    {
                        Article = item.Article,
                        Name = item.Name,
                        Unit = item.Unit,
                        Price = item.Price,
                        StockQuantity = item.Quantity,
                        DiscountPercent = item.DiscountPercent,
                        Description = item.Description,
                        PhotoPath = item.PhotoPath,
                        Category = new Category() { Name = item?.CategoryName ?? string.Empty, },
                        Manufacturer = new Manufacturer() { Name = item?.ManufacturerName ?? string.Empty, },
                        Supplier = new Supplier() { Name = item?.SupplierName ?? string.Empty, },
                        WhsId = _whs.Id,
                    };

                    return Add(product);
                }
                else
                {
                    product.StockQuantity += item.Quantity;
                    if (product.StockQuantity > 0)
                        return Edit(product);
                    else
                        return Delete(product);
                }
            }  
        }

        public ApplicationContext GetContext()
            => _context;

        public Warehouse GetWarehouse()
            => _whs;
    }
}
