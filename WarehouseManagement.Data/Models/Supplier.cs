using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WarehouseManagement.Data.Models
{
    public class Supplier
    {
        public string Name { get; set; } = null!;
        public Supplier() { }

        public Supplier(string name) 
        { 
            this.Name = name;
        }
    }
}
