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
    public partial class RoomManagementWindow : Window
    {
        private RoomInformationService _roomInformationService = new();
        private BookingDetailService _bookingDetailService = new();
        public RoomManagementWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }
        private void FillDataGrid()
        {
            RoomDataGrid.ItemsSource = null;
            RoomDataGrid.ItemsSource = _roomInformationService.GetAll();
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
            RoomInformation? selectedRoom = RoomDataGrid.SelectedItem as RoomInformation;
            if (selectedRoom == null)
            {
                MessageBox.Show("Please select a room to delete.", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult rs = MessageBox.Show($"Are you sure you want to delete room?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                //get room by id
                var room = _roomInformationService.GetRoomInformationById(selectedRoom.RoomId);
                //deleting when room information does not exist in booking detail
                var bookingDetail = _bookingDetailService.GetBookingDetailByRoomId(room!.RoomId);
                if(bookingDetail == null)
                {
                    _roomInformationService.DeleteRoomInformation(room);
                }
                //changing status if room information exists in booking detail
                else
                {
                    room.RoomStatus = 0;
                    _roomInformationService.UpdateRoomInformation(room);
                }
            }
            FillDataGrid();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //search by max capacity
            string searchText = SearchTextBox.Text.Trim();
            if(string.IsNullOrEmpty(searchText))
            {
                RoomDataGrid.ItemsSource = null;
                RoomDataGrid.ItemsSource = _roomInformationService.GetAll();
                return;
            }
            int max;
            bool rs = int.TryParse(searchText, out max);
            if (rs)
            {
                RoomDataGrid.ItemsSource = null;
                RoomDataGrid.ItemsSource = _roomInformationService.GetByMaxCapacity(max);
            }
            else
            {
                MessageBox.Show("Please enter a valid number for max capacity.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            RoomInformation? selectedRoom = RoomDataGrid.SelectedItem as RoomInformation;
            if (selectedRoom == null)
            {
                MessageBox.Show("Please select a room to update.", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RoomDetailWindow roomDetailWindow = new RoomDetailWindow();
            roomDetailWindow.SelectedRoom = selectedRoom;
            roomDetailWindow.ShowDialog();
            FillDataGrid();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            RoomDetailWindow roomDetailWindow = new RoomDetailWindow();
            roomDetailWindow.SelectedRoom = null;
            roomDetailWindow.RoomIdTextBox.IsEnabled = false;
            roomDetailWindow.ShowDialog();
            FillDataGrid();
        }
    }
}
