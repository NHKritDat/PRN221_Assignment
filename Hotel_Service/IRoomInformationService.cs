using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public interface IRoomInformationService
    {
        public List<RoomInformation> GetRoomInformations();
        public RoomInformation? GetRoomInformation(int roomId);
        public bool AddRoomInformation(RoomInformation roomInformation);
        public bool RemoveRoomInformation(int roomId);
        public bool UpdateRoomInformation(RoomInformation roomInformation);
        public void WriteFile(List<RoomInformationDto> roomInformationDtos, string filePath, string type);
        public List<RoomInformationDto> GetRoomInformations(string path, string type);
        public RoomInformationDto GetRoomInformation(string path, string type, int roomId);
        public bool AddRoomInformation(string path, string type, RoomInformationDto roomInformation);
        public bool RemoveRoomInformation(string path, string type, int roomId);
        public bool UpdateRoomInformation(string path, string type, RoomInformation roomInformation);
    }
}
