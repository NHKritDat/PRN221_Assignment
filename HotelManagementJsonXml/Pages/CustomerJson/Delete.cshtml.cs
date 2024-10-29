using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.CustomerJson
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public DeleteModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        private string pathCustomer = "..\\Hotel_Daos\\customer";
        private string type = ".json";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _customerService.GetCustomers(pathCustomer, type) == null)
            {
                return NotFound();
            }

            var customer = ToModel(_customerService.GetCustomer(pathCustomer, type, id.Value));

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _customerService.GetCustomers(pathCustomer, type) == null)
            {
                return NotFound();
            }
            var customer = ToModel(_customerService.GetCustomer(pathCustomer, type, id.Value));

            if (customer != null)
            {
                Customer = customer;
                _customerService.RemoveCustomer(pathCustomer, type, Customer.CustomerId);
            }

            return RedirectToPage("./Index");
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
    }
}
