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
    public partial class RoomDetailWindow : Window
    {
        private RoomInformationService _roomInformationService = new();
        private RoomTypeService _roomTypeService = new();

        public RoomInformation? SelectedRoom { get; set; }
        public RoomDetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
            if(SelectedRoom != null)
            {
                FillElement(SelectedRoom);
            }
        }

        private void FillElement(RoomInformation selectedRoom)
        {
            if(selectedRoom == null) return;
            RoomIdTextBox.Text = selectedRoom.RoomId.ToString();
            RoomIdTextBox.IsEnabled = false;
            RoomNumberTextBox.Text = selectedRoom.RoomNumber ?? string.Empty;
            RoomDetailDescriptionTextBox.Text = selectedRoom.RoomDetailDescription ?? string.Empty;
            RoomMaxCapacityTextBox.Text = selectedRoom.RoomMaxCapacity?.ToString() ?? string.Empty;
            RoomStatusTextBox.Text = selectedRoom.RoomStatus?.ToString() ?? string.Empty;
            RoomPricePerDayTextBox.Text = selectedRoom.RoomPricePerDay?.ToString("F2") ?? string.Empty;
            RoomTypeIdComboBox.SelectedValue = selectedRoom.RoomTypeId;
        }

        private void FillComboBox()
        {
            var roomTypes = _roomTypeService.GetAll();
            RoomTypeIdComboBox.ItemsSource = roomTypes;
            RoomTypeIdComboBox.DisplayMemberPath = "RoomTypeName";
            RoomTypeIdComboBox.SelectedValuePath = "RoomTypeId";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //nếu là update thì lấy id của room
            //nêu là create thì tạo mới
            RoomInformation room = new();
            if (SelectedRoom != null) // nếu là update
            {
                room = _roomInformationService.GetRoomInformationById(SelectedRoom.RoomId)!;
            }
            //cập nhật các trường thông tin
            //validate đầu vào room number
            if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text))
            {
                MessageBox.Show("Room Number cannot be empty.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            room.RoomNumber = RoomNumberTextBox.Text;
            room.RoomDetailDescription = RoomDetailDescriptionTextBox.Text;
            //validate đầu vào max capacity
            if (!int.TryParse(RoomMaxCapacityTextBox.Text, out int maxCapacity) || maxCapacity <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for Max Capacity.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            room.RoomMaxCapacity = maxCapacity;
            //validate đầu vào status
            if (!byte.TryParse(RoomStatusTextBox.Text, out byte status) || status < 0 || status > 1)
            {
                MessageBox.Show("Please enter a valid status (0 or 1).", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            room.RoomStatus = status;
            //validate đầu vào price per day
            if (!decimal.TryParse(RoomPricePerDayTextBox.Text, out decimal pricePerDay) || pricePerDay < 0)
            {
                MessageBox.Show("Please enter a valid positive number for Price Per Day.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            room.RoomPricePerDay = pricePerDay;
            //validate đầu vào room type id
            if (RoomTypeIdComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a Room Type.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            room.RoomTypeId = (int)RoomTypeIdComboBox.SelectedValue;
            if(SelectedRoom == null) // nếu là create
            {
                _roomInformationService.AddRoomInformation(room);
            }
            else // nếu là update
            {
                _roomInformationService.UpdateRoomInformation(room);
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
