using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public interface IRoomTypeService
    {
        public List<RoomType> GetRoomTypes();
        public RoomType? GetRoomType(int roomTypeId);
        public bool AddRoomType(RoomType roomType);
        public bool RemoveRoomType(int roomTypeId);
        public bool UpdateRoomType(RoomType roomType);
        public void WriteFile(List<RoomTypeDto> roomTypeDtos, string filePath, string type);
        public List<RoomTypeDto> GetRoomTypes(string path, string type);
        public RoomTypeDto GetRoomType(string path, string type, int roomTypeId);
        public bool AddRoomType(string path, string type, RoomTypeDto roomType);
        public bool RemoveRoomType(string path, string type, int roomTypeId);
        public bool UpdateRoomType(string path, string type, RoomType roomType);
    }
}
