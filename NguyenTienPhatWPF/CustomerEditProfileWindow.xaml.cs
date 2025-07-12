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
    /// Interaction logic for CustomerEditProfileWindow.xaml
    /// </summary>
    public partial class CustomerEditProfileWindow : Window
    {
        private CustomerService _customerService = new();
        public Customer? Customer { get; set; }
        public CustomerEditProfileWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        private void FillData()
        {
            if (Customer == null) return;
            var cust = _customerService.GetCustomerById(Customer.CustomerId);
            CustomerFullNameTextBox.Text = cust.CustomerFullName;
            TelephoneTextBox.Text = cust.Telephone;
            EmailAddressTextBox.Text = cust.EmailAddress;
            BirthdayDatePicker.SelectedDate = cust.CustomerBirthday?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Now;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var cust = _customerService.GetCustomerById(Customer!.CustomerId);
            cust!.CustomerFullName = CustomerFullNameTextBox.Text;
            cust.Telephone = TelephoneTextBox.Text;
            cust.EmailAddress = EmailAddressTextBox.Text;
            //email is not null or empty
            if (string.IsNullOrWhiteSpace(cust.EmailAddress))
            {
                MessageBox.Show("Email address cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //email is unique
            if (_customerService.GetAll().Any(c => c.EmailAddress == cust.EmailAddress && c.CustomerId != cust.CustomerId))
            {
                MessageBox.Show("Email address already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cust.CustomerBirthday = BirthdayDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(BirthdayDatePicker.SelectedDate.Value) : null;
            if(!string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                cust.Password = PasswordTextBox.Text;
            }
            _customerService.UpdateCustomer(cust);
            MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
