using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public interface IBookingReservationService
    {
        public List<BookingReservation> GetBookingReservations();
        public BookingReservation? GetBookingReservation(int id);
        public bool AddBookingReservation(BookingReservation bookingReservation);
        public bool RemoveBookingReservation(int id);
        public bool UpdateBookingReservation(BookingReservation bookingReservation);
        public void WriteFile(List<BookingReservationDto> bookingReservationDtos, string filePath, string type);
        public List<BookingReservationDto> GetBookingReservations(string path, string type);
        public BookingReservationDto GetBookingReservation(string path, string type, int id);
        public bool AddBookingReservation(string path, string type, BookingReservationDto bookingReservation);
        public bool RemoveBookingReservation(string path, string type, int id);
        public bool UpdateBookingReservation(string path, string type, BookingReservation bookingReservation);
    }
}
