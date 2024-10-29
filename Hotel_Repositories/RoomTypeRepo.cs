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
    public class RoomTypeRepo : IRoomTypeRepo
    {
        public bool AddRoomType(RoomType roomType) => RoomTypeDao.Instance.AddRoomType(roomType);

        public bool AddRoomType(string path, string type, RoomTypeDto roomType) => RoomTypeDao.Instance.AddRoomType(path, type, roomType);

        public RoomType? GetRoomType(int roomTypeId) => RoomTypeDao.Instance.GetRoomType(roomTypeId);

        public RoomTypeDto GetRoomType(string path, string type, int roomTypeId) => RoomTypeDao.Instance.GetRoomType(path, type, roomTypeId);

        public List<RoomType> GetRoomTypes() => RoomTypeDao.Instance.GetRoomTypes();

        public List<RoomTypeDto> GetRoomTypes(string path, string type) => RoomTypeDao.Instance.GetRoomTypes(path, type);

        public bool RemoveRoomType(int roomTypeId) => RoomTypeDao.Instance.RemoveRoomType(roomTypeId);

        public bool RemoveRoomType(string path, string type, int roomTypeId) => RoomTypeDao.Instance.RemoveRoomType(path, type, roomTypeId);

        public bool UpdateRoomType(RoomType roomType) => RoomTypeDao.Instance.UpdateRoomType(roomType);

        public bool UpdateRoomType(string path, string type, RoomType roomType) => RoomTypeDao.Instance.UpdateRoomType(path, type, roomType);

        public void WriteFile(List<RoomTypeDto> roomtypes, string filePath, string type) => RoomTypeDao.Instance.WriteFile(roomtypes, filePath, type);
    }
}
