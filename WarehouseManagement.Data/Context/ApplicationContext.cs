using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WarehouseManagement.Data.Models;

namespace WarehouseManagement.Data.Context
{
    public class ApplicationContext
    {
        public ObservableCollection<Organization> Orgs;
        public ObservableCollection<Warehouse> Whs;
        public ObservableCollection<Product> Prdcts { get; } = new();
        public ObservableCollection<Invoice> Invcs { get; } = new();

        public ApplicationContext()
        {
            Orgs = new ObservableCollection<Organization>();
            Whs = new ObservableCollection<Warehouse>();
            Prdcts = new ObservableCollection<Product>();
            Invcs = new ObservableCollection<Invoice>();

            OrgsFill();
        }

        private void OrgsFill()
        {
            Organization org1 = new Organization("Рога и копыта, ООО");
            Organization org2 = new Organization("Пупкин и сыновья, ООО");

            Warehouse wh1 = new Warehouse("Склад Рогов и Копыт №1", "Там, за лесом", org1.Id);
            Warehouse wh2 = new Warehouse("Склад Рогов и Копыт №2", "Там, за лесом", org1.Id);
            Warehouse wh3 = new Warehouse("Склад Пупкина №1", "Там, за горой", org2.Id);
            Warehouse wh4 = new Warehouse("Склад Пупкина №2", "Там, за горой", org2.Id);

            Product product1 = new Product()
            {
                Article = "Артикль",
                Name = "Рога",
                WhsId = wh1.Id,
                Price = 120,
                StockQuantity = 100,
            };

            Product product2 = new Product()
            {
                Article = "Артикль 2",
                Name = "Копыта",
                WhsId = wh1.Id,
                Price = 120,
                StockQuantity = 100,
            };

            Orgs.Add(org1);
            Orgs.Add(org2);
            Whs.Add(wh1);
            Whs.Add(wh2);
            Whs.Add(wh3);
            Whs.Add(wh4);
            Prdcts.Add(product1);
            Prdcts.Add(product2);
        }
    }
}
