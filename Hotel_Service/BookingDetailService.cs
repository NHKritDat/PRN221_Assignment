using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public class BookingDetailService : IBookingDetailService
    {
        private IBookingDetailRepo bookingDetailRepo;
        public BookingDetailService()
        {
            bookingDetailRepo = new BookingDetailRepo();
        }

        public bool AddBookingDetail(BookingDetail bookingDetail) => bookingDetailRepo.AddBookingDetail(bookingDetail);

        public bool AddBookingDetail(string path, string type, BookingDetailDto bookingDetail) => bookingDetailRepo.AddBookingDetail(path, type, bookingDetail);

        public BookingDetail? GetBookingDetail(int bookingReservationId, int roomId) => bookingDetailRepo.GetBookingDetail(bookingReservationId, roomId);

        public BookingDetailDto GetBookingDetail(string path, string type, int bookingReservationId, int roomId) => bookingDetailRepo.GetBookingDetail(path, type, bookingReservationId, roomId);

        public List<BookingDetail> GetBookingDetails() => bookingDetailRepo.GetBookingDetails();

        public List<BookingDetailDto> GetBookingDetails(string path, string type) => bookingDetailRepo.GetBookingDetails(path, type);

        public bool RemoveBookingDetail(int bookingReservationId, int roomId) => bookingDetailRepo.RemoveBookingDetail(bookingReservationId, roomId);

        public bool RemoveBookingDetail(string path, string type, int bookingReservationId, int roomId) => bookingDetailRepo.RemoveBookingDetail(path, type, bookingReservationId, roomId);

        public bool UpdateBookingDetail(BookingDetail bookingDetail) => bookingDetailRepo.UpdateBookingDetail(bookingDetail);

        public bool UpdateBookingDetail(string path, string type, BookingDetail bookingDetail) => bookingDetailRepo.UpdateBookingDetail(path, type, bookingDetail);

        public void WriteFile(List<BookingDetailDto> bookingDetailDtos, string filePath, string type) => bookingDetailRepo.WriteFile(bookingDetailDtos, filePath, type);
    }
}
