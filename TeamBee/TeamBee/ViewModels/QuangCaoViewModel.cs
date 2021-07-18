using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public class QuangCaoViewModel
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public int PhanTram { get; set; }
        public IFormFile Anh { get; set; }
        public string LienKet { get; set; }
    }
}
