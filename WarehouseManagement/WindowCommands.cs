using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using WarehouseManagement.Controls;
using WarehouseManagement.Views;

namespace WarehouseManagement
{
    public class WindowCommands
    {
        public static RoutedCommand Exit {  get; set; }
        public static RoutedCommand GetProducts { get; set; }
        public static RoutedCommand AddProduct { get; set; }
        public static RoutedCommand EditProduct { get; set; }
        public static RoutedCommand RemoveProduct { get; set; }
        public static RoutedCommand OrgCancel { get; set; }
        public static RoutedCommand OrgOk { get; set; }
        public static RoutedCommand OrgAdd { get; set; }
        public static RoutedCommand OrgEdit { get; set; }
        public static RoutedCommand OrgDelete { get; set; }
        public static RoutedCommand WhAdd { get; set; }
        public static RoutedCommand WhEdit { get; set; }
        public static RoutedCommand WhDelete { get; set; }
        public static RoutedCommand GetOrders { get; set; }
        public static RoutedCommand AddOrder { get; set; }
        public static RoutedCommand EditOrder { get; set; }
        public static RoutedCommand RemoveOrder { get; set; }

        public static RoutedCommand AddIncomingInvoice { get; set; }
        public static RoutedCommand AddOutgoingInvoice { get; set; }
        public static RoutedCommand OpenInvoices { get; set; }

        public static RoutedCommand Open { get; set; }
        public static RoutedCommand Save { get; set; }

        static WindowCommands()
        {
            OrgCancel = new RoutedCommand("OrgCancel", typeof(OrgsView)); 
            OrgOk = new RoutedCommand("OrgOk", typeof(OrgsView)); 
            OrgAdd = new RoutedCommand("OrgAdd", typeof(OrgsView)); 
            OrgEdit = new RoutedCommand("OrgEdit", typeof(OrgsView)); 
            OrgDelete = new RoutedCommand("OrgDelete", typeof(OrgsView));

            WhAdd = new RoutedCommand("WhAdd", typeof(UserControllWareHouses));
            WhEdit = new RoutedCommand("WhEdit", typeof(UserControllWareHouses));
            WhDelete = new RoutedCommand("WhDelete", typeof(UserControllWareHouses));

            AddIncomingInvoice = new RoutedCommand("AddIncomingInvoice", typeof(MainWindow));
            AddOutgoingInvoice = new RoutedCommand("AddOutgoingInvoice", typeof(MainWindow));
            OpenInvoices = new RoutedCommand("OpenInvoices", typeof(MainWindow));

            Exit = new RoutedCommand("Exit", typeof(MainWindow)); 
            GetProducts = new RoutedCommand("GetProducts", typeof(MainWindow)); 
            AddProduct = new RoutedCommand("AddProduct", typeof(MainWindow)); 
            EditProduct = new RoutedCommand("EditProduct", typeof(MainWindow)); 
            RemoveProduct = new RoutedCommand("RemoveProduct", typeof(MainWindow)); 
            GetOrders = new RoutedCommand("GetOrders", typeof(MainWindow)); 
            AddOrder = new RoutedCommand("AddOrder", typeof(MainWindow)); 
            EditOrder = new RoutedCommand("EditOrder", typeof(MainWindow)); 
            RemoveOrder = new RoutedCommand("RemoveOrder", typeof(MainWindow)); 
            Open = new RoutedCommand("Open", typeof(MainWindow));
            Save = new RoutedCommand("Save", typeof(MainWindow));
        }
    }
}
