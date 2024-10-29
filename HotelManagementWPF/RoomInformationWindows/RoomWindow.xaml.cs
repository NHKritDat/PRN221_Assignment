using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using HotelManagementWPF.BookingReservationWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagementWPF.RoomInformationWindows
{
    /// <summary>
    /// Interaction logic for RoomWindow.xaml
    /// </summary>
    public partial class RoomWindow : Window
    {
        private IRoomInformationService _roomInformationService;
        private IRoomTypeService _roomTypeService;
        private IBookingDetailService _bookDetailService;
        private RoomDto selectedRoom;
        public RoomWindow()
        {
            InitializeComponent();
            _roomInformationService = new RoomInformationService();
            _roomTypeService = new RoomTypeService();
            _bookDetailService = new BookingDetailService();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to exit app?", "Exit App!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.ShowDialog();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            BookingReservationWindow bookingReservationWindow = new BookingReservationWindow();
            bookingReservationWindow.ShowDialog();
        }
        private void LoadData()
        {
            dtgRoom.ItemsSource = ToListDto(_roomInformationService.GetRoomInformations(), _roomTypeService.GetRoomTypes());
        }

        private RoomDto ToDto(RoomInformation roomInformation, RoomType roomType)
        {
            RoomDto roomDto = new RoomDto();
            roomDto.RoomId = roomInformation.RoomId;
            roomDto.RoomNumber = roomInformation.RoomNumber;
            roomDto.RoomDetailDescription = roomInformation.RoomDetailDescription;
            roomDto.RoomMaxCapacity = roomInformation.RoomMaxCapacity;
            roomDto.RoomTypeId = roomInformation.RoomTypeId;
            roomDto.RoomTypeName = roomType.RoomTypeName;
            roomDto.TypeDescription = roomType.TypeDescription;
            roomDto.TypeNote = roomType.TypeNote;
            roomDto.RoomStatus = roomInformation.RoomStatus;
            roomDto.RoomPricePerDay = roomInformation.RoomPricePerDay;
            return roomDto;
        }
        private List<RoomDto> ToListDto(List<RoomInformation> roomInformations, List<RoomType> roomTypes)
        {
            List<RoomDto> roomDtos = new List<RoomDto>();
            roomInformations.ForEach(roomInformation => roomTypes.ForEach(roomType =>
            {
                if (roomType.RoomTypeId == roomInformation.RoomTypeId)
                    roomDtos.Add(ToDto(roomInformation, roomType));
            }));
            return roomDtos;
        }

        private void Loaded_Room(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateRoom createRoom = new CreateRoom();
            createRoom.ShowDialog();
            LoadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dtgRoom.ItemsSource = ToListDto(_roomInformationService.GetRoomInformations(), _roomTypeService.GetRoomTypes()).Where(r => r.RoomTypeName.ToLower().Contains(txtSearch.Text.ToLower()));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to delete this booking?", "Delete Booking!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                bool has = false;
                _bookDetailService.GetBookingDetails().ForEach(b =>
                {
                    if (b.RoomId.Equals(selectedRoom.RoomId)) has = true;
                });

                if (has)
                {
                    RoomInformation roomInformation = ToRoomInformation(selectedRoom);
                    roomInformation.RoomStatus = 0;
                    if (_roomInformationService.UpdateRoomInformation(roomInformation))
                    {
                        MessageBox.Show("Remove successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        return;
                    }
                }
                else
                {
                    if (_roomInformationService.RemoveRoomInformation(selectedRoom.RoomId))
                    {
                        MessageBox.Show("Remove successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        return;
                    }
                }
                MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RoomInformation ToRoomInformation(RoomDto roomDto)
        {
            RoomInformation roomInformation = new RoomInformation();
            roomInformation.RoomId = roomDto.RoomId;
            roomInformation.RoomNumber = roomDto.RoomNumber;
            roomInformation.RoomDetailDescription = roomDto.RoomDetailDescription;
            roomInformation.RoomMaxCapacity = roomDto.RoomMaxCapacity;
            roomInformation.RoomTypeId = roomDto.RoomTypeId;
            roomInformation.RoomStatus = roomDto.RoomStatus;
            roomInformation.RoomPricePerDay = roomDto.RoomPricePerDay;
            return roomInformation;
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateRoom updateRoom = new UpdateRoom();
            updateRoom.Selected = selectedRoom;
            updateRoom.ShowDialog();
            LoadData();
        }

        private void dtgRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgRoom.SelectedItems.Count > 0)
                selectedRoom = dtgRoom.SelectedItems[0] as RoomDto;
        }

        private void btnJson_Click(object sender, RoutedEventArgs e)
        {
            string roominformation = "..\\Hotel_Daos\\roominformation";
            string roomtype = "..\\Hotel_Daos\\roomtype";
            var roominformations = ToListDto(_roomInformationService.GetRoomInformations());
            var roomtypes = ToListDto(_roomTypeService.GetRoomTypes());
            _roomInformationService.WriteFile(roominformations, roominformation, ".json");
            _roomTypeService.WriteFile(roomtypes, roomtype, ".json");
            MessageBox.Show("Export successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnXml_Click(object sender, RoutedEventArgs e)
        {
            string roominformation = "..\\Hotel_Daos\\roominformation";
            string roomtype = "..\\Hotel_Daos\\roomtype";
            var roominformations = ToListDto(_roomInformationService.GetRoomInformations());
            var roomtypes = ToListDto(_roomTypeService.GetRoomTypes());
            _roomInformationService.WriteFile(roominformations, roominformation, ".xml");
            _roomTypeService.WriteFile(roomtypes, roomtype, ".xml");
            MessageBox.Show("Export successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private RoomInformationDto ToDto(RoomInformation roomInformation)
        {
            RoomInformationDto dto = new RoomInformationDto();
            dto.RoomId = roomInformation.RoomId;
            dto.RoomNumber = roomInformation.RoomNumber;
            dto.RoomDetailDescription = roomInformation.RoomDetailDescription;
            dto.RoomTypeId = roomInformation.RoomTypeId;
            dto.RoomMaxCapacity = roomInformation.RoomMaxCapacity;
            dto.RoomStatus = roomInformation.RoomStatus;
            dto.RoomPricePerDay = roomInformation.RoomPricePerDay;
            return dto;
        }
        private List<RoomInformationDto> ToListDto(List<RoomInformation> roomInformations)
        {
            List<RoomInformationDto> dtos = new List<RoomInformationDto>();
            roomInformations.ForEach(roomInformation => dtos.Add(ToDto(roomInformation)));
            return dtos;
        }
        private RoomTypeDto ToDto(RoomType roomType)
        {
            RoomTypeDto dto = new RoomTypeDto();
            dto.RoomTypeId = roomType.RoomTypeId;
            dto.RoomTypeName = roomType.RoomTypeName;
            dto.TypeDescription = roomType.TypeDescription;
            dto.TypeNote = roomType.TypeNote;
            return dto;
        }
        private List<RoomTypeDto> ToListDto(List<RoomType> roomTypes)
        {
            List<RoomTypeDto> dtos = new List<RoomTypeDto>();
            roomTypes.ForEach(r => dtos.Add(ToDto(r)));
            return dtos;
        }
    }
}
