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

namespace HotelManagementRazorPage.Pages.RoomPage
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _roomInformationService.GetRoomInformations() == null)
            {
                return NotFound();
            }
            var roominformation = _roomInformationService.GetRoomInformation(id.Value);
            if (roominformation != null)
            {
                bool has = false;
                _bookingDetailService.GetBookingDetails().ForEach(b =>
                {
                    if (b.RoomId == roominformation.RoomId)
                    {
                        has = true;
                        return;
                    }
                });
                if (!has)
                {
                    RoomInformation = roominformation;
                    _roomInformationService.RemoveRoomInformation(RoomInformation.RoomId);
                }
                else
                {
                    roominformation.RoomStatus = 0;
                    RoomInformation = roominformation;
                    _roomInformationService.UpdateRoomInformation(RoomInformation);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
