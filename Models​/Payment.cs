using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class Payment
{
    [Display(Name = "Mã thanh toán")]
    public int PaymentId { get; set; }

    [Display(Name = "Mã đặt sân")]
    public int BookingId { get; set; }

    [Display(Name = "Phương thức thanh toán")]
    public string PaymentMethod { get; set; } = null!;

    [Display(Name = "Số tiền")]
    public decimal Amount { get; set; }

    [Display(Name = "Thời gian thanh toán")]
    public DateTime PaymentTime { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
