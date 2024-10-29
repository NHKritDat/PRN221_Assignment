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

namespace HotelManagementWPF
{
    /// <summary>
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        private ICustomerService _customerService;
        public Customer SelectedCustomer { get; set; } = null;
        public UpdateCustomerWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer.CustomerId = SelectedCustomer.CustomerId;
                customer.CustomerFullName = txtFullName.Text;
                customer.Telephone = txtTelephone.Text;
                customer.EmailAddress = txtEmail.Text;
                customer.Password = txtPassword.Password;
                customer.CustomerStatus = Byte.Parse(txtStatus.Text);
                customer.CustomerBirthday = DateTime.Parse(datBirthday.Text);
                if (_customerService.UpdateCustomer(customer))
                {
                    MessageBox.Show("Update successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                    MessageBox.Show("Something wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Loaded_Customer(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFullName.Text = SelectedCustomer.CustomerFullName;
                txtEmail.Text = SelectedCustomer.EmailAddress;
                txtPassword.Password = SelectedCustomer.Password;
                txtTelephone.Text = SelectedCustomer.Telephone;
                txtStatus.Text = SelectedCustomer.CustomerStatus.ToString();
                datBirthday.Text = SelectedCustomer.CustomerBirthday.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Please choose Item to update!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
