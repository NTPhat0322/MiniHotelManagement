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
    
    public partial class CustomerWindow : Window
    {
        public Customer? Customer { get; set; }
        private BookingReservationService _bookingReservationService = new BookingReservationService();
        private BookingDetailService _bookingDetailService = new BookingDetailService();
        private RoomInformationService _roomInformationService = new RoomInformationService();
        public CustomerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Customer == null)
            {
                this.Close();
                return;
            }
            this.WelcomeMsgLable.Content = $"Welcome {Customer.CustomerFullName}";
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            //lấy danh sách booking reservation của khách hàng
            var bookingReservations = _bookingReservationService.GetBookingReservationByCustomerId(Customer!.CustomerId);
            if (bookingReservations.Count == 0)
            {
                return;
            }
            List<ReservationCustomerDTO> rs = new();
            foreach(var bookingReservation in bookingReservations)
            {
                foreach(var bookingDetail in bookingReservation.BookingDetails)
                {
                    var roomInformation = _roomInformationService.GetRoomInformationById(bookingDetail.RoomId);
                    rs.Add(new ReservationCustomerDTO()
                    {
                        BookingReservationId = bookingReservation.BookingReservationId,
                        BookingDate = bookingReservation.BookingDate,
                        TotalPrice = bookingReservation.TotalPrice,
                        BookingStatus = bookingReservation.BookingStatus,
                        RoomNumber = roomInformation?.RoomNumber ?? "Unknown",
                        StartDate = bookingDetail.StartDate,
                        EndDate = bookingDetail.EndDate,
                        ActualPrice = bookingDetail.ActualPrice
                    });
                }
            }

            ReservationDataGrid.ItemsSource = rs;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerEditProfileWindow customerEditProfileWindow = new();
            customerEditProfileWindow.Customer = Customer;
            customerEditProfileWindow.CustomerIdTextBox.IsEnabled = false;
            customerEditProfileWindow.ShowDialog();
        }
    }
}
