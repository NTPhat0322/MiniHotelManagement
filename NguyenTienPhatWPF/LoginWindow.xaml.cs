using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Extensions.Configuration;

namespace NguyenTienPhatWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private CustomerService CustomerService = new();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private bool IsAdmin(string email, string password)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            string adminEmail = configuration["AdminEmail"];
            string adminPassword = configuration["AdminPassword"];
            return email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && password.Equals(adminPassword, StringComparison.Ordinal);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailAddressTextBox.Text;
            string password = PasswordBox.Password;
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter email and password.", "Login fail", MessageBoxButton.OK);
                return;
            }
            //login admin
            if(IsAdmin(email, password))
            {
                MainWindow mainWindow = new MainWindow();
                this.Hide();
                mainWindow.ShowDialog();
                this.Show();
                //this.EmailAddressTextBox = null;
                this.PasswordBox.Clear();
                return;
            }

            var customer = CustomerService.Login(email, password);
            if (customer is null)
            {
                MessageBox.Show("Invalid email or password", "Invalid data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //login customer
            //MainWindow mainWindowCustomer = new MainWindow();
            //this.Hide();
            //mainWindowCustomer.ShowDialog();
            //return;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure to quit", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
