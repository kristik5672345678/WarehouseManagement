using System.Windows;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductModWindow.xaml
    /// </summary>
    public partial class ProductModWindow : Window
    {
        private ServiceProducts _service;
        private Product? _editingProduct;
        private bool _isEdit => _editingProduct != null;
        public Product? CreatedProduct { get; private set; }


        public ProductModWindow(ServiceProducts service)
        {
            InitializeComponent();

            _service = service;

            Title = "Новый товар";
            txtArticle.Focus();
        }

        public ProductModWindow(Product productToEdit, ServiceProducts serviceProducts)
        {
            InitializeComponent();
            _service = serviceProducts;
            _editingProduct = productToEdit;

            Title = "Редактирование товара";

            LoadProductData();
        }

        private void LoadProductData()
        {
            if (!_isEdit)
                return;

            txtArticle.Text = _editingProduct.Article;
            txtName.Text = _editingProduct.Name;
            txtUnit.Text = _editingProduct.Unit;
            txtPrice.Text = _editingProduct.Price.ToString("0.##");
            txtDiscount.Text = _editingProduct.DiscountPercent.ToString("0.##");
            txtSupplierName.Text = _editingProduct.Supplier?.Name ?? string.Empty;
            txtCategoryName.Text = _editingProduct.Category?.Name ?? string.Empty;
            txtManufacturerName.Text = _editingProduct.Manufacturer?.Name ?? string.Empty;
            txtPhotoPath.Text = _editingProduct.PhotoPath ?? "";
            txtDescription.Text = _editingProduct.Description ?? "";
            txtQuantity.Text = _editingProduct.StockQuantity.ToString();
            txtQuantity.IsReadOnly = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text) || string.IsNullOrWhiteSpace(txtName.Text)
                || string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Заполните все поля, помеченные символом \"*\"", "Ошибка");
                return;
            }

            int quantity = 1;
            decimal price = 0, discount = 0;

            if (!decimal.TryParse(txtPrice.Text, out price) 
                || !decimal.TryParse(txtDiscount.Text, out discount)
                || !int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Поля \"Цена\", \"Скидка\", \"Количество\" должны быть заполнены корректно", "Ошибка");
                return;
            }

            var product = _editingProduct ?? new Product();

            product.Article = txtArticle.Text;
            product.Name = txtName.Text;
            product.Unit = txtUnit.Text;
            product.Price = price;
            product.DiscountPercent = discount;
            product.PhotoPath = txtPhotoPath.Text;
            product.Description = txtDescription.Text;
            product.WhsId = this._service.GetWarehouse().Id;
            product.Category = new Category() { Name = txtCategoryName.Text };
            product.Manufacturer = new Manufacturer() { Name = txtManufacturerName.Text };
            product.Supplier = new Supplier() { Name = txtSupplierName.Text };
            product.StockQuantity = quantity;

            bool success = !_isEdit || _service.Edit(product);

            if (!success)
            {
                MessageBox.Show("Не удалось сохранить товар. Возможно, артикул уже существует.", "Ошибка");
                return;
            }

            CreatedProduct = product;
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
