using System.Collections.ObjectModel;
using System.Windows;
using WarehouseManagement.Data.Enums;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Логика взаимодействия для OutgoingInvoiceWindow.xaml
    /// </summary>
    public partial class OutgoingInvoiceWindow : Window
    {
        private ServiceInvoices _service;
        public ObservableCollection<InvoiceItem> Items { get; } = new ObservableCollection<InvoiceItem>();

        public OutgoingInvoiceWindow(ServiceInvoices service)
        {
            InitializeComponent();

            _service = service;

            Title = $"Исходящая накладная — {_service.GetWarehouse().WhName}";
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddInvoiceItemWindow(new ServiceProducts(
                _service.GetContext(), 
                _service.GetWarehouse()));

            if (addWindow.ShowDialog() == true && addWindow.ResultItem != null)
            {
                Items.Add(addWindow.ResultItem);
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
                MessageBox.Show("Добавьте хотя бы одну позицию!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var invoice = new Invoice
            {
                Type = InvoiceType.Outgoing,
                WhsId = _service.GetWarehouse().Id,
                Items = Items.ToList()
            };

            if (!_service.Add(invoice))
            {
                MessageBox.Show("Ошибка при проведении накладной!\nВозможно, недостаточно товара на складе.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServiceProducts serviceProducts = new ServiceProducts(_service.GetContext(), _service.GetWarehouse());

            foreach (var item in Items)
                serviceProducts.Add(item, InvoiceType.Outgoing);

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
