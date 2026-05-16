using System.Windows;
using System.Windows.Input;
using WarehouseManagement.Controls;
using WarehouseManagement.Data.Services;
using WarehouseManagement.Views;

namespace WarehouseManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceWhs _service;

        private UserControllWareHouses? _curWhsControll;

        public MainWindow(string title, ServiceWhs service)
        {
            InitializeComponent();

            tbStatus.Text = title;
            this.Title = title;

            _service = service;
            _curWhsControll = new UserControllWareHouses(_service);

            this.MainFrame.Navigate(_curWhsControll);
        }

        private void AddIncomingInvoice_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_curWhsControll?.GetSelectedWhs() == null)
            {
                MessageBox.Show("Сначала выберите организацию и склад!", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedWhs = _curWhsControll.GetSelectedWhs();

            if (selectedWhs == null)
                return;

            var window = new IncomingInvoiceWindow(new ServiceInvoices(_service.GetContext(), selectedWhs));
            window.Owner = Window.GetWindow(this);

            if (window.ShowDialog() == true)
            {
                RefreshCurrentWarehousesControl();
            }
        }

        private void AddOutgoingInvoice_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_curWhsControll?.GetSelectedWhs() == null)
            {
                MessageBox.Show("Склад не выбран", "Внимание");
                return;
            }

            var selectedWhs = _curWhsControll.GetSelectedWhs();

            if (selectedWhs == null)
                return;

            var window = new OutgoingInvoiceWindow(new ServiceInvoices(_service.GetContext(), selectedWhs));
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                RefreshCurrentWarehousesControl();
            } 
        }

        public void RefreshCurrentWarehousesControl()
        {
            if (_curWhsControll == null)
                return;

            _curWhsControll.FillProductsCollection();
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}