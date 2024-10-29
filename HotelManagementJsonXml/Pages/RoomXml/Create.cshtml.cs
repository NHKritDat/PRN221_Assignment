using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.RoomXml
{
    public class CreateModel : PageModel
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;

        public CreateModel(IRoomInformationService roomInformationService, IRoomTypeService roomTypeService)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
        }

        public IActionResult OnGet()
        {
            ViewData["RoomTypeId"] = new SelectList(ToListModel(_roomTypeService.GetRoomTypes(pathRoomType, type)), "RoomTypeId", "RoomTypeName");
            return Page();
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;
        private string type = ".xml";
        private string pathRoomInformation = "..\\Hotel_Daos\\roominformation";
        private string pathRoomType = "..\\Hotel_Daos\\roomtype";


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("RoomInformation.RoomType");
            if (!ModelState.IsValid || _roomInformationService.GetRoomInformations(pathRoomInformation, type) == null || RoomInformation == null)
            {
                ViewData["RoomTypeId"] = new SelectList(ToListModel(_roomTypeService.GetRoomTypes(pathRoomType, type)), "RoomTypeId", "RoomTypeName");
                return Page();
            }

            _roomInformationService.AddRoomInformation(pathRoomInformation, type, ToDto(RoomInformation));

            return RedirectToPage("./Index");
        }
        private RoomInformationDto ToDto(RoomInformation roomInformation)
        {
            RoomInformationDto dto = new RoomInformationDto();
            dto.RoomNumber = roomInformation.RoomNumber;
            dto.RoomDetailDescription = roomInformation.RoomDetailDescription;
            dto.RoomMaxCapacity = roomInformation.RoomMaxCapacity;
            dto.RoomTypeId = roomInformation.RoomTypeId;
            dto.RoomStatus = roomInformation.RoomStatus;
            dto.RoomPricePerDay = roomInformation.RoomPricePerDay;
            return dto;
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
        private List<RoomType> ToListModel(List<RoomTypeDto> dtos)
        {
            List<RoomType> list = new List<RoomType>();
            dtos.ForEach(d => list.Add(ToModel(d)));
            return list;
        }
    }
}
