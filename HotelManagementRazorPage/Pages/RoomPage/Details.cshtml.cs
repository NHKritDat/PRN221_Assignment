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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagementRazorPage.Pages.RoomPage
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _roomInformationService.GetRoomInformations() == null)
            {
                return NotFound();
            }

            var roominformation = _roomInformationService.GetRoomInformation(id.Value);
            if (roominformation == null)
            {
                return NotFound();
            }
            else
            {
                roominformation.RoomType = _roomTypeService.GetRoomType(roominformation.RoomTypeId);
                RoomInformation = roominformation;
            }
            return Page();
        }
    }
}
