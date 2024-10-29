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
    public class RoomTypeService : IRoomTypeService
    {
        private IRoomTypeRepo roomTypeRepo;
        public RoomTypeService()
        {
            roomTypeRepo = new RoomTypeRepo();
        }

        public bool AddRoomType(RoomType roomType) => roomTypeRepo.AddRoomType(roomType);

        public bool AddRoomType(string path, string type, RoomTypeDto roomType)=>roomTypeRepo.AddRoomType(path, type, roomType);

        public RoomType? GetRoomType(int roomTypeId) => roomTypeRepo.GetRoomType(roomTypeId);

        public RoomTypeDto GetRoomType(string path, string type, int roomTypeId)=>roomTypeRepo.GetRoomType(path,type,roomTypeId);

        public List<RoomType> GetRoomTypes() => roomTypeRepo.GetRoomTypes();

        public List<RoomTypeDto> GetRoomTypes(string path, string type)=>roomTypeRepo.GetRoomTypes(path,type);

        public bool RemoveRoomType(int roomTypeId) => roomTypeRepo.RemoveRoomType(roomTypeId);

        public bool RemoveRoomType(string path, string type, int roomTypeId)=>roomTypeRepo.RemoveRoomType(path,type,roomTypeId);

        public bool UpdateRoomType(RoomType roomType) => roomTypeRepo.UpdateRoomType(roomType);

        public bool UpdateRoomType(string path, string type, RoomType roomType)=>roomTypeRepo.UpdateRoomType(path,type, roomType);

        public void WriteFile(List<RoomTypeDto> roomTypeDtos, string filePath, string type) => roomTypeRepo.WriteFile(roomTypeDtos, filePath, type);
    }
}
