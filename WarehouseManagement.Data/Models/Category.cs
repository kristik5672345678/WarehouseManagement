using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WarehouseManagement.Data.Models
{
    public class Category
    {
        public string Name { get; set; } = null!;

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
