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
    [Table("BlogItem")]
    public class BlogItem
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Blog_Category")]
        public int Category_BlogId { get; set; }
        public Blog Blog_Category { get; set; }

       
        public string Title { get; set; }

        
        public string Content { get; set; }

   

       

        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; } = "#";

       

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }

 

        public String URL { get; set; }
    
        


    }
}
