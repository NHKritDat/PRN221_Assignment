using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.RoomXml
{
    public class EditModel : PageModel
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;

        public EditModel(IRoomInformationService roomInformationService, IRoomTypeService roomTypeService)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;
        private string type = ".xml";
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
            RoomInformation = roominformation;
            ViewData["RoomTypeId"] = new SelectList(ToListModel(_roomTypeService.GetRoomTypes(pathRoomType, type)), "RoomTypeId", "RoomTypeName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("RoomInformation.RoomType");
            if (!ModelState.IsValid)
            {
                ViewData["RoomTypeId"] = new SelectList(ToListModel(_roomTypeService.GetRoomTypes(pathRoomType, type)), "RoomTypeId", "RoomTypeName");
                return Page();
            }

            try
            {
                _roomInformationService.UpdateRoomInformation(pathRoomInformation, type, RoomInformation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomInformationExists(RoomInformation.RoomId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RoomInformationExists(int id)
        {
            return _roomInformationService.GetRoomInformation(pathRoomInformation, type, id) != null;
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
