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
    public partial class BookingDetailWindowV2 : Window
    {
        private CustomerService customerService = new CustomerService();
        private RoomInformationService roomInformationService = new RoomInformationService();
        private BookingReservationService bookingReservationService = new BookingReservationService();
        private BookingDetailService bookingDetailService = new BookingDetailService();

        private List<BookingDetail> selectedBookingDetails = new();
        private BookingReservation bookingReservation = new BookingReservation();
        public BookingDetailWindowV2()
        {
            InitializeComponent();
        }
        private void FillData()
        {
            var customers = customerService.GetAll();
            var rooms = roomInformationService.GetAll();
            CustomerComboBox.ItemsSource = customers;
            CustomerComboBox.DisplayMemberPath = "CustomerFullName";
            CustomerComboBox.SelectedValuePath = "CustomerId";
            RoomComboBox.ItemsSource = rooms;
            RoomComboBox.DisplayMemberPath = "RoomNumber";
            RoomComboBox.SelectedValuePath = "RoomId";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillData();

            var bookingReservations = bookingReservationService.GetAll();
            //lấy booking reservation có id lớn nhất
            if (bookingReservations.Count > 0)
            {
                bookingReservation.BookingReservationId = bookingReservations.OrderByDescending(br => br.BookingReservationId).First().BookingReservationId + 1;
            }
            else
            {
                bookingReservation.BookingReservationId = 1; // nếu không có booking reservation nào thì tạo mới với id 1
            }
            bookingReservation.BookingDate = DateOnly.FromDateTime(DateTime.Now);
            bookingReservation.TotalPrice = 0; // Khởi tạo tổng giá

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var customerId = CustomerComboBox.SelectedValue as int?;
            var roomId = RoomComboBox.SelectedValue as int?;
            if (customerId == null || roomId == null)
            {
                MessageBox.Show("Please select a customer and a room.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //kiểm tra xem có bị trùng roomId khong
            if (selectedBookingDetails.Any(bd => bd.RoomId == roomId.Value))
            {
                MessageBox.Show("This room has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateOnly startDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate ?? DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate ?? DateTime.Now);
            //kiểu tra ngày bắt đầu có lớn hơn hôm nay không
            if (startDate < DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("Start date cannot be earlier than today.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //kiểm tra ngày bắt đầu và kết thúc
            if (startDate >= endDate)
            {
                MessageBox.Show("Start date must be earlier than end date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!AvailableRoom(roomId.Value, startDate, endDate))
            {
                MessageBox.Show("This room is not available for the selected dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var roomInformation = roomInformationService.GetRoomInformationById(roomId.Value);
            if (roomInformation!.RoomStatus != 1)
            {
                MessageBox.Show("This room is locked.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //tìm room
            bookingReservation.CustomerId = customerId.Value;
            bookingReservation.BookingStatus = 1;
            //tạo booking detail với thông tin đã chọn
            BookingDetail bookingDetail = new BookingDetail()
            {
                BookingReservationId = bookingReservation.BookingReservationId,
                RoomId = roomId.Value,
                StartDate = startDate,
                EndDate = endDate,
                ActualPrice = roomInformation!.RoomPricePerDay * (endDate.DayNumber - startDate.DayNumber)
            };
            //add vào list 
            selectedBookingDetails.Add(bookingDetail);
            bookingReservation.TotalPrice += roomInformation!.RoomPricePerDay * (endDate.DayNumber - startDate.DayNumber);
            TotalPriceTextBox.Text = bookingReservation.TotalPrice.ToString();
            //thông báo add thành cônng
            MessageBox.Show("Booking detail added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool AvailableRoom(int roomId, DateOnly startDate, DateOnly endDate)
        {
            var room = roomInformationService.GetRoomInformationById(roomId);
            var bookingDetail = bookingDetailService.GetBookingDetailsByRoomId(roomId);
            if (bookingDetail.Count == 0) return true; //nếu không có booking detail nào thì phòng trống
            //kiểm tra xem phòng có bị trùng lịch không
            foreach (var bd in bookingDetail)
            {
                if ((startDate >= bd.StartDate && startDate <= bd.EndDate) || endDate >= bd.StartDate && endDate <= bd.EndDate)
                {
                    return false; //phòng đã được đặt trong khoảng thời gian này
                }
            }
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //hỏi xem có thật sự muốn lưu không
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save the booking details?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            if (selectedBookingDetails.Count == 0)
            {
                MessageBox.Show("Please add at least one booking detail.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //lưu booking reservation
            bookingReservationService.AddBookingReservation(bookingReservation);
            //lưu tất cả booking detail
            foreach (var bookingDetail in selectedBookingDetails)
            {
                bookingDetailService.AddBookingDetail(bookingDetail);
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

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var roomId = RoomComboBox.SelectedValue as int?;
            var roomInformation = roomInformationService.GetRoomInformationById(roomId!.Value);
            PriceTextBox.Text = roomInformation!.RoomPricePerDay.ToString();
        }
    }
}
