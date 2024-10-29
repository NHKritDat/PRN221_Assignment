using Hotel_BusinessObjects.Models;
using Hotel_Services;
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

namespace HotelManagementWPF.RoomInformationWindows
{
    /// <summary>
    /// Interaction logic for CreateRoom.xaml
    /// </summary>
    public partial class CreateRoom : Window
    {
        private IRoomTypeService _roomTypeService;
        private IRoomInformationService _roomInformationService;
        private List<RoomType> _roomTypes;
        private List<RoomInformation> _roomInformations;
        public CreateRoom()
        {
            InitializeComponent();
            _roomTypeService = new RoomTypeService();
            _roomInformationService = new RoomInformationService();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string roomNumber = txtRoomNumber.Text;
                string description = txtDescription.Text;
                int? capacity = int.Parse(txtCapacity.Text);
                byte? status = Byte.Parse(txtStatus.Text);
                decimal? price = decimal.Parse(txtPrice.Text);
                int type = cboType.SelectedIndex + 1;
                _roomInformations.ForEach(r =>
                {
                    if (r.RoomNumber.Equals(roomNumber))
                    {
                        MessageBox.Show("Invalid input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                });

                RoomInformation roomInformation = new RoomInformation();
                roomInformation.RoomNumber = roomNumber;
                roomInformation.RoomDetailDescription = description;
                roomInformation.RoomMaxCapacity = capacity;
                roomInformation.RoomTypeId = type;
                roomInformation.RoomStatus = status;
                roomInformation.RoomPricePerDay = price;

                if (!_roomInformationService.AddRoomInformation(roomInformation))
                {
                    MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Create successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadType(object sender, RoutedEventArgs e)
        {
            _roomInformations = _roomInformationService.GetRoomInformations();
            _roomTypes = _roomTypeService.GetRoomTypes();
            List<string> types = new List<string>();
            _roomTypes.ForEach(type => types.Add(type.RoomTypeName));
            cboType.ItemsSource = types;
        }
    }
}
