using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class Booking
{
    [Display(Name = "Mã đặt sân")]
    public int BookingId { get; set; }

    [Display(Name = "Mã khách hàng")]
    public int? CustomerId { get; set; }

    [Display(Name = "Mã sân bóng")]
    public int? FieldId { get; set; }

    [Display(Name = "Thời gian đặt")]
    public DateTime BookingTime { get; set; }

    [Display(Name = "Thời lượng (giờ)")]
    public int? Duration { get; set; }

    [Display(Name = "Tổng giá sân")]
    public decimal? TotalPrice { get; set; }

    [Display(Name = "Tổng giá dịch vụ")]
    public decimal? TotalServicePrice { get; set; }

    [Display(Name = "Tổng số tiền")]
    public decimal? TotalAmount { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    [Display(Name = "Khách hàng")]
    public virtual Customer? Customer { get; set; }

    [Display(Name = "Sân bóng")]
    public virtual Field? Field { get; set; }

    [Display(Name = "Thanh toán")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
