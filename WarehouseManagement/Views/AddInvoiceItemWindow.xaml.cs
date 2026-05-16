using System.Windows;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Логика взаимодействия для AddInvoiceItemWindow.xaml
    /// </summary>
    public partial class AddInvoiceItemWindow : Window
    {
        private ServiceProducts _service;
        public InvoiceItem ResultItem { get; private set; } = null!;

        public AddInvoiceItemWindow(ServiceProducts service)
        {
            InitializeComponent();

            _service = service;

            FillProductsCollection();
        }

        private void FillProductsCollection()
        {
            lbProducts.ItemsSource = _service.GetAll();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество!", "Ошибка");
                return;
            }

            Product? selectedProduct = lbProducts.SelectedItem as Product;

            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите товар или отметьте создание нового", "Внимание");
                return;
            }

            ResultItem = new InvoiceItem(selectedProduct)
            {
                Quantity = quantity,
            };

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
