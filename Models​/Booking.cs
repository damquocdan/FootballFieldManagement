using System;
using System.Collections.Generic;

namespace FootballFieldManagement.Models​;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int FieldId { get; set; }

    public DateTime BookingTime { get; set; }

    public int Duration { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal TotalServicePrice { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Field Field { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
