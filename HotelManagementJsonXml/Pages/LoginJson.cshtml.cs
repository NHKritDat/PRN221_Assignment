using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelManagementJsonXml.Pages
{
    public class LoginJsonModel : PageModel
    {
        private ICustomerService _customerService;
        public LoginJsonModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public void OnGet()
        {
        }
        public void OnPost()
        {
            string email = Request.Form["txtEmail"];
            string password = Request.Form["txtPassword"];

            List<Customer> customers = ToListModel(_customerService.GetCustomers("D:\\Develops\\Projects\\Personal\\PRN221_Assignment\\NguyenHoangDat_SE170330\\Hotel_Daos\\customer", ".json"));
            customers.ForEach(c =>
            {
                if (c.EmailAddress.Equals(email) && c.Password.Equals(password))
                {
                    int roleID = 2;
                    HttpContext.Session.SetString("RoleID", roleID.ToString());
                    HttpContext.Session.SetString("ProfileID", c.CustomerId.ToString());
                    Response.Redirect("/ProfileJson/Profile?id=" + c.CustomerId);
                }
            });
        }
        private Customer ToModel(CustomerDto customerDto)
        {
            Customer customer = new Customer();
            customer.CustomerId = customerDto.CustomerId;
            customer.CustomerFullName = customerDto.CustomerFullName;
            customer.Telephone = customerDto.Telephone;
            customer.EmailAddress = customerDto.EmailAddress;
            customer.CustomerBirthday = customerDto.CustomerBirthday;
            customer.CustomerStatus = customerDto.CustomerStatus;
            customer.Password = customerDto.Password;
            return customer;
        }
        private List<Customer> ToListModel(List<CustomerDto> customerList)
        {
            List<Customer> result = new List<Customer>();
            foreach (var customer in customerList)
                result.Add(ToModel(customer));
            return result;
        }
    }
}
