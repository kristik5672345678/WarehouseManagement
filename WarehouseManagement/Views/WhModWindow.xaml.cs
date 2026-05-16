using System.Windows;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Логика взаимодействия для WhModWindow.xaml
    /// </summary>
    public partial class WhModWindow : Window
    {
        private readonly ServiceWhs _service;
        private Warehouse? _whs;

        public WhModWindow(string title, ServiceWhs service, Warehouse? whs)
        {
            InitializeComponent();

            this.Title = title;
            this._service = service;
            this._whs = whs;
            this.tbWhName.Text = whs?.WhName;
            this.tbWhAddress.Text = whs?.WhAddress;
        }

        private void WhOk_Executed(object sender, RoutedEventArgs e)
        {
            this.tbError.Text = string.Empty;
            string name = this.tbWhName.Text?.Trim() ?? string.Empty;
            string address = this.tbWhAddress.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(address))
            {
                this.tbError.Text = "Наименование склада и/или адрес не заполнены.";
                return;
            }

            bool saved;
            if (this._whs != null)
            {
                this._whs.WhName = name;
                this._whs.WhAddress = address;
                saved = this._service.Edit(this._whs);
            }
            else
            {
                Warehouse newWhs = new Warehouse(name, address, this._service.GetOrganization().Id);
                saved = this._service.Add(newWhs);
            }

            if (!saved)
            {
                MessageBox.Show(this, "Не удалось сохранить склад.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void WhCancel_Executed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
