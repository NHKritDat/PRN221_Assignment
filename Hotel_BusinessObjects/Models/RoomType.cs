using System;
using System.Collections.Generic;

namespace Hotel_BusinessObjects.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            RoomInformations = new HashSet<RoomInformation>();
        }

        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = null!;
        public string? TypeDescription { get; set; }
        public string? TypeNote { get; set; }

        public virtual ICollection<RoomInformation> RoomInformations { get; set; }
    }
}
