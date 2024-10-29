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
    public class BookingReservationRepo : IBookingReservationRepo
    {
        public bool AddBookingReservation(BookingReservation bookingReservation) => BookingReservationDao.Instance.AddBookingReservation(bookingReservation);

        public bool AddBookingReservation(string path, string type, BookingReservationDto bookingReservation) => BookingReservationDao.Instance.AddBookingReservation(path, type, bookingReservation);

        public BookingReservation? GetBookingReservation(int id) => BookingReservationDao.Instance.GetBookingReservation(id);

        public BookingReservationDto GetBookingReservation(string path, string type, int id) => BookingReservationDao.Instance.GetBookingReservation(path, type, id);

        public List<BookingReservation> GetBookingReservations() => BookingReservationDao.Instance.GetBookingReservations();

        public List<BookingReservationDto> GetBookingReservations(string path, string type) => BookingReservationDao.Instance.GetBookingReservations(path, type);

        public bool RemoveBookingReservation(int id) => BookingReservationDao.Instance.RemoveBookingReservation(id);

        public bool RemoveBookingReservation(string path, string type, int id) => BookingReservationDao.Instance.RemoveBookingReservation(path, type, id);

        public bool UpdateBookingReservation(BookingReservation bookingReservation) => BookingReservationDao.Instance.UpdateBookingReservation(bookingReservation);

        public bool UpdateBookingReservation(string path, string type, BookingReservation bookingReservation) => BookingReservationDao.Instance.UpdateBookingReservation(path, type, bookingReservation);

        public void WriteFile(List<BookingReservationDto> bookingreservations, string filePath, string type) => BookingReservationDao.Instance.WriteFile(bookingreservations, filePath, type);
    }
}
