using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using HotelUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Daos
{
    public class BookingReservationDao
    {
        private static BookingReservationDao? instance;
        private FUMiniHotelManagementContext _context;
        public BookingReservationDao()
        {
            _context = new FUMiniHotelManagementContext();
        }
        public static BookingReservationDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new BookingReservationDao();
                return instance;
            }
        }
        public List<BookingReservation> GetBookingReservations() => _context.BookingReservations.ToList();
        public BookingReservation? GetBookingReservation(int id) => _context.BookingReservations.SingleOrDefault(br => br.BookingReservationId == id);
        public bool AddBookingReservation(BookingReservation bookingReservation)
        {
            try
            {
                _context.Add(bookingReservation);
                _context.SaveChanges();
                _context.Entry(bookingReservation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveBookingReservation(int id)
        {
            try
            {
                BookingReservation? bookingReservation = GetBookingReservation(id);
                if (bookingReservation != null)
                {
                    _context.BookingReservations.Remove(bookingReservation);
                    _context.SaveChanges();
                    _context.Entry(bookingReservation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateBookingReservation(BookingReservation bookingReservation)
        {
            try
            {
                BookingReservation? old = GetBookingReservation(bookingReservation.BookingReservationId);
                if (old != null)
                {
                    _context.BookingReservations.Update(bookingReservation);
                    _context.SaveChanges();
                    _context.Entry(bookingReservation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void WriteFile(List<BookingReservationDto> bookingreservations, string filePath, string type) => FileUtil<BookingReservationDto>.WriteFile(bookingreservations, filePath, type);
        public List<BookingReservationDto> GetBookingReservations(string path, string type) => FileUtil<BookingReservationDto>.ReadFile(path, type);
        public BookingReservationDto GetBookingReservation(string path, string type, int id) => GetBookingReservations(path, type).FirstOrDefault(b => b.BookingReservationId == id);
        public bool AddBookingReservation(string path, string type, BookingReservationDto bookingReservation)
        {
            try
            {
                var bookingreservations = GetBookingReservations(path, type);
                bookingreservations.Add(bookingReservation);
                WriteFile(bookingreservations, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateBookingReservation(string path, string type, BookingReservation bookingReservation)
        {
            try
            {
                var bookingreservations = GetBookingReservations(path, type);
                bookingreservations.ForEach(b =>
                {
                    if (b.BookingReservationId == bookingReservation.BookingReservationId)
                    {
                        b.BookingDate = bookingReservation.BookingDate;
                        b.TotalPrice = bookingReservation.TotalPrice;
                        b.CustomerId = bookingReservation.CustomerId;
                        b.BookingStatus = bookingReservation.BookingStatus;
                        return;
                    }
                });
                WriteFile(bookingreservations, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveBookingReservation(string path, string type, int id)
        {
            try
            {
                var bookingreservations = GetBookingReservations(path, type);
                var bookingreservation = bookingreservations.Find(b => b.BookingReservationId == id);
                bookingreservations.Remove(bookingreservation);
                WriteFile(bookingreservations, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
