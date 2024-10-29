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

namespace HotelManagementJsonXml.Pages.BookingJson
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
        private string type = ".json";
        private string pathBookingReservation = "..\\Hotel_Daos\\bookingreservation";
        private string pathCustomer = "..\\Hotel_Daos\\customer";

        public async Task OnGetAsync()
        {
            if (_bookingReservationService.GetBookingReservations(pathBookingReservation, type) != null)
            {
                var bookingreservations = ToListModel(_bookingReservationService.GetBookingReservations(pathBookingReservation, type));
                bookingreservations.ForEach(b => b.Customer = ToModel(_customerService.GetCustomer(pathCustomer, type, b.CustomerId)));
                BookingReservation = bookingreservations;
            }
        }
        private Customer ToModel(CustomerDto dto)
        {
            Customer customer = new Customer();
            customer.CustomerId = dto.CustomerId;
            customer.CustomerFullName = dto.CustomerFullName;
            customer.Telephone = dto.Telephone;
            customer.EmailAddress = dto.EmailAddress;
            customer.CustomerBirthday = dto.CustomerBirthday;
            customer.CustomerStatus = dto.CustomerStatus;
            customer.Password = dto.Password;
            return customer;
        }
        private List<BookingReservation> ToListModel(List<BookingReservationDto> dtos)
        {
            List<BookingReservation> list = new List<BookingReservation>();
            dtos.ForEach(d => list.Add(ToModel(d)));
            return list;
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
