using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class Customer
{
    [Display(Name = "Mã khách hàng")]
    public int CustomerId { get; set; }

    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = null!;

    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = null!;

    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Địa chỉ")]
    public string? Address { get; set; }

    [Display(Name = "Danh sách đặt sân")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
