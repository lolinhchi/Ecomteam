using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public string Anh { get; set; }
        public string Name { get; set; }
        public Double Gia { get; set; }
        public Double USD { get; set; }

    }
}
