﻿using System;
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

    public partial class CustomerDetailWindow : Window
    {
        private CustomerService _customerService = new();
        public Customer? SelectedCustomer { get; set; }
        public CustomerDetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedCustomer != null) FillElement();
        }

        private void FillElement()
        {
            if (SelectedCustomer == null) return;
            CustomerIdTextBox.Text = SelectedCustomer.CustomerId.ToString();
            CustomerIdTextBox.IsEnabled = false; // Disable editing of CustomerId
            CustomerFullNameTextBox.Text = SelectedCustomer.CustomerFullName ?? string.Empty;
            TelephoneTextBox.Text = SelectedCustomer.Telephone ?? string.Empty;
            EmailAddressTextBox.Text = SelectedCustomer.EmailAddress;
            BirthdayDatePicker.SelectedDate = SelectedCustomer.CustomerBirthday?.ToDateTime(TimeOnly.MinValue);
            StatusTextBox.Text = SelectedCustomer.CustomerStatus?.ToString() ?? string.Empty;
            //StatusTextBox.IsEnabled = false;
            PasswordTextBox.Text = SelectedCustomer.Password ?? string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new();
            if (SelectedCustomer != null) //neu la update --> lay thang id do
            {
                customer = _customerService.GetCustomerById(SelectedCustomer.CustomerId)!;
            }
            customer.CustomerFullName = CustomerFullNameTextBox.Text;
            customer.Telephone = TelephoneTextBox.Text;
            customer.EmailAddress = EmailAddressTextBox.Text;
            //email is not null or empty
            if (string.IsNullOrWhiteSpace(customer.EmailAddress))
            {
                MessageBox.Show("Email address cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //email is unique
            if (_customerService.GetAll().Any(c => c.EmailAddress == customer.EmailAddress && c.CustomerId != customer.CustomerId))
            {
                MessageBox.Show("Email address already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            customer.CustomerBirthday = BirthdayDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(BirthdayDatePicker.SelectedDate.Value) : null;
            customer.CustomerStatus = byte.TryParse(StatusTextBox.Text, out byte status) ? status : (byte?)null;
            //if creating, status is 1
            //if updating, status is the same as before
            //customer.CustomerStatus = SelectedCustomer == null ? (byte)1 : SelectedCustomer.CustomerStatus;
            customer.Password = PasswordTextBox.Text;

            if (SelectedCustomer == null)
            {
                // Add new customer
                _customerService.AddCustomer(customer);
            }
            else
            {
                // Update existing customer
                _customerService.UpdateCustomer(customer);
            }
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to close?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
