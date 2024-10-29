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

namespace HotelManagementRazorPage.Pages.BookingPage
{
    public class DeleteModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;

        public DeleteModel(IBookingReservationService bookingReservationService)
        {
            _bookingReservationService = bookingReservationService;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _bookingReservationService.GetBookingReservations() == null)
            {
                return NotFound();
            }

            var bookingreservation = _bookingReservationService.GetBookingReservation(id.Value);

            if (bookingreservation == null)
            {
                return NotFound();
            }
            else
            {
                BookingReservation = bookingreservation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _bookingReservationService.GetBookingReservations() == null)
            {
                return NotFound();
            }
            var bookingreservation = _bookingReservationService.GetBookingReservation(id.Value);

            if (bookingreservation != null)
            {
                BookingReservation = bookingreservation;
                _bookingReservationService.RemoveBookingReservation(BookingReservation.BookingReservationId);
            }

            return RedirectToPage("./Index");
        }
    }
}
