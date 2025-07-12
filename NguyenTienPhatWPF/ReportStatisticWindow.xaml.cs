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
    /// Interaction logic for ReportStatisticWindow.xaml
    /// </summary>
    public partial class ReportStatisticWindow : Window
    {
        private BookingDetailService _bookingDetailService = new();
        private RoomInformationService _roomInformationService = new();
        private RoomTypeService _roomTypeService = new();
        public ReportStatisticWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select both start and end dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateOnly start = DateOnly.FromDateTime(StartDatePicker.SelectedDate ?? DateTime.Now);
            DateOnly end = DateOnly.FromDateTime(EndDatePicker.SelectedDate ?? DateTime.Now.AddDays(1));
            if(start >= end)
            {
                MessageBox.Show("Start date must be earlier than end date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var bookingDetails = _bookingDetailService.GetInPeriod(start, end);
            var rs = bookingDetails
                .GroupBy(bd => new { bd.RoomId })
                .Select(g => new StatisticDTO()
                {
                    RoomNumber = _roomInformationService.GetRoomInformationById(g.Key.RoomId)?.RoomNumber ?? "Unknown",
                    RoomTypeName = _roomInformationService.GetRoomInformationById(g.Key.RoomId)?.RoomType.RoomTypeName ?? "Unknown",
                    NumberOfBookings = g.Count(),
                    TotalRevenue = g.Sum(bd => bd.ActualPrice ?? 0),
                    RoomStatus = _roomInformationService.GetRoomInformationById(g.Key.RoomId)?.RoomStatus
                })
                .OrderByDescending(s => s.TotalRevenue)
                .ToList();
            FillDataGrid(rs);
        }

        private void FillDataGrid(List<StatisticDTO> list)
        {
            BookingDataGrid.ItemsSource = null;
            BookingDataGrid.ItemsSource = list;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            //thông báo có muốn tắt không
            MessageBoxResult rs = MessageBox.Show("Are you sure you want to close?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
