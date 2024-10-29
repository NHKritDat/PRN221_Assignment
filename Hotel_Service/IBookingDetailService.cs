using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public interface IBookingDetailService
    {
        public List<BookingDetail> GetBookingDetails();
        public BookingDetail? GetBookingDetail(int bookingReservationId, int roomId);
        public bool AddBookingDetail(BookingDetail bookingDetail);
        public bool RemoveBookingDetail(int bookingReservationId, int roomId);
        public bool UpdateBookingDetail(BookingDetail bookingDetail);
        public void WriteFile(List<BookingDetailDto> bookingDetailDtos, string filePath, string type);
        public List<BookingDetailDto> GetBookingDetails(string path, string type);
        public BookingDetailDto GetBookingDetail(string path, string type, int bookingReservationId, int roomId);
        public bool AddBookingDetail(string path, string type, BookingDetailDto bookingDetail);
        public bool RemoveBookingDetail(string path, string type, int bookingReservationId, int roomId);
        public bool UpdateBookingDetail(string path, string type, BookingDetail bookingDetail);
    }
}
