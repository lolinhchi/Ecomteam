using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;

namespace TeamBee.ViewModels
{
    public class DonHangChiTiet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SoLuong { get; set; }
        public int Gia { get; set; }


        public DonHangChiTiet() { }
        private readonly TeamBeeDbContext _context;

        public DonHangChiTiet(TeamBeeDbContext context)
        {
            _context = context;
        }

        public List<DonHangChiTiet> GetDonHangChiTiet(int id)
        {
            List<DonHangChiTiet> product = new List<DonHangChiTiet>();


            var query = (from p in _context.Products
                         join c in _context.CartDetails
                         on p.Id equals c.Product_Id
                         where c.Cart_Id == id

                         select new DonHangChiTiet
                         {
                             Id = p.Id,
                             Name = p.Name,
                             SoLuong = c.Quantity,
                             Gia = c.PriceSingle
                         });
            product = query.ToList();

            return product;
        }
    }
}
