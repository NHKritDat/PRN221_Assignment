using System;
using System.Collections.Generic;

namespace Hotel_BusinessObjects.Models
{
    public partial class Customer
    {
        public Customer()
        {
            BookingReservations = new HashSet<BookingReservation>();
        }

        public int CustomerId { get; set; }
        public string? CustomerFullName { get; set; }
        public string? Telephone { get; set; }
        public string EmailAddress { get; set; } = null!;
        public DateTime? CustomerBirthday { get; set; }
        public byte? CustomerStatus { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<BookingReservation> BookingReservations { get; set; }
    }
}
