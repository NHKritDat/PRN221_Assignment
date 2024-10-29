using Hotel_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelManagementJsonXml.Pages
{
    public class LoginAdminModel : PageModel
    {
        private IAccountService _accountService;
        public LoginAdminModel(IAccountService accountService)
        {
            _accountService = accountService;
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
                Response.Redirect("/CustomerXml");
            }
        }
    }
}
