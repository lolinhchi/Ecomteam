using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public User User { get; set; }

        [Display(Name = "Tổng số tiền")]
        public int TotalPrice { get; set; } = 0;

        [Display(Name = "Số lượng hàng trong giỏ")]
        public int TotalQuantity { get; set; } = 0;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string ShipEmail { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20, ErrorMessage = "Số điện thoại phải từ 10 đến 20 kí tự", MinimumLength = 10)]
        public string ShipPhone{ get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên không quá 100 kí tự")]
        [Display(Name = "Họ")]
        public string ShipName { get; set; }


        [Required(ErrorMessage = "Địa Chỉ không được để trống")]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        public string Status { get; set; }
        public string GhiChu { get; set; }
        public string Pay { get; set; }
        // Phần này dành cho khóa ngoại
        #region Foreign Keys
        public ICollection<CartDetail> CartDetails { get; set; }
        #endregion
    }
}
