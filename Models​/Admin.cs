using System;
using System.ComponentModel.DataAnnotations;

namespace FootballFieldManagement.Models;

public partial class Admin
{
    [Display(Name = "Mã quản trị viên")]
    public int AdminId { get; set; }

    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = null!;

    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = null!;
}
