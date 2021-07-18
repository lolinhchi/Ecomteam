using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Models
{
    [Table("Tracking")]
    public class Tracking
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cart")]
        public int Cart_Id { get; set; }
        public Order Cart { get; set; }
        public DateTime Time { get; set; }
        public string ThongTin { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string TongTien { get; set; }

        public string GhiChu { get; set; }
        public string DiaChi { get; set; }
    }
}
