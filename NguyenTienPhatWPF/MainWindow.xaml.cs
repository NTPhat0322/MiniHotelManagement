using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NguyenTienPhatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CustManagementButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerManagementWindow customerManagementWindow = new();
            this.Hide();
            customerManagementWindow.ShowDialog();
            this.ShowDialog();
        }

        private void RoomManagementButton_Click(object sender, RoutedEventArgs e)
        {
            RoomManagementWindow roomManagementWindow = new();
            this.Hide();
            roomManagementWindow.ShowDialog();
            this.ShowDialog();
        }

        private void BookingManagementButton_Click(object sender, RoutedEventArgs e)
        {
            BookingManagement bookingManagement = new();
            this.Hide();
            bookingManagement.ShowDialog();
            this.ShowDialog();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportStatisticWindow reportStatisticWindow = new();
            this.Hide();
            reportStatisticWindow.ShowDialog();
            this.ShowDialog();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure to quit", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}