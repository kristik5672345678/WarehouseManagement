using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;
using WarehouseManagement.Views;

namespace WarehouseManagement.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControllWareHouses.xaml
    /// </summary>
    public partial class UserControllWareHouses : UserControl
    {
        private ServiceWhs _serviceWhs;
        private ServiceProducts? _serviceProducts;

        private Warehouse? _selectedWhs;

        public UserControllWareHouses(ServiceWhs serviceWhs)
        {
            InitializeComponent();

            _serviceWhs = serviceWhs;

            FillWarehousesCollection();
        }

        private void FillWarehousesCollection()
        {
            int idx = 0;
            if (lbxWhList.SelectedIndex > 0)
                idx = lbxWhList.SelectedIndex;


            lbxWhList.ItemsSource = null;
            lbxWhList.Items.Clear();
            lbxWhList.ItemsSource = _serviceWhs.GetAll();
            if (lbxWhList.Items.Count > 0)
            {
                try
                {
                    lbxWhList.SelectedIndex = idx;
                    _selectedWhs = lbxWhList.SelectedItem as Warehouse;
                }
                catch
                {
                    lbxWhList.SelectedIndex = -1;
                    _selectedWhs = null;
                }
            }

            CommandManager.InvalidateRequerySuggested();
        }

        private void lbxWhList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedWhs = lbxWhList.SelectedItem as Warehouse;

            if (_selectedWhs == null)
                return;

            _serviceProducts = new ServiceProducts(_serviceWhs.GetContext(), _selectedWhs);

            string filter = TxtSearch.Text.Trim().ToLower();
            FillProductsCollection(filter);
            CommandManager.InvalidateRequerySuggested();
        }

        public void FillProductsCollection(string filter = "")
        {
            if (_selectedWhs == null || _serviceProducts == null)
            {
                DgProducts.ItemsSource = null;
                return;
            }

            int idx = 0;
            if (DgProducts.SelectedIndex > 0)
                idx = DgProducts.SelectedIndex;

            DgProducts.ItemsSource = null;
            DgProducts.Items.Clear();
            DgProducts.ItemsSource = _serviceProducts.GetAll(filter);
            if (DgProducts.Items.Count > 0)
            {
                try
                {
                    DgProducts.SelectedIndex = idx;
                }
                catch
                {
                    DgProducts.SelectedIndex = -1;
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = TxtSearch.Text.Trim().ToLower();
            FillProductsCollection(filter);
        }

        private void WhAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WhModWindow cmdWindow = new WhModWindow("Добавить склад", _serviceWhs, null);
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                cmdWindow.Owner = parentWindow;
            }
            if (cmdWindow.ShowDialog() == true)
                FillWarehousesCollection();
        }

        private void WhEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_selectedWhs == null)
            {
                MessageBox.Show("Выберите склад для изменения.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            WhModWindow cmdWindow = new WhModWindow("Изменить склад", _serviceWhs, _selectedWhs);
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                cmdWindow.Owner = parentWindow;
            }
            if (cmdWindow.ShowDialog() == true)
                FillWarehousesCollection();
        }

        private void WhDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_selectedWhs != null)
            {
                var result = MessageBox.Show(
                $"Вы действительно хотите удалить склад «{_selectedWhs.WhName}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _serviceWhs.Delete(_selectedWhs);
                    FillWarehousesCollection();
                }
            } 
        }

        private void DgProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject source)
            {
                while (source != null)
                {
                    if (source is DataGridColumnHeader)
                        return;

                    source = VisualTreeHelper.GetParent(source);
                }
            }

            if (DgProducts.SelectedItem is Product product)
            {
                var selectedWhs = lbxWhList.SelectedItem as Warehouse;

                if (selectedWhs != null)
                {
                    var editor = new ProductModWindow(product, new ServiceProducts(_serviceWhs.GetContext(), selectedWhs));
                    if (Window.GetWindow(this) is Window parent)
                        editor.Owner = parent;

                    if (editor.ShowDialog() == true)
                    {
                        FillProductsCollection();
                    }
                } 
            }
        }

        public Warehouse? GetSelectedWhs()
        {
            return this._selectedWhs;
        }


        private void WhAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void WhEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =  _selectedWhs != null;
        }

        private void WhDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _selectedWhs != null;
        }
    }
}
