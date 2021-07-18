using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public class OrderViewModels
    {
        public int Id { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhone { get; set; }

        public string ShipName { get; set; }
        public string Address { get; set; }
        public string GhiChu { get; set; }
        public string NgayGiao{get;set;}
        public int Status { get; set; }
    }
}
