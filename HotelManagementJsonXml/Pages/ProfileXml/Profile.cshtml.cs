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
using Hotel_BusinessObjects.Dtos;
using Hotel_Services;

namespace HotelManagementJsonXml.Pages.ProfileXml
{
    public class ProfileModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IBookingReservationService _bookingReservationService;

        public ProfileModel(ICustomerService customerService, IBookingReservationService bookingReservationService)
        {
            _bookingReservationService = bookingReservationService;
            _customerService = customerService;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        public List<BookingReservation> BookingReservation { get; set; } = default!;
        private string type = ".xml";
        private string pathCustomer = "..\\Hotel_Daos\\customer";
        private string pathReservation = "..\\Hotel_Daos\\bookingreservation";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _customerService.GetCustomers(pathCustomer, type) == null)
            {
                return NotFound();
            }

            var customer = ToModel(_customerService.GetCustomer(pathCustomer, type, id.Value));
            var bookingReservations = ToListModel(_bookingReservationService.GetBookingReservations(pathReservation, type)).Where(b => b.CustomerId == id.Value).ToList();
            bookingReservations.ForEach(b => b.Customer = customer);

            if (customer == null)
            {
                return NotFound();
            }

            Customer = customer;
            BookingReservation = bookingReservations;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var customer = ToModel(_customerService.GetCustomer(pathCustomer, type, Customer.CustomerId));
            var bookingReservations = ToListModel(_bookingReservationService.GetBookingReservations(pathReservation, type)).Where(b => b.CustomerId == Customer.CustomerId).ToList();
            bookingReservations.ForEach(b => b.Customer = customer);
            if (!ModelState.IsValid)
            {
                Customer = customer;
                BookingReservation = bookingReservations;
                return Page();
            }

            try
            {
                _customerService.UpdateCustomer(pathCustomer, type, Customer);
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
            BookingReservation = bookingReservations;
            return Page();
        }

        private bool CustomerExists(int id)
        {
            return _customerService.GetCustomer(pathCustomer, type, id) != null;
        }
        private Customer ToModel(CustomerDto customerDto)
        {
            Customer customer = new Customer();
            customer.CustomerId = customerDto.CustomerId;
            customer.CustomerFullName = customerDto.CustomerFullName;
            customer.Telephone = customerDto.Telephone;
            customer.EmailAddress = customerDto.EmailAddress;
            customer.CustomerBirthday = customerDto.CustomerBirthday;
            customer.CustomerStatus = customerDto.CustomerStatus;
            customer.Password = customerDto.Password;
            return customer;
        }
        private BookingReservation ToModel(BookingReservationDto dto)
        {
            BookingReservation bookingReservation = new BookingReservation();
            bookingReservation.BookingReservationId = dto.BookingReservationId;
            bookingReservation.BookingDate = dto.BookingDate;
            bookingReservation.TotalPrice = dto.TotalPrice;
            bookingReservation.CustomerId = dto.CustomerId;
            bookingReservation.BookingStatus = dto.BookingStatus;
            return bookingReservation;
        }
        private List<BookingReservation> ToListModel(List<BookingReservationDto> dtos)
        {
            List<BookingReservation> bookingReservations = new List<BookingReservation>();
            dtos.ForEach(dto => bookingReservations.Add(ToModel(dto)));
            return bookingReservations;
        }
    }
}
