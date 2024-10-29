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

namespace HotelManagementRazorPage.Pages.RoomPage
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
            ViewData["RoomTypeId"] = new SelectList(_roomTypeService.GetRoomTypes(), "RoomTypeId", "RoomTypeName");
            return Page();
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("RoomInformation.RoomType");
            if (!ModelState.IsValid || _roomInformationService.GetRoomInformations() == null || RoomInformation == null)
            {
                ViewData["RoomTypeId"] = new SelectList(_roomTypeService.GetRoomTypes(), "RoomTypeId", "RoomTypeName");
                return Page();
            }

            _roomInformationService.AddRoomInformation(RoomInformation);

            return RedirectToPage("./Index");
        }
    }
}
