using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using WarehouseManagement.Data.AbstractClasses;

namespace WarehouseManagement.Data.Models
{
    public class Warehouse : ObjectModel
    {
        public string WhName { get; set; }
        public string WhAddress { get; set; }
        public BigInteger OrgId { get; set; }

        public Warehouse()
            :base()
        { }

        public Warehouse(string name, string address, BigInteger orgid)
            :base()
        {
            this.Id = ++Counter;
            this.WhName = name;
            this.WhAddress = address;
            this.OrgId = orgid;
        }
    }
}
