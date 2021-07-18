using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public class UserViewModels
    {
      
        public int Id { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
     
        public string UserName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20, ErrorMessage = "Số điện thoại phải từ 10 đến 20 kí tự", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(25, ErrorMessage = "Mật khẩu từ 6 đến 25 kí tự", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public string passwordnew { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên không quá 100 kí tự")]
        [Display(Name = "Tên")]
        public string NameFirst { get; set; }

        [Required(ErrorMessage = "Họ không được để trống")]
        [StringLength(100, ErrorMessage = "Họ không quá 100 kí tự")]
        [Display(Name = "Họ")]
        public string NameLast { get; set; }


        [Required(ErrorMessage = "Địa Chỉ không được để trống")]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }
        public int status { get; set; }
    }
}
