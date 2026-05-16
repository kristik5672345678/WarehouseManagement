using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WarehouseManagement.Data.Models;
using WarehouseManagement.Data.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Логика взаимодействия для OrgsView.xaml
    /// </summary>
    public partial class OrgsView : Window
    {
        private ServiceOrgs _service;
        private Organization? _selectedOrg {  get; set; }

        public OrgsView()
        {
            InitializeComponent();
        }

        public OrgsView(ServiceOrgs svcOrgs)
        {
            InitializeComponent();

            this._service = svcOrgs;

            FillCompaniesCollection();
            this.lbxOrgsList.Focus();
        }


        private void FillCompaniesCollection()
        {
            int idx = 0;
            if (lbxOrgsList.SelectedIndex > 0)
                idx = lbxOrgsList.SelectedIndex;

            lbxOrgsList.ItemsSource = null;
            lbxOrgsList.Items.Clear();
            lbxOrgsList.ItemsSource = _service.GetAll();
            if (lbxOrgsList.Items.Count > 0)
            {
                try
                {
                    lbxOrgsList.SelectedIndex = idx;
                }
                catch
                {
                    lbxOrgsList.SelectedIndex = -1;
                }
            }
        }

        private void lbxOrgsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedOrg = lbxOrgsList.SelectedItem as Organization;
            tbStatus.Text = _selectedOrg?.OrgName ?? "—";
        }

        private void OrgAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите название новой организации:", "Новая организация", "");

            if (string.IsNullOrWhiteSpace(input))
                return;

            var newOrg = new Organization(input.Trim());
            if (!_service.Add(newOrg))
                return;

            tbStatus.Text = $"Организация «{input}» добавлена";
            FillCompaniesCollection();
        }

        private void OrgEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_selectedOrg == null)
            {
                MessageBox.Show("Выберите организацию для редактирования", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(
            "Введите новое название:", "Редактирование организации", _selectedOrg.OrgName);

            if (string.IsNullOrWhiteSpace(input) || input == _selectedOrg.OrgName)
                return;

            var editedOrg = new Organization(input)
            {
                Id = _selectedOrg.Id
            };

            if (_service.Edit(editedOrg))
            {
                FillCompaniesCollection();
            }
        }

        private void OrgDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_selectedOrg == null)
            {
                MessageBox.Show("Выберите организацию для редактирования", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Удалить организацию '{_selectedOrg.OrgName}'?", 
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_service.Delete(_selectedOrg))
                {
                    FillCompaniesCollection();
                }
            }
        }

        private void Ok_Executed(object sender, RoutedEventArgs e)
        {
            if (_selectedOrg != null && !string.IsNullOrEmpty(_selectedOrg.OrgName))
            {
                MainWindow mainWindow = new MainWindow(
                    _selectedOrg.OrgName, 
                    new ServiceWhs(_service.GetContext(), _selectedOrg));
                mainWindow.Show();
            }
        }

        private void Exit_Executed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
