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
using DAL.DTOs;
using DAL.Entities;

namespace NguyenTienPhatWPF
{
    /// <summary>
    /// Interaction logic for BookingManagement.xaml
    /// </summary>
    public partial class BookingManagement : Window
    {
        private BookingReservationService _bookingReservationService = new();
        private BookingDetailService _bookingDetailService = new();

        public BookingManagement()
        {
            InitializeComponent();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            BookingInformationDTO? bookingInformationDTO = BookingDataGrid.SelectedItem as BookingInformationDTO;
            if (bookingInformationDTO == null)
            {
                MessageBox.Show("Please select a booking to delete.", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult rs = MessageBox.Show($"Are you sure you want to delete?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(rs == MessageBoxResult.Yes)
            {
                BookingDetail? bookingDetail = _bookingDetailService.GetByRoomAndReservationId(bookingInformationDTO.RoomId, bookingInformationDTO.BookingReservationId);
                if (bookingDetail == null)
                {
                    MessageBox.Show("Booking detail not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _bookingDetailService.DeleteBookingDetail(bookingDetail);
                FillDataGrid();
                MessageBox.Show("Booking deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            BookingDetailWindowV2 bookingDetailWindow = new();
            bookingDetailWindow.StatusTextBox.IsEnabled = false;
            bookingDetailWindow.ShowDialog();
            FillDataGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            BookingInformationDTO? selectedBooking = BookingDataGrid.SelectedItem as BookingInformationDTO;
            if(selectedBooking == null)
            {
                MessageBox.Show("Please select a booking to update.", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BookingDetailUpdateWindow UpdateWindow = new();
            UpdateWindow.BookingInformationDTO = selectedBooking;
            UpdateWindow.CustomerComboBox.IsEnabled = false;
            UpdateWindow.RoomComboBox.IsEnabled = false;
            UpdateWindow.ShowDialog();
            FillDataGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            BookingDataGrid.ItemsSource = null;
            BookingDataGrid.ItemsSource = _bookingReservationService.CreateBookingInforList();
        }
    }
}
