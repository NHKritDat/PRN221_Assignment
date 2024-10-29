using Hotel_BusinessObjects.Dtos;
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

namespace HotelManagementWPF.ProfileWindow
{
    /// <summary>
    /// Interaction logic for CustomerProfile.xaml
    /// </summary>
    public partial class CustomerProfile : Window
    {
        private ICustomerService _customerService;
        private IBookingReservationService _bookingReservationService;
        public Customer CurrentProfile { get; set; }
        public CustomerProfile()
        {
            InitializeComponent();
            _bookingReservationService = new BookingReservationService();
            _customerService = new CustomerService();
            CurrentProfile = new Customer();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to exit app?", "Exit App!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            txtFullName.Text = CurrentProfile.CustomerFullName;
            txtEmail.Text = CurrentProfile.EmailAddress;
            txtTelephone.Text = CurrentProfile.Telephone;
            datBirthday.Text = CurrentProfile.CustomerBirthday.ToString();
            txtPassword.Password = CurrentProfile.Password;

            dtgHistory.ItemsSource = ToListDto(_bookingReservationService.GetBookingReservations().Where(b => b.CustomerId == CurrentProfile.CustomerId).ToList());
        }

        private List<BookingHistoryDto> ToListDto(List<BookingReservation> history)
        {
            List<BookingHistoryDto> bookingHistoryDtos = new List<BookingHistoryDto>();
            history.ForEach(h => bookingHistoryDtos.Add(ToDto(h)));
            return bookingHistoryDtos;
        }

        private BookingHistoryDto ToDto(BookingReservation bookingReservation)
        {
            BookingHistoryDto dto = new BookingHistoryDto();
            dto.BookingDate = bookingReservation.BookingDate;
            dto.BookingStatus = bookingReservation.BookingStatus;
            dto.TotalPrice = bookingReservation.TotalPrice;
            dto.CustomerFullName = CurrentProfile.CustomerFullName;
            return dto;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer.CustomerId = CurrentProfile.CustomerId;
                customer.CustomerFullName = txtFullName.Text;
                customer.Telephone = txtTelephone.Text;
                customer.EmailAddress = txtEmail.Text;
                customer.Password = txtPassword.Password;
                customer.CustomerStatus = CurrentProfile.CustomerStatus;
                customer.CustomerBirthday = DateTime.Parse(datBirthday.Text);
                if (_customerService.UpdateCustomer(customer))
                {
                    MessageBox.Show("Update successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
