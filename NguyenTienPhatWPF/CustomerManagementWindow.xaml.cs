using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL.Services;
using DAL.Entities;

namespace NguyenTienPhatWPF
{
    /// <summary>
    /// Interaction logic for CustomerManagementWindow.xaml
    /// </summary>
    public partial class CustomerManagementWindow : Window
    {
        private CustomerService _customerService = new();
        public CustomerManagementWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            CustomerDataGrid.ItemsSource = null;
            CustomerDataGrid.ItemsSource = _customerService.GetAll();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Customer? selectedCustomer = CustomerDataGrid.SelectedItem as Customer;
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to update.", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CustomerDetailWindow customerDetailWindow = new CustomerDetailWindow();
            customerDetailWindow.SelectedCustomer = selectedCustomer;
            customerDetailWindow.ShowDialog();
            FillDataGrid();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerDetailWindow customerDetailWindow = new CustomerDetailWindow();
            customerDetailWindow.SelectedCustomer = null; // No customer selected, so create a new one
            customerDetailWindow.CustomerIdTextBox.IsEnabled = false;
            //customerDetailWindow.StatusTextBox.IsEnabled = false; // Disable editing of Status
            customerDetailWindow.ShowDialog();
            FillDataGrid();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Customer? selectedCustomer = CustomerDataGrid.SelectedItem as Customer;
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to delete.", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult rs = MessageBox.Show($"Are you sure you want to delete customer {selectedCustomer.CustomerFullName}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                var existingCust = _customerService.GetCustomerById(selectedCustomer.CustomerId);
                _customerService.DeleteCustomer(existingCust!);
            }
            FillDataGrid();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //search by name
            string searchText = SearchTextBox.Text.Trim();
            CustomerDataGrid.ItemsSource = _customerService.SearchByName(searchText);
        }
    }
}
