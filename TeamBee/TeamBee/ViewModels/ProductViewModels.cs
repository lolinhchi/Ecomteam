using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public class ProductViewModels
    { 
        public int Id { get; set; }
        public int Category_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } = 0;
        public int SalePrice { get; set; } = 0;  
        public IFormFile file { get; set; }
    }
}
