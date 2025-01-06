using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class Service
{
    [Display(Name = "Mã dịch vụ")]
    public int ServiceId { get; set; }

    [Display(Name = "Tên dịch vụ")]
    public string ServiceName { get; set; } = null!;

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Giá")]
    public decimal Price { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
