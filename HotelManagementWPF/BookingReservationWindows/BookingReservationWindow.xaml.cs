using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using HotelManagementWPF.RoomInformationWindows;
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

namespace HotelManagementWPF.BookingReservationWindows
{
    /// <summary>
    /// Interaction logic for BookingReservationWindow.xaml
    /// </summary>
    public partial class BookingReservationWindow : Window
    {
        private IBookingReservationService _reservationService;
        private IBookingDetailService _detailService;
        private ICustomerService _customerService;
        private IRoomInformationService _roomInformationService;
        private BookingDto? selected;
        public BookingReservationWindow()
        {
            InitializeComponent();
            _detailService = new BookingDetailService();
            _reservationService = new BookingReservationService();
            _customerService = new CustomerService();
            _roomInformationService = new RoomInformationService();
        }

        private void dtgBooking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgBooking.SelectedItems.Count > 0)
            {
                selected = dtgBooking.SelectedItems[0] as BookingDto;
            }
        }

        private void Loaded_Booking(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dtgBooking.ItemsSource = ToListDto(_reservationService.GetBookingReservations(), _detailService.GetBookingDetails());
        }

        private BookingDto ToDto(BookingReservation reservation, BookingDetail detail, Customer customer, RoomInformation roomInformation)
        {
            BookingDto dto = new BookingDto();
            dto.BookingReservationId = reservation.BookingReservationId;
            dto.CustomerId = reservation.CustomerId;
            dto.CustomerFullName = customer.CustomerFullName;
            dto.RoomId = detail.RoomId;
            dto.RoomNumber = roomInformation.RoomNumber;
            dto.BookingDate = reservation.BookingDate;
            dto.EndDate = detail.EndDate;
            dto.StartDate = detail.StartDate;
            dto.ActualPrice = detail.ActualPrice;
            dto.TotalPrice = reservation.TotalPrice;
            dto.BookingStatus = reservation.BookingStatus;
            return dto;
        }

        private List<BookingDto> ToListDto(List<BookingReservation> reservations, List<BookingDetail> details)
        {
            List<BookingDto> dtos = new List<BookingDto>();
            reservations.ForEach(reservation =>
            {
                details.ForEach(detail =>
                {
                    if (detail.BookingReservationId == reservation.BookingReservationId)
                        dtos.Add(ToDto(reservation, detail, _customerService.GetCustomer(reservation.CustomerId), _roomInformationService.GetRoomInformation(detail.RoomId)));
                });
            });
            return dtos;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.ShowDialog();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dtgBooking.ItemsSource = ToListDto(_reservationService.GetBookingReservations(), _detailService.GetBookingDetails())
                .Where(c =>
                c.CustomerFullName.ToLower().Contains(txtSearch.Text.ToLower()));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to delete this booking?", "Delete Booking!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                if (_detailService.RemoveBookingDetail(selected.BookingReservationId, selected.RoomId))
                {
                    BookingReservation reservation = ToReservation(selected);
                    reservation.TotalPrice = selected.TotalPrice - selected.ActualPrice;
                    if (reservation.TotalPrice > 0)
                    {
                        if (_reservationService.UpdateBookingReservation(reservation))
                        {
                            MessageBox.Show("Remove successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                            return;
                        }
                    }
                    else if (_reservationService.RemoveBookingReservation(reservation.BookingReservationId))
                    {
                        MessageBox.Show("Remove successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        return;
                    }
                }
                MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateBookingWindow updateBookingWindow = new UpdateBookingWindow();
            updateBookingWindow.Selected = selected;
            updateBookingWindow.ShowDialog();
            LoadData();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreatBookingWindow creatBookingWindow = new CreatBookingWindow();
            creatBookingWindow.ShowDialog();
            LoadData();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to exit app?", "Exit App!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private BookingReservation ToReservation(BookingDto bookingDto)
        {
            BookingReservation reservation = new BookingReservation();
            reservation.BookingReservationId = bookingDto.BookingReservationId;
            reservation.BookingDate = bookingDto.BookingDate;
            reservation.TotalPrice = bookingDto.TotalPrice;
            reservation.CustomerId = bookingDto.CustomerId;
            reservation.BookingStatus = bookingDto.BookingStatus;
            return reservation;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RoomWindow roomWindow = new RoomWindow();
            roomWindow.ShowDialog();
        }

        private void btnJson_Click(object sender, RoutedEventArgs e)
        {
            string bookingreservation = "..\\Hotel_Daos\\bookingreservation";
            string bookingdetail = "..\\Hotel_Daos\\bookingdetail";
            var bookingreservations = ToListDto(_reservationService.GetBookingReservations());
            var bookingdetails = ToListDto(_detailService.GetBookingDetails());
            _reservationService.WriteFile(bookingreservations, bookingreservation, ".json");
            _detailService.WriteFile(bookingdetails, bookingdetail, ".json");
            MessageBox.Show("Export successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private BookingDetailDto ToDto(BookingDetail bookingDetail)
        {
            BookingDetailDto dto = new BookingDetailDto();
            dto.BookingReservationId = bookingDetail.BookingReservationId;
            dto.RoomId = bookingDetail.RoomId;
            dto.EndDate = bookingDetail.EndDate;
            dto.StartDate = bookingDetail.StartDate;
            dto.ActualPrice = bookingDetail.ActualPrice;
            return dto;
        }

        private BookingReservationDto ToDto(BookingReservation bookingReservation)
        {
            BookingReservationDto dto = new BookingReservationDto();
            dto.BookingReservationId = bookingReservation.BookingReservationId;
            dto.BookingDate = bookingReservation.BookingDate;
            dto.TotalPrice = bookingReservation.TotalPrice;
            dto.CustomerId = bookingReservation.CustomerId;
            dto.BookingStatus = bookingReservation.BookingStatus;
            return dto;
        }

        private List<BookingReservationDto> ToListDto(List<BookingReservation> bookingReservationList)
        {
            List<BookingReservationDto> dtos = new List<BookingReservationDto>();
            bookingReservationList.ForEach(b => dtos.Add(ToDto(b)));
            return dtos;
        }

        private List<BookingDetailDto> ToListDto(List<BookingDetail> bookingDetailList)
        {
            List<BookingDetailDto> dtos = new List<BookingDetailDto>();
            bookingDetailList.ForEach(b => dtos.Add(ToDto(b)));
            return dtos;
        }

        private void btnXml_Click(object sender, RoutedEventArgs e)
        {
            string bookingreservation = "..\\Hotel_Daos\\bookingreservation";
            string bookingdetail = "..\\Hotel_Daos\\bookingdetail";
            var bookingreservations = ToListDto(_reservationService.GetBookingReservations());
            var bookingdetails = ToListDto(_detailService.GetBookingDetails());
            _reservationService.WriteFile(bookingreservations, bookingreservation, ".xml");
            _detailService.WriteFile(bookingdetails, bookingdetail, ".xml");
            MessageBox.Show("Export successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
