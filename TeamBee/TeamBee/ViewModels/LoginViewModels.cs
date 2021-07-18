using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public class LoginViewModels
    {
        [Required(ErrorMessage = "Thông tin đăng nhập không được để trống")]
        [StringLength(255, ErrorMessage = "Thông tin đăng nhập từ 6 đến 255 kí tự", MinimumLength = 6)]
        [Display(Name = "Email hoặc số điện thoại")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(25, ErrorMessage = "Mật khẩu từ 6 đến 25 kí tự", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

    }
}
