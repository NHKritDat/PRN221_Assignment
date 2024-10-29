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

namespace HotelManagementRazorPage.Pages.CustomerPage
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _customerService.GetCustomers() == null)
            {
                return NotFound();
            }

            var customer = _customerService.GetCustomer(id.Value);

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
            if (id == null || _customerService.GetCustomers() == null)
            {
                return NotFound();
            }
            var customer = _customerService.GetCustomer(id.Value);

            if (customer != null)
            {
                Customer = customer;
                _customerService.RemoveCustomer(Customer.CustomerId);
            }

            return RedirectToPage("./Index");
        }
    }
}
