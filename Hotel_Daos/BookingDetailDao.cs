using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using HotelUtil;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Daos
{
    public class BookingDetailDao
    {
        private static BookingDetailDao? instance;
        private FUMiniHotelManagementContext _context;
        public BookingDetailDao()
        {
            _context = new FUMiniHotelManagementContext();
        }
        public static BookingDetailDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new BookingDetailDao();
                return instance;
            }
        }

        public List<BookingDetail> GetBookingDetails() => _context.BookingDetails.ToList();
        public BookingDetail? GetBookingDetail(int bookingReservationId, int roomId) => _context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationId == bookingReservationId && bd.RoomId == roomId);
        public bool AddBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                _context.Add(bookingDetail);
                _context.SaveChanges();
                _context.Entry(bookingDetail).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveBookingDetail(int bookingReservationId, int roomId)
        {
            try
            {
                var trackedEntity = _context.ChangeTracker
                                    .Entries<BookingDetail>()
                                    .FirstOrDefault(e => e.Entity.BookingReservationId == bookingReservationId && e.Entity.RoomId == roomId);
                BookingDetail? bookingDetail;
                if (trackedEntity != null)
                    bookingDetail = trackedEntity.Entity;
                else
                    bookingDetail = GetBookingDetail(bookingReservationId, roomId);
                if (bookingDetail != null)
                {
                    _context.BookingDetails.Remove(bookingDetail);
                    _context.SaveChanges();
                    _context.Entry(bookingDetail).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                BookingDetail? old = GetBookingDetail(bookingDetail.BookingReservationId, bookingDetail.RoomId);
                if (old != null)
                {
                    _context.BookingDetails.Update(bookingDetail);
                    _context.SaveChanges();
                    _context.Entry(bookingDetail).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void WriteFile(List<BookingDetailDto> bookingdetails, string filePath, string type) => FileUtil<BookingDetailDto>.WriteFile(bookingdetails, filePath, type);
        public List<BookingDetailDto> GetBookingDetails(string path, string type) => FileUtil<BookingDetailDto>.ReadFile(path, type);
        public BookingDetailDto GetBookingDetail(string path, string type, int bookingReservationId, int roomId) => GetBookingDetails(path, type).FirstOrDefault(b => b.BookingReservationId == bookingReservationId && b.RoomId == roomId);
        public bool AddBookingDetail(string path, string type, BookingDetailDto bookingDetail)
        {
            try
            {
                var bookingdetails = GetBookingDetails(path, type);
                bookingdetails.Add(bookingDetail);
                WriteFile(bookingdetails, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateBookingDetail(string path, string type, BookingDetail bookingDetail)
        {
            try
            {
                var bookingdetails = GetBookingDetails(path, type);
                bookingdetails.ForEach(b =>
                {
                    if (b.BookingReservationId == bookingDetail.BookingReservationId && b.RoomId == bookingDetail.RoomId)
                    {
                        b.StartDate = bookingDetail.StartDate;
                        b.EndDate = bookingDetail.EndDate;
                        b.ActualPrice = bookingDetail.ActualPrice;
                    }
                });
                WriteFile(bookingdetails, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveBookingDetail(string path, string type, int bookingReservationId, int roomId)
        {
            try
            {
                var bookingdetails = GetBookingDetails(path, type);
                var bookingdetail = bookingdetails.Find(b => b.BookingReservationId == bookingReservationId && b.RoomId == roomId);
                bookingdetails.Remove(bookingdetail);
                WriteFile(bookingdetails, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
