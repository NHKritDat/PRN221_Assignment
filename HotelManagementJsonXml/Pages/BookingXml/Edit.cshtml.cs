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
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.BookingXml
{
    public class EditModel : PageModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IRoomInformationService _roomInformationService;
        private readonly ICustomerService _customerService;

        public EditModel(IBookingReservationService bookingReservationService, IBookingDetailService bookingDetailService, IRoomInformationService roomInformationService, ICustomerService customerService)
        {
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;
            _roomInformationService = roomInformationService;
            _customerService = customerService;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;
        [BindProperty]
        public List<int> RoomInformation { get; set; } = default!;
        private string type = ".xml";
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
            var bookingdetails = ToListModel(_bookingDetailService.GetBookingDetails(pathBookingDetail, type)).Where(b => b.BookingReservationId == id.Value).ToList();
            var roominfos = new List<int>();
            if (bookingreservation == null)
            {
                return NotFound();
            }
            BookingReservation = bookingreservation;
            BookingDetail = bookingdetails[0];
            bookingdetails.ForEach(b => roominfos.Add(b.RoomId));
            RoomInformation = roominfos;
            ViewData["CustomerId"] = new SelectList(ToListModel(_customerService.GetCustomers(pathCustomer, type)), "CustomerId", "EmailAddress");
            ViewData["RoomId"] = new MultiSelectList(ToListModel(_roomInformationService.GetRoomInformations(pathRoomInformation, type)), "RoomId", "RoomNumber");
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
                ViewData["CustomerId"] = new SelectList(ToListModel(_customerService.GetCustomers(pathCustomer, type)), "CustomerId", "EmailAddress");
                ViewData["RoomId"] = new MultiSelectList(ToListModel(_roomInformationService.GetRoomInformations(pathRoomInformation, type)), "RoomId", "RoomNumber");
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
                    rooms.Add(ToModel(_roomInformationService.GetRoomInformation(pathRoomInformation, type, r)));
                });
                rooms.ForEach(r =>
                {
                    BookingDetail bookingDetail = new BookingDetail();
                    bookingDetail.RoomId = r.RoomId;
                    totalPrice += bookingDetail.ActualPrice = (r.RoomPricePerDay * days);
                    bookingDetails.Add(bookingDetail);
                });
                BookingReservation.TotalPrice = totalPrice;
                _bookingReservationService.UpdateBookingReservation(pathBookingReservation, type, BookingReservation);

                var bookingdetail = ToListModel(_bookingDetailService.GetBookingDetails(pathBookingDetail, type)).Where(b => b.BookingReservationId == BookingReservation.BookingReservationId).ToList();
                bookingdetail.ForEach(b => _bookingDetailService.RemoveBookingDetail(pathBookingDetail, type, b.BookingReservationId, b.RoomId));

                bookingDetails.ForEach(b =>
                {
                    BookingDetail.ActualPrice = b.ActualPrice;
                    BookingDetail.RoomId = b.RoomId;
                    _bookingDetailService.AddBookingDetail(pathBookingDetail, type, ToDto(BookingDetail));
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
            return _bookingReservationService.GetBookingReservation(pathBookingReservation, type, id) != null;
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
