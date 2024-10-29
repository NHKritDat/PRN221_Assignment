using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelManagementRazorPage.Pages
{
    public class LoginModel : PageModel
    {
        private IAccountService _accountService;
        private ICustomerService _customerService;
        public LoginModel(IAccountService accountService, ICustomerService customerService)
        {
            _accountService = accountService;
            _customerService = customerService;
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            string email = Request.Form["txtEmail"];
            string password = Request.Form["txtPassword"];

            List<string> account = _accountService.GetAccount();

            if (account[0].Equals(email) && account[1].Equals(password))
            {
                int roleID = 1;
                HttpContext.Session.SetString("RoleID", roleID.ToString());
                Response.Redirect("/CustomerPage");
            }
            else
            {
                List<Customer> customers = _customerService.GetCustomers();
                customers.ForEach(c =>
                {
                    if (c.EmailAddress.Equals(email) && c.Password.Equals(password))
                    {
                        int roleID = 2;
                        HttpContext.Session.SetString("RoleID", roleID.ToString());
                        HttpContext.Session.SetString("ProfileID", c.CustomerId.ToString());
                        Response.Redirect("/ProfilePage/Profile?id="+c.CustomerId);
                    }
                });
            }
        }
    }
}
