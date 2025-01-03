using System;
using System.Collections.Generic;

namespace FootballFieldManagement.Models​;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
