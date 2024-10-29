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
    public class RoomTypeDao
    {
        private static RoomTypeDao? instance;
        private FUMiniHotelManagementContext _context;
        public RoomTypeDao()
        {
            _context = new FUMiniHotelManagementContext();
        }
        public static RoomTypeDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoomTypeDao();
                return instance;
            }
        }
        public List<RoomType> GetRoomTypes() => _context.RoomTypes.ToList();
        public RoomType? GetRoomType(int id) => _context.RoomTypes.SingleOrDefault(rt => rt.RoomTypeId == id);
        public bool AddRoomType(RoomType roomType)
        {
            try
            {
                _context.RoomTypes.Add(roomType);
                _context.SaveChanges();
                _context.Entry(roomType).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveRoomType(int id)
        {
            try
            {
                RoomType? roomType = GetRoomType(id);
                if (roomType != null)
                {
                    _context.RoomTypes.Remove(roomType);
                    _context.SaveChanges();
                    _context.Entry(roomType).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateRoomType(RoomType roomType)
        {
            try
            {
                RoomType? old = GetRoomType(roomType.RoomTypeId);
                if (old != null)
                {
                    _context.RoomTypes.Update(roomType);
                    _context.SaveChanges();
                    _context.Entry(roomType).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void WriteFile(List<RoomTypeDto> roomtypes, string filePath, string type) => FileUtil<RoomTypeDto>.WriteFile(roomtypes, filePath, type);
        public List<RoomTypeDto> GetRoomTypes(string path, string type) => FileUtil<RoomTypeDto>.ReadFile(path, type);
        public RoomTypeDto GetRoomType(string path, string type, int id) => GetRoomTypes(path, type).FirstOrDefault(r => r.RoomTypeId == id);
        public bool AddRoomType(string path, string type, RoomTypeDto roomType)
        {
            try
            {
                var roomtypes = GetRoomTypes(path, type);
                roomType.RoomTypeId = roomtypes.Last().RoomTypeId + 1;
                roomtypes.Add(roomType);
                WriteFile(roomtypes, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveRoomType(string path, string type, int id)
        {
            try
            {
                var roomtypes = GetRoomTypes(path, type);
                var roomtype = roomtypes.First(r => r.RoomTypeId == id);
                roomtypes.Remove(roomtype);
                WriteFile(roomtypes, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateRoomType(string path, string type, RoomType roomType)
        {
            try
            {
                var roomtypes = GetRoomTypes(path, type);
                roomtypes.ForEach(r =>
                {
                    if (r.RoomTypeId == roomType.RoomTypeId)
                    {
                        r.RoomTypeName = roomType.RoomTypeName;
                        r.TypeDescription = roomType.TypeDescription;
                        r.TypeNote = roomType.TypeNote;
                    }
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
