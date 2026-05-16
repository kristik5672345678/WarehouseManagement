using System;
using System.Collections.Generic;
using System.Numerics;
using WarehouseManagement.Data.AbstractClasses;
using WarehouseManagement.Data.Enums;

namespace WarehouseManagement.Data.Models
{
    public class Invoice : ObjectModel
    {
        public InvoiceType Type { get; set; }
        public BigInteger WhsId { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public Invoice()
            :base()
        { }

        public Invoice(Warehouse whs)
            : base()
        {
            this.WhsId = whs.Id;
        }
    }
}
