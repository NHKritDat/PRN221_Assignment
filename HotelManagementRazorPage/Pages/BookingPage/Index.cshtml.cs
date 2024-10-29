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
    public class IndexModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly ICustomerService _customerService;

        public IndexModel(IBookingReservationService bookingReservationService, ICustomerService customerService)
        {
            _bookingReservationService = bookingReservationService;
            _customerService = customerService;
        }

        public IList<BookingReservation> BookingReservation { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_bookingReservationService.GetBookingReservations() != null)
            {
                var bookingReservations = _bookingReservationService.GetBookingReservations();
                bookingReservations.ForEach(reservation => reservation.Customer = _customerService.GetCustomer(reservation.CustomerId));
                BookingReservation = bookingReservations;
            }
        }
    }
}
