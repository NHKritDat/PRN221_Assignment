using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;

namespace HotelManagementRazorPage.Pages.ProfilePage
{
    public class ProfileModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookDetailService;

        public ProfileModel(ICustomerService customerService, IBookingReservationService bookingReservationService, IBookingDetailService bookDetailService)
        {
            _customerService = customerService;
            _bookingReservationService = bookingReservationService;
            _bookDetailService = bookDetailService;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        public List<BookingReservation> BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _customerService.GetCustomers() == null)
            {
                return NotFound();
            }

            var customer = _customerService.GetCustomer(id.Value);
            var bookingreservation = _bookingReservationService.GetBookingReservations().Where(b => b.CustomerId == id.Value).ToList();
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            BookingReservation = bookingreservation;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var customer = _customerService.GetCustomer(Customer.CustomerId);
            var bookingreservation = _bookingReservationService.GetBookingReservations().Where(b => b.CustomerId == Customer.CustomerId).ToList();
            if (!ModelState.IsValid)
            {
                Customer = customer;
                BookingReservation = bookingreservation;
                return Page();
            }

            try
            {
                _customerService.UpdateCustomer(Customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(Customer.CustomerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Customer = customer;
            BookingReservation = bookingreservation;
            return Page();
        }

        private bool CustomerExists(int id)
        {
            return _customerService.GetCustomer(id) != null;
        }
    }
}
