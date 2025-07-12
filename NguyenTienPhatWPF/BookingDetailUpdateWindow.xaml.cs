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

namespace NguyenTienPhatWPF
{
    /// <summary>
    /// Interaction logic for BookingDetailUpdateWindow.xaml
    /// </summary>
    public partial class BookingDetailUpdateWindow : Window
    {
        private CustomerService customerService = new CustomerService();
        private RoomInformationService roomInformationService = new RoomInformationService();
        private BookingReservationService bookingReservationService = new BookingReservationService();
        private BookingDetailService bookingDetailService = new BookingDetailService();
        public BookingInformationDTO? BookingInformationDTO { get; set; }
        public BookingDetailUpdateWindow()
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

        private void FillDataToUpdate(BookingInformationDTO bookingInformationDTO)
        {
            if (bookingInformationDTO == null) return;
            CustomerComboBox.SelectedValue = bookingInformationDTO.CustomerId;
            RoomComboBox.SelectedValue = bookingInformationDTO.RoomId;
            var room = roomInformationService.GetRoomInformationById(bookingInformationDTO.RoomId);
            PriceTextBox.Text = room!.RoomPricePerDay.ToString();
            StartDatePicker.SelectedDate = bookingInformationDTO.StartDate.ToDateTime(new TimeOnly(0, 0));
            EndDatePicker.SelectedDate = bookingInformationDTO.EndDate.ToDateTime(new TimeOnly(0, 0));
            TotalPriceTextBox.Text = bookingInformationDTO.TotalPrice.ToString();
            StatusTextBox.Text = bookingInformationDTO.BookingStatus.HasValue ? bookingInformationDTO.BookingStatus.Value.ToString() : "Unknown";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillData();
            if (BookingInformationDTO != null)
            {
                FillDataToUpdate(BookingInformationDTO);
            }
        }

        private bool AvailableRoom(int roomId, DateOnly startDate, DateOnly endDate)
        {
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

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var roomId = RoomComboBox.SelectedValue as int?;
            var roomInformation = roomInformationService.GetRoomInformationById(roomId!.Value);
            PriceTextBox.Text = roomInformation!.RoomPricePerDay.ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to close?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //lấy booking detail từ BookingInformationDTO
            var oldBookingDetail = bookingDetailService.GetByRoomAndReservationId(BookingInformationDTO!.RoomId, BookingInformationDTO.BookingReservationId);
            var oldBookingReservation = bookingReservationService.GetbookingReservationById(BookingInformationDTO.BookingReservationId);

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

            //status
            var statusTmp = StatusTextBox.Text;
            byte? status = byte.TryParse(statusTmp, out var b) ? b : (byte?)null;
            if (status == null || (status != 0 && status != 1))
            {
                MessageBox.Show("Please enter a valid booking status (0 or 1).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var room = roomInformationService.GetRoomInformationById(BookingInformationDTO.RoomId);
            oldBookingDetail!.StartDate = startDate;
            oldBookingDetail.EndDate = endDate;
            //old price 
            var oldPrice = oldBookingDetail.ActualPrice;
            oldBookingDetail.ActualPrice = room!.RoomPricePerDay * (endDate.DayNumber - startDate.DayNumber);

            oldBookingReservation!.TotalPrice = oldBookingReservation!.TotalPrice - oldPrice + oldBookingDetail.ActualPrice;
            oldBookingReservation.BookingStatus = status.Value;
            bookingReservationService.UpdateBookingReservation(oldBookingReservation);
            bookingDetailService.UpdateBookingDetail(oldBookingDetail);
            this.Close();
        }
    }
}
