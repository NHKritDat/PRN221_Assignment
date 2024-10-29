using Hotel_BusinessObjects.Models;
using Hotel_Services;
using HotelManagementWPF.ProfileWindow;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagementWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAccountService _accountService;
        private ICustomerService _customerService;
        public MainWindow()
        {
            InitializeComponent();
            _accountService = new AccountService();
            _customerService = new CustomerService();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to exit app?", "Exit App!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter email and password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<string> account = _accountService.GetAccount();

            if (account[0].Equals(txtEmail.Text))
            {
                if (!account[1].Equals(txtPassword.Password))
                {
                    MessageBox.Show("Wrong Email or Password!", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                this.Hide();
                CustomerWindow customerWindow = new CustomerWindow();
                customerWindow.ShowDialog();
                this.Close();
            }
            else
            {
                bool valid = false;
                List<Customer> customers = _customerService.GetCustomers();
                customers.ForEach(c =>
                {
                    if (c.EmailAddress.Equals(txtEmail.Text) && c.Password.Equals(txtPassword.Password))
                    {
                        valid = true;
                        this.Hide();
                        CustomerProfile profile = new CustomerProfile();
                        profile.CurrentProfile = c;
                        profile.ShowDialog();
                    }
                });
                if (!valid)
                {
                    MessageBox.Show("Wrong Email or Password!", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

            }
        }
    }
}