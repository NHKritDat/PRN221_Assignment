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

namespace HotelManagementRazorPage.Pages.RoomPage
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
            RoomInformation = roominformation;
            ViewData["RoomTypeId"] = new SelectList(_roomTypeService.GetRoomTypes(), "RoomTypeId", "RoomTypeName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("RoomInformation.RoomType");
            if (!ModelState.IsValid)
            {
                ViewData["RoomTypeId"] = new SelectList(_roomTypeService.GetRoomTypes(), "RoomTypeId", "RoomTypeName");
                return Page();
            }

            try
            {
                _roomInformationService.UpdateRoomInformation(RoomInformation);
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
            return _roomInformationService.GetRoomInformation(id) != null;
        }
    }
}
