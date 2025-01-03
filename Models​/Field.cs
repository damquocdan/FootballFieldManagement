using System;
using System.Collections.Generic;

namespace FootballFieldManagement.Models​;

public partial class Field
{
    public int FieldId { get; set; }

    public string FieldName { get; set; } = null!;

    public int Capacity { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
