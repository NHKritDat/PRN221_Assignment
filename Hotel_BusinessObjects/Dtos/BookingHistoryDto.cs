using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BusinessObjects.Dtos
{
    public class BookingHistoryDto
    {
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? CustomerFullName { get; set; }
        public byte? BookingStatus { get; set; }
    }
}
