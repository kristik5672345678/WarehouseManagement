using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using WarehouseManagement.Data.AbstractClasses;

namespace WarehouseManagement.Data.Models
{
    public class Organization : ObjectModel
    {
        public string OrgName { get; set; }

        public Organization(string name)
            :base()
        {
            OrgName = name;
        }

    }
}
