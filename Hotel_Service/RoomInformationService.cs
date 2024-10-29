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
    public class RoomInformationService : IRoomInformationService
    {
        private IRoomInformationRepo roomInformationRepo;
        public RoomInformationService()
        {
            roomInformationRepo = new RoomInformationRepo();
        }

        public bool AddRoomInformation(RoomInformation roomInformation) => roomInformationRepo.AddRoomInformation(roomInformation);

        public bool AddRoomInformation(string path, string type, RoomInformationDto roomInformation) => roomInformationRepo.AddRoomInformation(path, type, roomInformation);

        public RoomInformation? GetRoomInformation(int roomId) => roomInformationRepo.GetRoomInformation(roomId);

        public RoomInformationDto GetRoomInformation(string path, string type, int roomId) => roomInformationRepo.GetRoomInformation(path, type, roomId);

        public List<RoomInformation> GetRoomInformations() => roomInformationRepo.GetRoomInformations();

        public List<RoomInformationDto> GetRoomInformations(string path, string type) => roomInformationRepo.GetRoomInformations(path, type);

        public bool RemoveRoomInformation(int roomId) => roomInformationRepo.RemoveRoomInformation(roomId);

        public bool RemoveRoomInformation(string path, string type, int roomId) => roomInformationRepo.RemoveRoomInformation(path, type, roomId);

        public bool UpdateRoomInformation(RoomInformation roomInformation) => roomInformationRepo.UpdateRoomInformation(roomInformation);

        public bool UpdateRoomInformation(string path, string type, RoomInformation roomInformation) => roomInformationRepo.UpdateRoomInformation(path, type, roomInformation);

        public void WriteFile(List<RoomInformationDto> roomInformationDtos, string filePath, string type) => roomInformationRepo.WriteFile(roomInformationDtos, filePath, type);
    }
}
