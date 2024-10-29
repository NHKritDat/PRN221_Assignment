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
    public class DeleteModel : PageModel
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IRoomTypeService _roomTypeService;

        public DeleteModel(IRoomInformationService roomInformationService, IBookingDetailService bookingDetailService, IRoomTypeService roomTypeService)
        {
            _roomInformationService = roomInformationService;
            _bookingDetailService = bookingDetailService;
            _roomTypeService = roomTypeService;
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;
        private string type = ".xml";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";
        private string pathRoomType = "..\\Hotel_Daos\\roomtype";
        private string pathBookingDetail = "..\\Hotel_Daos\\bookingdetail";

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _roomInformationService.GetRoomInformations(pathRoomInformation, type) == null)
            {
                return NotFound();
            }
            var roominformation = ToModel(_roomInformationService.GetRoomInformation(pathRoomInformation, type, id.Value));

            if (roominformation != null)
            {
                bool has = false;
                _bookingDetailService.GetBookingDetails(pathBookingDetail, type).ForEach(b =>
                {
                    if (b.RoomId == roominformation.RoomId)
                    {
                        has = true;
                        return;
                    }
                });
                if (has)
                {
                    roominformation.RoomStatus = 0;
                    RoomInformation = roominformation;
                    _roomInformationService.UpdateRoomInformation(pathRoomInformation, type, RoomInformation);
                }
                else
                {
                    RoomInformation = roominformation;
                    _roomInformationService.RemoveRoomInformation(pathRoomInformation, type, RoomInformation.RoomId);
                }
            }

            return RedirectToPage("./Index");
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
        private RoomType ToModel(RoomTypeDto dto)
        {
            RoomType type = new RoomType();
            type.RoomTypeId = dto.RoomTypeId;
            type.RoomTypeName = dto.RoomTypeName;
            type.TypeDescription = dto.TypeDescription;
            type.TypeNote = dto.TypeNote;
            return type;
        }
    }
}
