using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;
using TeamBee.ViewModels;

namespace TeamBee.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Display(Name = "Đơn giá")]
        public int Price { get; set; } = 0;


        [Display(Name = "Giá Khuyến Mãi")]
        public int SalePrice { get; set; } = 0;

        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; } = "#";

        [Range(0.0, 5.0)]
        [Display(Name = "Đánh giá trung bình")]
        public double Stars { get; set; } = 5.0;

        [Display(Name = "Lượt xem")]
        public int Views { get; set; } = 0;

        [Display(Name = "Lượt đặt hàng")]
        public int Orders { get; set; } = 0;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày chỉnh sửa cuối")]
        public DateTime DateModify { get; set; }

        public String URL { get; set; }
        public int Status { get; set; } = 0;
        public ICollection<Comment> Comments { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }


       
    }
}
