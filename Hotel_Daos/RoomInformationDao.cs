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
    public class RoomInformationDao
    {
        private static RoomInformationDao? instance;
        private FUMiniHotelManagementContext _context;
        public RoomInformationDao()
        {
            _context = new FUMiniHotelManagementContext();
        }
        public static RoomInformationDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoomInformationDao();
                return instance;
            }
        }
        public List<RoomInformation> GetRoomInformations() => _context.RoomInformations.ToList();
        public RoomInformation? GetRoomInformation(int id) => _context.RoomInformations.SingleOrDefault(ri => ri.RoomId == id);
        public bool AddRoomInformation(RoomInformation roomInformation)
        {
            try
            {
                _context.RoomInformations.Add(roomInformation);
                _context.SaveChanges();
                _context.Entry(roomInformation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveRoomInformation(int id)
        {
            try
            {
                RoomInformation? roomInformation = GetRoomInformation(id);
                if (roomInformation != null)
                {
                    _context.RoomInformations.Remove(roomInformation);
                    _context.SaveChanges();
                    _context.Entry(roomInformation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateRoomInformation(RoomInformation roomInformation)
        {
            try
            {
                RoomInformation? old = GetRoomInformation(roomInformation.RoomId);
                if (old != null)
                {
                    _context.RoomInformations.Update(roomInformation);
                    _context.SaveChanges();
                    _context.Entry(roomInformation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void WriteFile(List<RoomInformationDto> roominformations, string filePath, string type) => FileUtil<RoomInformationDto>.WriteFile(roominformations, filePath, type);
        public List<RoomInformationDto> GetRoomInformations(string filePath, string type) => FileUtil<RoomInformationDto>.ReadFile(filePath, type);
        public RoomInformationDto GetRoomInformation(string path, string type, int id) => GetRoomInformations(path, type).FirstOrDefault(r => r.RoomId == id);
        public bool AddRoomInformation(string path, string type, RoomInformationDto roomInformation)
        {
            try
            {
                var roomInformations = GetRoomInformations(path, type);
                roomInformation.RoomId = roomInformations.Last().RoomId + 1;
                roomInformations.Add(roomInformation);
                WriteFile(roomInformations, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateRoomInformation(string path, string type, RoomInformation roomInformation)
        {
            try
            {
                var roomInformations = GetRoomInformations(path, type);
                roomInformations.ForEach(r =>
                {
                    if (r.RoomId == roomInformation.RoomId)
                    {
                        r.RoomNumber = roomInformation.RoomNumber;
                        r.RoomDetailDescription = roomInformation.RoomDetailDescription;
                        r.RoomMaxCapacity = roomInformation.RoomMaxCapacity;
                        r.RoomTypeId = roomInformation.RoomTypeId;
                        r.RoomStatus = roomInformation.RoomStatus;
                        r.RoomPricePerDay = roomInformation.RoomPricePerDay;
                    }
                });
                WriteFile(roomInformations, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveRoomInformation(string path, string type, int id)
        {
            try
            {
                var roomInformations = GetRoomInformations(path, type);
                var roomInformation = roomInformations.FirstOrDefault(r => r.RoomId == id);
                roomInformations.Remove(roomInformation);
                WriteFile(roomInformations, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
