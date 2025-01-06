using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class BookingService
{
    [Display(Name = "Mã dịch vụ đặt sân")]
    public int BookingServiceId { get; set; }

    [Display(Name = "Mã đặt sân")]
    public int? BookingId { get; set; }

    [Display(Name = "Mã dịch vụ")]
    public int? ServiceId { get; set; }

    [Display(Name = "Số lượng")]
    public int Quantity { get; set; }

    [Display(Name = "Tổng giá")]
    public decimal TotalPrice { get; set; }

    [Display(Name = "Đặt sân")]
    public virtual Booking? Booking { get; set; }

    [Display(Name = "Dịch vụ")]
    public virtual Service? Service { get; set; }
}
