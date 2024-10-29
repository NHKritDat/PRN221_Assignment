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
    /// Interaction logic for CreatCustomerWindow.xaml
    /// </summary>
    public partial class CreatCustomerWindow : Window
    {
        private ICustomerService _customerService;
        public CreatCustomerWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer.CustomerFullName = txtFullName.Text;
                customer.Telephone = txtTelephone.Text;
                customer.EmailAddress = txtEmail.Text;
                customer.Password = txtPassword.Password;
                customer.CustomerStatus = Byte.Parse(txtStatus.Text);
                customer.CustomerBirthday = DateTime.Parse(datBirthday.Text);
                if (_customerService.AddCustomer(customer))
                {
                    MessageBox.Show("Create successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
