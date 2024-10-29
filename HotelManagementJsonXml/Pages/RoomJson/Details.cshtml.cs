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

namespace HotelManagementJsonXml.Pages.RoomJson
{
    public class DetailsModel : PageModel
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;

        public DetailsModel(IRoomInformationService roomInformationService, IRoomTypeService roomTypeService)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
        }

        public RoomInformation RoomInformation { get; set; } = default!;
        private string type = ".json";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";
        private string pathRoomType = "..\\Hotel_Daos\\roomtype";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _roomInformationService.GetRoomInformations(pathRoomInformation, type) == null)
            {
                return NotFound();
            }

            var roominformation = ToModel(_roomInformationService.GetRoomInformation(pathRoomInformation, type, id.Value));
            if (roominformation == null)
            {
                return NotFound();
            }
            else
            {
                roominformation.RoomType = ToModel(_roomTypeService.GetRoomType(pathRoomType, type, roominformation.RoomTypeId));
                RoomInformation = roominformation;
            }
            return Page();
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
