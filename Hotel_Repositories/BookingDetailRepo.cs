using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Repositories
{
    public class BookingDetailRepo : IBookingDetailRepo
    {
        public bool AddBookingDetail(BookingDetail bookingDetail) => BookingDetailDao.Instance.AddBookingDetail(bookingDetail);

        public bool AddBookingDetail(string path, string type, BookingDetailDto bookingDetail) => BookingDetailDao.Instance.AddBookingDetail(path, type, bookingDetail);

        public BookingDetail? GetBookingDetail(int bookingReservationId, int roomId) => BookingDetailDao.Instance.GetBookingDetail(bookingReservationId, roomId);

        public BookingDetailDto GetBookingDetail(string path, string type, int bookingReservationId, int roomId) => BookingDetailDao.Instance.GetBookingDetail(path, type, bookingReservationId, roomId);

        public List<BookingDetail> GetBookingDetails() => BookingDetailDao.Instance.GetBookingDetails();

        public List<BookingDetailDto> GetBookingDetails(string path, string type) => BookingDetailDao.Instance.GetBookingDetails(path, type);

        public bool RemoveBookingDetail(int bookingReservationId, int roomId) => BookingDetailDao.Instance.RemoveBookingDetail(bookingReservationId, roomId);

        public bool RemoveBookingDetail(string path, string type, int bookingReservationId, int roomId) => BookingDetailDao.Instance.RemoveBookingDetail(path, type, bookingReservationId, roomId);

        public bool UpdateBookingDetail(BookingDetail bookingDetail) => BookingDetailDao.Instance.UpdateBookingDetail(bookingDetail);

        public bool UpdateBookingDetail(string path, string type, BookingDetail bookingDetail) => BookingDetailDao.Instance.UpdateBookingDetail(path, type, bookingDetail);

        public void WriteFile(List<BookingDetailDto> bookingdetails, string filePath, string type) => BookingDetailDao.Instance.WriteFile(bookingdetails, filePath, type);
    }
}
