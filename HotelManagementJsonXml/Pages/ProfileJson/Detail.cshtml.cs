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

namespace HotelManagementJsonXml.Pages.ProfileJson
{
    public class DetailModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookDetailService;
        private readonly IRoomInformationService _roomInformationService;
        private readonly ICustomerService _customerService;

        public DetailModel(IBookingReservationService bookingReservationService, IBookingDetailService bookDetailService, IRoomInformationService roomInformationService, ICustomerService customerService)
        {
            _bookingReservationService = bookingReservationService;
            _bookDetailService = bookDetailService;
            _roomInformationService = roomInformationService;
            _customerService = customerService;
        }

        public BookingReservation BookingReservation { get; set; } = default!;
        public List<BookingDetail> BookingDetail { get; set; } = default!;
        private string pathBookingReservation = "..\\Hotel_Daos\\bookingreservation";
        private string pathBookingDetail = "..\\Hotel_Daos\\bookingdetail";
        private string pathCustomer = "..\\Hotel_Daos\\customer";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";
        private string type = ".json";

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
                bookingreservation.Customer = ToModel(_customerService.GetCustomer(pathCustomer, type, bookingreservation.CustomerId));

                var bookingdetails = ToListModel(_bookDetailService.GetBookingDetails(pathBookingDetail, type)).Where(b => b.BookingReservationId == bookingreservation.BookingReservationId).ToList();

                bookingdetails.ForEach(b => b.Room = ToModel(_roomInformationService.GetRoomInformation(pathRoomInformation, type, b.RoomId)));

                BookingReservation = bookingreservation;
                BookingDetail = bookingdetails;
            }
            return Page();
        }
        private RoomInformation ToModel(RoomInformationDto dto)
        {
            RoomInformation room = new RoomInformation();
            room.RoomId = dto.RoomId;
            room.RoomNumber = dto.RoomNumber;
            room.RoomDetailDescription = dto.RoomDetailDescription;
            room.RoomMaxCapacity = dto.RoomMaxCapacity;
            room.RoomTypeId = dto.RoomTypeId;
            room.RoomStatus = dto.RoomStatus;
            room.RoomPricePerDay = dto.RoomPricePerDay;
            return room;
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
        private BookingDetail ToModel(BookingDetailDto dto)
        {
            BookingDetail bookingdetail = new BookingDetail();
            bookingdetail.BookingReservationId = dto.BookingReservationId;
            bookingdetail.RoomId = dto.RoomId;
            bookingdetail.StartDate = dto.StartDate;
            bookingdetail.EndDate = dto.EndDate;
            bookingdetail.ActualPrice = dto.ActualPrice;
            return bookingdetail;
        }
        private List<BookingDetail> ToListModel(List<BookingDetailDto> dtos)
        {
            List<BookingDetail> bookingDetails = new List<BookingDetail>();
            dtos.ForEach(dto => bookingDetails.Add(ToModel(dto)));
            return bookingDetails;
        }
    }
}
