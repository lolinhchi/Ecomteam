using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Models
{
    public class Item
    {
        public string Title
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Link
        {
            get; set;
        }
        public string Guid
        {
            get; set;
        }
        public string PubDate
        {
            get; set;
        }
        public int Size
        {
            get; set;
        }
    }
}
