using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class Field
{
    [Display(Name = "Mã sân")]
    public int FieldId { get; set; }

    [Display(Name = "Tên sân")]
    public string FieldName { get; set; } = null!;

    [Display(Name = "Sức chứa")]
    public int Capacity { get; set; }

    [Display(Name = "Giá thuê")]
    public decimal Price { get; set; }

    [Display(Name = "Danh sách đặt sân")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
