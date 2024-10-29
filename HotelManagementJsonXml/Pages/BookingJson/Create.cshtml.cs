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
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.BookingJson
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
            ViewData["CustomerId"] = new SelectList(ToListModel(_customerService.GetCustomers(pathCustomer, type)), "CustomerId", "EmailAddress");
            ViewData["RoomId"] = new MultiSelectList(ToListModel(_roomInformationService.GetRoomInformations(pathRoomInformation, type)), "RoomId", "RoomNumber");
            return Page();
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;
        [BindProperty]
        public List<int> RoomInformation { get; set; } = default!;
        private string type = ".json";
        private string pathBookingReservation = "..\\Hotel_Daos\\bookingreservation";
        private string pathCustomer = "..\\Hotel_Daos\\customer";
        private string pathBookingDetail = "..\\Hotel_Daos\\bookingdetail";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("BookingReservation.Customer");
            ModelState.Remove("BookingDetail.BookingReservation");
            ModelState.Remove("BookingDetail.Room");
            if (!ModelState.IsValid || _bookingReservationService.GetBookingReservations(pathBookingReservation, type) == null || BookingReservation == null)
            {
                ViewData["CustomerId"] = new SelectList(ToListModel(_customerService.GetCustomers(pathCustomer, type)), "CustomerId", "EmailAddress");
                ViewData["RoomId"] = new MultiSelectList(ToListModel(_roomInformationService.GetRoomInformations(pathRoomInformation, type)), "RoomId", "RoomNumber");
                return Page();
            }

            decimal? totalPrice = 0;
            var days = (BookingDetail.EndDate - BookingDetail.StartDate).Days;
            int bookingReservationId = _bookingReservationService.GetBookingReservations(pathBookingReservation, type).Last().BookingReservationId + 1;
            var bookingDetails = new List<BookingDetail>();
            BookingDetail.BookingReservationId = bookingReservationId;
            var rooms = new List<RoomInformation>();
            RoomInformation.ForEach(r =>
            {
                rooms.Add(ToModel(_roomInformationService.GetRoomInformation(pathRoomInformation, type, r)));
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
            _bookingReservationService.AddBookingReservation(pathBookingReservation, type, ToDto(BookingReservation));

            bookingDetails.ForEach(b =>
            {
                BookingDetail.ActualPrice = b.ActualPrice;
                BookingDetail.RoomId = b.RoomId;
                _bookingDetailService.AddBookingDetail(pathBookingDetail, type, ToDto(BookingDetail));
            });

            return RedirectToPage("./Index");
        }
        private BookingReservationDto ToDto(BookingReservation bookingReservation)
        {
            BookingReservationDto dto = new BookingReservationDto();
            dto.BookingReservationId = bookingReservation.BookingReservationId;
            dto.CustomerId = bookingReservation.CustomerId;
            dto.BookingDate = bookingReservation.BookingDate;
            dto.TotalPrice = bookingReservation.TotalPrice;
            dto.BookingStatus = bookingReservation.BookingStatus;
            return dto;
        }
        private BookingDetailDto ToDto(BookingDetail bookingDetail)
        {
            BookingDetailDto dto = new BookingDetailDto();
            dto.BookingReservationId = bookingDetail.BookingReservationId;
            dto.RoomId = bookingDetail.RoomId;
            dto.StartDate = bookingDetail.StartDate;
            dto.EndDate = bookingDetail.EndDate;
            dto.ActualPrice = bookingDetail.ActualPrice;
            return dto;
        }
        private List<RoomInformation> ToListModel(List<RoomInformationDto> dtos)
        {
            var list = new List<RoomInformation>();
            dtos.ForEach(dto => list.Add(ToModel(dto)));
            return list;
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
        private List<Customer> ToListModel(List<CustomerDto> dtos)
        {
            var list = new List<Customer>();
            dtos.ForEach(dto => list.Add(ToModel(dto)));
            return list;
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
    }
}
