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

namespace HotelManagementWPF.BookingReservationWindows
{
    /// <summary>
    /// Interaction logic for CreatBookingWindow.xaml
    /// </summary>
    public partial class CreatBookingWindow : Window
    {
        private ICustomerService _customerService;
        private IRoomInformationService _roomInformationService;
        private IBookingDetailService _bookingDetailService;
        private IBookingReservationService _bookingReservationService;
        private List<BookingReservation> bookingReservations;
        private List<Customer> customers;
        private List<RoomInformation> rooms;
        public CreatBookingWindow()
        {
            InitializeComponent();
            _bookingDetailService = new BookingDetailService();
            _bookingReservationService = new BookingReservationService();
            _customerService = new CustomerService();
            _roomInformationService = new RoomInformationService();
            customers = _customerService.GetCustomers();
            rooms = _roomInformationService.GetRoomInformations();
            bookingReservations = _bookingReservationService.GetBookingReservations();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var customerDatas = new List<string>();
            customers.ForEach(c => customerDatas.Add(c.CustomerFullName));
            cboCustomer.ItemsSource = customerDatas;
            var roomDatas = new List<string>();
            rooms.ForEach(r => roomDatas.Add(r.RoomNumber));
            libRoom.ItemsSource = roomDatas;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startDate = DateTime.Parse(datStart.Text);
                DateTime endDate = DateTime.Parse(datEnd.Text);
                DateTime bookingDate = DateTime.Parse(datBooking.Text);
                byte status = Byte.Parse(txtStatus.Text);
                var selectedRooms = new List<RoomInformation>();
                foreach (var item in libRoom.SelectedItems.Cast<string>().ToList())
                {
                    selectedRooms.Add(rooms.Where(r => r.RoomNumber.Equals(item)).ToList()[0]);
                }
                var selectedCustomer = customers[cboCustomer.SelectedIndex];
                decimal? totalPrice = 0;
                decimal? actualPrice = 0;
                int days = (endDate - startDate).Days;
                int bookingReservationId = bookingReservations.Count + 1;
                List<BookingDetail> bookingDetails = new List<BookingDetail>();

                selectedRooms.ForEach(r =>
                {
                    BookingDetail bookingDetail = new BookingDetail();
                    bookingDetail.BookingReservationId = bookingReservationId;
                    bookingDetail.RoomId = r.RoomId;
                    bookingDetail.StartDate = startDate;
                    bookingDetail.EndDate = endDate;
                    actualPrice = days * r.RoomPricePerDay;
                    totalPrice += actualPrice;
                    bookingDetail.ActualPrice = actualPrice;
                    bookingDetails.Add(bookingDetail);
                });

                BookingReservation bookingReservation = new BookingReservation();
                bookingReservation.TotalPrice = totalPrice;
                bookingReservation.CustomerId = selectedCustomer.CustomerId;
                bookingReservation.BookingReservationId = bookingReservationId;
                bookingReservation.BookingDate = bookingDate;
                bookingReservation.BookingStatus = status;
                if (!_bookingReservationService.AddBookingReservation(bookingReservation))
                {
                    MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bookingDetails.ForEach(bookingDetail =>
                 {
                     if (!_bookingDetailService.AddBookingDetail(bookingDetail))
                     {
                         MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                         return;
                     }
                 });
                MessageBox.Show("Create successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
