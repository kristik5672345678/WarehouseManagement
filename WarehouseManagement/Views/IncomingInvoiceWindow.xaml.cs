using System.Collections.ObjectModel;
using System.Windows;
using WarehouseManagement.Data.Enums;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Логика взаимодействия для IncomingInvoiceWindow.xaml
    /// </summary>
    public partial class IncomingInvoiceWindow : Window
    {
        private ServiceInvoices _service;

        public ObservableCollection<InvoiceItem> Items { get; } = new ObservableCollection<InvoiceItem>();

        public IncomingInvoiceWindow(ServiceInvoices serviceInvoice)
        {
            InitializeComponent();

            _service = serviceInvoice;

            Title = $"Входящая накладная — {_service.GetWarehouse().WhName}";
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var productWindow = new ProductModWindow(new ServiceProducts(_service.GetContext(), _service.GetWarehouse()));

            if (productWindow.ShowDialog() == true && productWindow.CreatedProduct != null)
            {
                var product = productWindow.CreatedProduct;

                var item = new InvoiceItem(product);

                Items.Add(item);
                dgItems.ItemsSource = Items;
            }
        }

        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgItems.SelectedItem is InvoiceItem item)
            {
                Items.Remove(item);
            }
            else
            {
                MessageBox.Show("Выберите позицию для удаления", "Внимание");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну позицию в накладную!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var invoice = new Invoice
            {
                Type = InvoiceType.Incoming,
                WhsId = _service.GetWarehouse().Id,           
                Items = Items.ToList()
            };

            if (!_service.Add(invoice))
            {
                MessageBox.Show("Ошибка при проведении накладной!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServiceProducts serviceProducts = new ServiceProducts(_service.GetContext(), _service.GetWarehouse());

            foreach (var item in Items)
                serviceProducts.Add(item, InvoiceType.Incoming);

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
