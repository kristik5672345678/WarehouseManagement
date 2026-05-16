using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WarehouseManagement.Data.Models
{
    public class Manufacturer
    {
        public string Name { get; set; } = null!;

        public Manufacturer() { }

        public Manufacturer(string name) 
        {
            this.Name = name;
        }
    }
}
