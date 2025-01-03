using System;
using System.Collections.Generic;

namespace FootballFieldManagement.Models​;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime PaymentTime { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
