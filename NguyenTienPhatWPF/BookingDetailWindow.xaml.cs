using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class BookingDetailWindow : Window
    {
        private CustomerService customerService = new CustomerService();
        private RoomTypeService roomTypeService = new RoomTypeService();
        private RoomInformationService roomInformationService = new RoomInformationService();
        private BookingReservationService bookingReservationService = new BookingReservationService();
        private BookingDetailService bookingDetailService = new BookingDetailService();

        private List<BookingDetailNewDTO> selectedBookingDetails = new();
        private BookingReservation bookingReservation = new BookingReservation();
        public BookingDetailWindow()
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
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            //lấy cus id và room id từ combobox
            var customerId = CustomerComboBox.SelectedValue as int?;
            var roomId = RoomComboBox.SelectedValue as int?;

            if (customerId == null || roomId == null)
            {
                MessageBox.Show("Please select a customer and a room.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //thêm thông tin cus vào booking reservation
            bookingReservation.CustomerId = customerId.Value;
            var roomInformation = roomInformationService.GetRoomInformationById(roomId.Value);

            //tạo booking detail với thông tin đã chọn
            //var bookingDetail = new BookingDetail
            //{
            //    BookingReservationId = bookingReservation.BookingReservationId,
            //    RoomId = roomId.Value,
            //    StartDate = DateOnly.FromDateTime(DateTime.Now), // mặc định đặt phòng từ ngày hiện tại, cập nhật sau
            //    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), // mặc định đặt phòng 1 ngày
            //    ActualPrice = roomInformation?.RoomPricePerDay ?? 0 // giá thực tế sẽ được cập nhật sau
            //};
            var bookingDetail = new BookingDetailNewDTO
            {
                BookingReservationId = bookingReservation.BookingReservationId,
                RoomId = roomId.Value,
                StartDate = DateTime.Now, // mặc định đặt phòng từ ngày hiện tại, cập nhật sau
                EndDate = DateTime.Now.AddDays(1), // mặc định đặt phòng 1 ngày
                ActualPrice = roomInformation?.RoomPricePerDay ?? 0 // giá thực tế sẽ được cập nhật sau
            };
            //thêm vào danh sách đã chọn
            selectedBookingDetails.Add(bookingDetail);

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            BookingDetailDataGrid.ItemsSource = null;
            BookingDetailDataGrid.ItemsSource = selectedBookingDetails;
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker == null) return;
            var row = FindParent<DataGridRow>(datePicker);
            if (row != null)
            {
                var bookingDetail = row.Item as BookingDetailNewDTO;
                if (bookingDetail == null) return;
                bookingDetail.StartDate = datePicker.SelectedDate ?? DateTime.Now;
                bookingDetail.EndDate = bookingDetail.StartDate.AddDays(1); // mặc định đặt phòng 1 ngày sau
                // Cập nhật EndDate nếu StartDate thay đổi
                // Lấy room từ id
                var room = roomInformationService.GetRoomInformationById(bookingDetail.RoomId);
                bookingDetail.ActualPrice = room!.RoomPricePerDay * (bookingDetail.EndDate - bookingDetail.StartDate).Days;
                FillDataGrid();
            }
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker == null) return;
            var row = FindParent<DataGridRow>(datePicker);
            if (row != null)
            {
                var bookingDetail = row.Item as BookingDetailNewDTO;
                if (bookingDetail == null) return;
                if (bookingDetail.StartDate >= bookingDetail.EndDate)
                {
                    MessageBox.Show("Start date must be earlier than end date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    bookingDetail.StartDate = DateTime.Now;// reset to current date
                    bookingDetail.EndDate = bookingDetail.StartDate.AddDays(1); // reset end date to 1 day later
                    FillDataGrid(); // cập nhật lại DataGrid
                    return;
                }
                bookingDetail.EndDate = datePicker.SelectedDate ?? DateTime.Now;
                // Lấy room từ id
                var room = roomInformationService.GetRoomInformationById(bookingDetail.RoomId);
                bookingDetail.ActualPrice = room!.RoomPricePerDay * (bookingDetail.EndDate - bookingDetail.StartDate).Days;
                FillDataGrid();
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
    }
}
