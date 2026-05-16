using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using WarehouseManagement.Data.AbstractClasses;

namespace WarehouseManagement.Data.Models
{
    public class Product : ObjectModel
    {
        public string Article { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public string? PhotoPath { get; set; }
        public BigInteger WhsId { get; set; }
        public Category Category { get; set; } = null!;
        public Manufacturer Manufacturer { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;

        public Product()
            : base()
        { }

        public Product(Warehouse wh)
            :base()
        {
            this.WhsId = wh.Id;
        }

        public bool Equals(Product other)
        {
            return this.Id == other.Id
                || (this.Name == other.Name 
                    && this.Article == other.Article
                    && this.Unit == other.Unit
                    && this.Price == other.Price
                    && this.DiscountPercent == other.DiscountPercent
                    && this.PhotoPath == other.PhotoPath
                    && this.PhotoPath == other.PhotoPath
                    && this.WhsId == other.WhsId
                    && this.Category.Name == other.Category.Name
                    && this.Manufacturer.Name == other.Manufacturer.Name
                    && this.Supplier.Name == other.Supplier.Name);
        }

        public bool Equals(InvoiceItem other, BigInteger whsId)
        {
            return this.Id == other.ProductId
                || (this.Name == other.Name
                    && this.Article == other.Article
                    && this.Unit == other.Unit
                    && this.Price == other.Price
                    && this.DiscountPercent == other.DiscountPercent
                    && this.PhotoPath == other.PhotoPath
                    && this.PhotoPath == other.PhotoPath
                    && this.WhsId == whsId
                    && (this.Category?.Name ?? string.Empty) == other.CategoryName
                    && (this.Manufacturer?.Name ?? string.Empty) == other.ManufacturerName
                    && (this.Supplier?.Name ?? string.Empty) == other.SupplierName);
        }
    }
}
