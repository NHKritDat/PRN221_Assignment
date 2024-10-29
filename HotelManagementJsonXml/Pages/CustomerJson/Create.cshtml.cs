using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.CustomerJson
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CreateModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        private string pathCustomer = "..\\Hotel_Daos\\customer";
        private string type = ".json";


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _customerService.GetCustomers(pathCustomer, type) == null || Customer == null)
            {
                return Page();
            }

            _customerService.AddCustomer(pathCustomer, type, ToDto(Customer));

            return RedirectToPage("./Index");
        }
        private CustomerDto ToDto(Customer customer)
        {
            CustomerDto dto = new CustomerDto();
            dto.CustomerFullName = customer.CustomerFullName;
            dto.Telephone = customer.Telephone;
            dto.EmailAddress = customer.EmailAddress;
            dto.CustomerBirthday = customer.CustomerBirthday;
            dto.CustomerStatus = customer.CustomerStatus;
            dto.Password = customer.Password;
            return dto;
        }
    }
}
