using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using HotelManagementWPF.BookingReservationWindows;
using HotelManagementWPF.RoomInformationWindows;
using HotelUtil;
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

namespace HotelManagementWPF
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private ICustomerService _customerService;
        private Customer? selected;
        public CustomerWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        private void LoadData()
        {
            dtgCustomer.ItemsSource = _customerService.GetCustomers();
        }

        private void Loaded_Customer(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dtgCustomer.ItemsSource = _customerService.GetCustomers()
                .Where(c =>
                c.CustomerFullName.ToLower().Contains(txtSearch.Text.ToLower()));
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreatCustomerWindow creatCustomerWindow = new CreatCustomerWindow();
            creatCustomerWindow.ShowDialog();
            LoadData();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to exit app?", "Exit App!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateCustomerWindow updateCustomerWindow = new UpdateCustomerWindow();
            updateCustomerWindow.SelectedCustomer = selected;
            updateCustomerWindow.ShowDialog();
            LoadData();
        }

        private void dtgCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgCustomer.SelectedItems.Count > 0)
            {
                selected = dtgCustomer.SelectedItems[0] as Customer;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to delete this customer?", "Delete Customer!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                if (_customerService.RemoveCustomer(selected.CustomerId))
                {
                    MessageBox.Show("Remove successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    return;
                }
                MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            BookingReservationWindow bookingReservationWindow = new BookingReservationWindow();
            bookingReservationWindow.ShowDialog();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RoomWindow roomWindow = new RoomWindow();
            roomWindow.ShowDialog();
        }

        private void btnXml_Click(object sender, RoutedEventArgs e)
        {
            string path = "..\\Hotel_Daos\\customer";
            var customers = ToListDto(_customerService.GetCustomers());
            _customerService.WriteFile(customers, path, ".xml");
            MessageBox.Show("Export successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnJson_Click(object sender, RoutedEventArgs e)
        {
            string path = "..\\Hotel_Daos\\customer";
            var customers = ToListDto(_customerService.GetCustomers());
            _customerService.WriteFile(customers, path, ".json");
            MessageBox.Show("Export successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private CustomerDto ToDto(Customer customer)
        {
            CustomerDto dto = new CustomerDto();
            dto.CustomerId = customer.CustomerId;
            dto.CustomerFullName = customer.CustomerFullName;
            dto.Telephone = customer.Telephone;
            dto.EmailAddress = customer.EmailAddress;
            dto.CustomerBirthday = customer.CustomerBirthday;
            dto.CustomerStatus = customer.CustomerStatus;
            dto.Password = customer.Password;
            return dto;
        }
        private List<CustomerDto> ToListDto(List<Customer> customers)
        {
            var list = new List<CustomerDto>();
            customers.ForEach(c => list.Add(ToDto(c)));
            return list;
        }
    }
}
