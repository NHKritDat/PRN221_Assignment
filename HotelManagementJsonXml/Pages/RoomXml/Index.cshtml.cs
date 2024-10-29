using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.RoomXml
{
    public class IndexModel : PageModel
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;

        public IndexModel(IRoomInformationService roomInformationService, IRoomTypeService roomTypeService)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
        }

        public IList<RoomInformation> RoomInformation { get; set; } = default!;
        private string type = ".xml";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";
        private string pathRoomType = "..\\Hotel_Daos\\roomtype";

        public async Task OnGetAsync()
        {
            if (_roomInformationService.GetRoomInformations(pathRoomInformation, type) != null)
            {
                var roomInformations = ToListModel(_roomInformationService.GetRoomInformations(pathRoomInformation, type));
                roomInformations.ForEach(r => r.RoomType = ToModel(_roomTypeService.GetRoomType(pathRoomType, type, r.RoomTypeId)));
                RoomInformation = roomInformations;
            }
        }
        private RoomType ToModel(RoomTypeDto dto)
        {
            RoomType type = new RoomType();
            type.RoomTypeId = dto.RoomTypeId;
            type.RoomTypeName = dto.RoomTypeName;
            type.TypeDescription = dto.TypeDescription;
            type.TypeNote = dto.TypeNote;
            return type;
        }
        private List<RoomInformation> ToListModel(List<RoomInformationDto> dtos)
        {
            List<RoomInformation> list = new List<RoomInformation>();
            dtos.ForEach(d => list.Add(ToModel(d)));
            return list;
        }
        private RoomInformation ToModel(RoomInformationDto dto)
        {
            RoomInformation room = new RoomInformation();
            room.RoomId = dto.RoomId;
            room.RoomNumber = dto.RoomNumber;
            room.RoomDetailDescription = dto.RoomDetailDescription;
            room.RoomMaxCapacity = dto.RoomMaxCapacity;
            room.RoomTypeId = dto.RoomTypeId;
            room.RoomStatus = dto.RoomStatus;
            room.RoomPricePerDay = dto.RoomPricePerDay;
            return room;
        }
    }
}
