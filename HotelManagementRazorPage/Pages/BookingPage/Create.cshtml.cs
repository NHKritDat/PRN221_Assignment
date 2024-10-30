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

namespace HotelManagementRazorPage.Pages.BookingPage
{
    public class CreateModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly ICustomerService _customerService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IRoomInformationService _roomInformationService;

        public CreateModel(IBookingReservationService bookingReservationService, ICustomerService customerService, IBookingDetailService bookingDetailService, IRoomInformationService roomInformationService)
        {
            _bookingReservationService = bookingReservationService;
            _customerService = customerService;
            _bookingDetailService = bookingDetailService;
            _roomInformationService = roomInformationService;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerId"] = new SelectList(_customerService.GetCustomers(), "CustomerId", "EmailAddress");
            ViewData["RoomId"] = new MultiSelectList(_roomInformationService.GetRoomInformations(), "RoomId", "RoomNumber");
            return Page();
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;
        [BindProperty]
        public List<int> RoomInformation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("BookingReservation.Customer");
            ModelState.Remove("BookingDetail.BookingReservation");
            ModelState.Remove("BookingDetail.Room");
            if (!ModelState.IsValid || _bookingReservationService.GetBookingReservations() == null || BookingReservation == null)
            {
                ViewData["CustomerId"] = new SelectList(_customerService.GetCustomers(), "CustomerId", "EmailAddress");
                ViewData["RoomId"] = new SelectList(_roomInformationService.GetRoomInformations(), "RoomId", "RoomNumber");
                return Page();
            }

            decimal? totalPrice = 0;
            var days = (BookingDetail.EndDate - BookingDetail.StartDate).Days;
            int bookingReservationId = _bookingReservationService.GetBookingReservations().Last().BookingReservationId + 1;
            var bookingDetails = new List<BookingDetail>();
            BookingDetail.BookingReservationId = bookingReservationId;
            var rooms = new List<RoomInformation>();
            RoomInformation.ForEach(r =>
            {
                rooms.Add(_roomInformationService.GetRoomInformation(r));
            });

            rooms.ForEach(r =>
            {
                BookingDetail bookingDetail = new BookingDetail();
                bookingDetail.RoomId = r.RoomId;
                totalPrice += bookingDetail.ActualPrice = (r.RoomPricePerDay * days);
                bookingDetails.Add(bookingDetail);
            });

            BookingReservation.BookingReservationId = bookingReservationId;
            BookingReservation.TotalPrice = totalPrice;
            BookingReservation.BookingDate = DateTime.Now;
            BookingReservation.BookingStatus = 1;
            _bookingReservationService.AddBookingReservation(BookingReservation);

            bookingDetails.ForEach(b =>
            {
                BookingDetail.ActualPrice = b.ActualPrice;
                BookingDetail.RoomId = b.RoomId;
                _bookingDetailService.AddBookingDetail(BookingDetail);
            });

            return RedirectToPage("./Index");
        }
    }
}
