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

namespace HotelManagementJsonXml.Pages.BookingXml
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
        private string type = ".xml";
        private string pathBookingReservation = "..\\Hotel_Daos\\bookingreservation";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _bookingReservationService.GetBookingReservations(pathBookingReservation, type) == null)
            {
                return NotFound();
            }

            var bookingreservation = ToModel(_bookingReservationService.GetBookingReservation(pathBookingReservation, type, id.Value));

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
            if (id == null || _bookingReservationService.GetBookingReservations(pathBookingReservation, type) == null)
            {
                return NotFound();
            }
            var bookingreservation = ToModel(_bookingReservationService.GetBookingReservation(pathBookingReservation, type, id.Value));

            if (bookingreservation != null)
            {
                BookingReservation = bookingreservation;
                _bookingReservationService.RemoveBookingReservation(pathBookingReservation, type, BookingReservation.BookingReservationId);
            }

            return RedirectToPage("./Index");
        }
        private BookingReservation ToModel(BookingReservationDto dto)
        {
            BookingReservation model = new BookingReservation();
            model.BookingReservationId = dto.BookingReservationId;
            model.BookingDate = dto.BookingDate;
            model.TotalPrice = dto.TotalPrice;
            model.CustomerId = dto.CustomerId;
            model.BookingStatus = dto.BookingStatus;
            return model;
        }
    }
}
