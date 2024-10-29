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
    public class BookingReservationService : IBookingReservationService
    {
        private IBookingReservationRepo bookingReservationRepo;
        public BookingReservationService()
        {
            bookingReservationRepo = new BookingReservationRepo();
        }

        public bool AddBookingReservation(BookingReservation bookingReservation) => bookingReservationRepo.AddBookingReservation(bookingReservation);

        public bool AddBookingReservation(string path, string type, BookingReservationDto bookingReservation) => bookingReservationRepo.AddBookingReservation(path, type, bookingReservation);

        public BookingReservation? GetBookingReservation(int id) => bookingReservationRepo.GetBookingReservation(id);

        public BookingReservationDto GetBookingReservation(string path, string type, int id) => bookingReservationRepo.GetBookingReservation(path, type, id);

        public List<BookingReservation> GetBookingReservations() => bookingReservationRepo.GetBookingReservations();

        public List<BookingReservationDto> GetBookingReservations(string path, string type) => bookingReservationRepo.GetBookingReservations(path, type);

        public bool RemoveBookingReservation(int id) => bookingReservationRepo.RemoveBookingReservation(id);

        public bool RemoveBookingReservation(string path, string type, int id) => bookingReservationRepo.RemoveBookingReservation(path, type, id);

        public bool UpdateBookingReservation(BookingReservation bookingReservation) => bookingReservationRepo.UpdateBookingReservation(bookingReservation);

        public bool UpdateBookingReservation(string path, string type, BookingReservation bookingReservation) => bookingReservationRepo.UpdateBookingReservation(path, type, bookingReservation);

        public void WriteFile(List<BookingReservationDto> bookingReservationDtos, string filePath, string type) => bookingReservationRepo.WriteFile(bookingReservationDtos, filePath, type);
    }
}
