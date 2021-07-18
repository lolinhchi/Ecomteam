using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public class BlogItemViewModel
    {

        public int Id { get; set; }
        public int Category_Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile file { get; set; }

    }
}
