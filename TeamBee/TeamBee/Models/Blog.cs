using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Models
{
    [Table("Blog_Category")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

       
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày chỉnh sửa cuối")]
        public DateTime DateModify { get; set; }

        [Required(ErrorMessage = "Chưa nhập hiển thị cho URL")]
        public string URL { get; set; }

        // Phần này dành cho khóa ngoại
        #region Foreign Keys
        public ICollection<BlogItem> BlogItem { get; set; }
        #endregion
    }
}
