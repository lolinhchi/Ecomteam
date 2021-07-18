using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Models
{

    [Table("QuangCao")]
    public class QuangCao
    {
        [Key]
        public int Id { get; set; }

        public string Ten { get; set; }
        public int PhanTram { get; set; }
        public string Anh { get; set; }
        public string LienKet { get; set; }
        public int Status { get; set; }
    }
}
