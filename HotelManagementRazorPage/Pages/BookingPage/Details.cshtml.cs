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
    public class DetailsModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly ICustomerService _customerService;
        private readonly IBookingDetailService _detailService;
        private readonly IRoomInformationService _roomInformationService;

        public DetailsModel(IBookingReservationService bookingReservationService, ICustomerService customerService, IBookingDetailService detailService, IRoomInformationService roomInformationService)
        {
            _bookingReservationService = bookingReservationService;
            _customerService = customerService;
            _detailService = detailService;
            _roomInformationService = roomInformationService;
        }

        public BookingReservation BookingReservation { get; set; } = default!;
        public List<BookingDetail> BookingDetail { get; set; } = default!;

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
                bookingreservation.Customer = _customerService.GetCustomer(bookingreservation.CustomerId);
                var bookingdetail = _detailService.GetBookingDetails().Where(b => b.BookingReservationId == bookingreservation.BookingReservationId).ToList();
                bookingdetail.ForEach(b => b.Room = _roomInformationService.GetRoomInformation(b.RoomId));
                BookingReservation = bookingreservation;
                BookingDetail = bookingdetail;
            }
            return Page();
        }
    }
}
