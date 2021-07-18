using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBee.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên Đăng Nhập không được để trống")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20, ErrorMessage = "Số điện thoại phải từ 10 đến 20 kí tự", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu từ 6 đến 100 kí tự", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

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
        // phân quyền
        public int Permission { get; set; } = 0;
        // check online
        public int Active { get; set; } = 0;
        // xác nhận qua email
        public int Confirm { get; set; } = 0;

        public ICollection<Comment> Comments { get; set; }
       
        public ICollection<Order> Carts { get; set; }
        
    }
}
