using System;
using System.Collections.Generic;

namespace Hotel_BusinessObjects.Models
{
    public partial class BookingReservation
    {
        public BookingReservation()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int BookingReservationId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public byte? BookingStatus { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
