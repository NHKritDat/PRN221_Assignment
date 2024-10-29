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

namespace HotelManagementRazorPage.Pages.BookingPage
{
    public class EditModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly ICustomerService _customerService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IRoomInformationService _roomInformationService;

        public EditModel(IBookingReservationService bookingReservationService, ICustomerService customerService, IBookingDetailService bookingDetailService, IRoomInformationService roomInformationService)
        {
            _bookingReservationService = bookingReservationService;
            _customerService = customerService;
            _roomInformationService = roomInformationService;
            _bookingDetailService = bookingDetailService;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;
        [BindProperty]
        public List<int> RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _bookingReservationService.GetBookingReservations() == null)
            {
                return NotFound();
            }

            var bookingreservation = _bookingReservationService.GetBookingReservation(id.Value);
            var bookingdetail = _bookingDetailService.GetBookingDetails().Where(b => b.BookingReservationId == id.Value).ToList();
            var roominfo = new List<int>();
            if (bookingreservation == null)
            {
                return NotFound();
            }
            BookingReservation = bookingreservation;
            BookingDetail = bookingdetail[0];
            bookingdetail.ForEach(b => roominfo.Add(b.RoomId));
            RoomInformation = roominfo;
            ViewData["CustomerId"] = new SelectList(_customerService.GetCustomers(), "CustomerId", "EmailAddress");
            ViewData["RoomId"] = new MultiSelectList(_roomInformationService.GetRoomInformations(), "RoomId", "RoomNumber");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("BookingReservation.Customer");
            ModelState.Remove("BookingDetail.BookingReservation");
            ModelState.Remove("BookingDetail.Room");
            if (!ModelState.IsValid)
            {
                ViewData["CustomerId"] = new SelectList(_customerService.GetCustomers(), "CustomerId", "EmailAddress");
                ViewData["RoomId"] = new MultiSelectList(_roomInformationService.GetRoomInformations(), "RoomId", "RoomNumber");
                return Page();
            }

            try
            {
                decimal? totalPrice = 0;
                var days = (BookingDetail.EndDate - BookingDetail.StartDate).Days;
                BookingDetail.BookingReservationId = BookingReservation.BookingReservationId;
                var bookingDetails = new List<BookingDetail>();
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
                BookingReservation.TotalPrice = totalPrice;
                _bookingReservationService.UpdateBookingReservation(BookingReservation);

                var bookingdetail = _bookingDetailService.GetBookingDetails().Where(b => b.BookingReservationId == BookingReservation.BookingReservationId).ToList();
                bookingdetail.ForEach(b => _bookingDetailService.RemoveBookingDetail(b.BookingReservationId, b.RoomId));

                bookingDetails.ForEach(b =>
                {
                    BookingDetail.ActualPrice = b.ActualPrice;
                    BookingDetail.RoomId = b.RoomId;
                    _bookingDetailService.AddBookingDetail(BookingDetail);
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingReservationExists(BookingReservation.BookingReservationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BookingReservationExists(int id)
        {
            return _bookingReservationService.GetBookingReservation(id) != null;
        }
    }
}
