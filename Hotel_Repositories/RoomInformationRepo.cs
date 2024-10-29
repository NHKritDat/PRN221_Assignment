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
    public class RoomInformationRepo : IRoomInformationRepo
    {
        public bool AddRoomInformation(RoomInformation roomInformation) => RoomInformationDao.Instance.AddRoomInformation(roomInformation);

        public bool AddRoomInformation(string path, string type, RoomInformationDto roomInformation) => RoomInformationDao.Instance.AddRoomInformation(path, type, roomInformation);

        public RoomInformation? GetRoomInformation(int roomId) => RoomInformationDao.Instance.GetRoomInformation(roomId);

        public RoomInformationDto GetRoomInformation(string path, string type, int roomId) => RoomInformationDao.Instance.GetRoomInformation(path, type, roomId);

        public List<RoomInformation> GetRoomInformations() => RoomInformationDao.Instance.GetRoomInformations();

        public List<RoomInformationDto> GetRoomInformations(string path, string type) => RoomInformationDao.Instance.GetRoomInformations(path, type);

        public bool RemoveRoomInformation(int roomId) => RoomInformationDao.Instance.RemoveRoomInformation(roomId);

        public bool RemoveRoomInformation(string path, string type, int roomId) => RoomInformationDao.Instance.RemoveRoomInformation(path, type, roomId);

        public bool UpdateRoomInformation(RoomInformation roomInformation) => RoomInformationDao.Instance.UpdateRoomInformation(roomInformation);

        public bool UpdateRoomInformation(string path, string type, RoomInformation roomInformation) => RoomInformationDao.Instance.UpdateRoomInformation(path, type, roomInformation);

        public void WriteFile(List<RoomInformationDto> roominformations, string filePath, string type) => RoomInformationDao.Instance.WriteFile(roominformations, filePath, type);
    }
}
