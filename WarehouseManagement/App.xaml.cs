using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Services;
using WarehouseManagement.Views;

namespace WarehouseManagement
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationContext _context;

        public ServiceOrgs service {  get; set; }

        public App()
        {
            _context = new ApplicationContext();
            service = new ServiceOrgs(_context);

            OrgsView orgsView = new OrgsView(service);
            orgsView.Show();
        }
    }
}
