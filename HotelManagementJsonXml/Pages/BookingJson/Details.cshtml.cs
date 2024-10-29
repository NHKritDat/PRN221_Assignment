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
    public class DetailsModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _detailService;
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomInformationService;

        public DetailsModel(IBookingReservationService bookingReservationService, IBookingDetailService detailService, ICustomerService customerService, IRoomInformationService roomInformationService)
        {
            _bookingReservationService = bookingReservationService;
            _detailService = detailService;
            _customerService = customerService;
            _roomInformationService = roomInformationService;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;
        [BindProperty]
        public List<BookingDetail> BookingDetail { get; set; } = default!;
        private string type = ".json";
        private string pathBookingReservation = "..\\Hotel_Daos\\bookingreservation";
        private string pathCustomer = "..\\Hotel_Daos\\customer";
        private string pathBookingDetail = "..\\Hotel_Daos\\bookingdetail";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";

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
                BookingReservation = bookingreservation;
                var bookingdetails = ToListModel(_detailService.GetBookingDetails(pathBookingDetail, type)).Where(b => b.BookingReservationId == bookingreservation.BookingReservationId).ToList();
                bookingdetails.ForEach(b => b.Room = ToModel(_roomInformationService.GetRoomInformation(pathRoomInformation, type, b.RoomId)));
                BookingDetail = bookingdetails;
            }
            return Page();
        }
        private RoomInformation ToModel(RoomInformationDto dto)
        {
            RoomInformation roominfo = new RoomInformation();
            roominfo.RoomId = dto.RoomId;
            roominfo.RoomNumber = dto.RoomNumber;
            roominfo.RoomDetailDescription = dto.RoomDetailDescription;
            roominfo.RoomMaxCapacity = dto.RoomMaxCapacity;
            roominfo.RoomTypeId = dto.RoomTypeId;
            roominfo.RoomStatus = dto.RoomStatus;
            roominfo.RoomPricePerDay = dto.RoomPricePerDay;
            return roominfo;
        }
        private List<BookingDetail> ToListModel(List<BookingDetailDto> dtos)
        {
            List<BookingDetail> list = new List<BookingDetail>();
            dtos.ForEach(dto => list.Add(ToModel(dto)));
            return list;
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
