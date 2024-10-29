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

        public async Task OnGetAsync()
        {
            if (_roomInformationService.GetRoomInformations() != null)
            {
                var roomInformations = _roomInformationService.GetRoomInformations();
                roomInformations.ForEach(r => r.RoomType = _roomTypeService.GetRoomType(r.RoomTypeId));
                RoomInformation = roomInformations;
            }
        }
    }
}
