using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WarehouseManagement.Data.Models
{
    public class InvoiceItem
    {
        public BigInteger? ProductId { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPercent { get; set; }
        public string? Description { get; set; }
        public string? PhotoPath { get; set; }
        public string? CategoryName { get; set; } = null!;
        public string? ManufacturerName { get; set; } = null!;
        public string? SupplierName { get; set; } = null!;

        public InvoiceItem() { }

        public InvoiceItem(Product pr)
        {
            ProductId = pr.Id;
            Article = pr.Article;
            Name = pr.Name;
            Unit = pr.Unit;
            Price = pr.Price;
            DiscountPercent = pr.DiscountPercent;
            Description = pr.Description;
            PhotoPath = pr.PhotoPath;
            CategoryName = pr.Category?.Name;
            ManufacturerName = pr.Manufacturer?.Name;
            SupplierName = pr.Manufacturer?.Name;
            Quantity = pr.StockQuantity;
        }
    }
}
