using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;

namespace TeamBee.ViewModels
{
    public class CommentViewModels
    {

        public string Name { get; set; }

        public string Content { get; set; }
        public int Stars { get; set; }
        public DateTime DateCreate { get; set; }

        public CommentViewModels() { }
        private readonly TeamBeeDbContext _context;

        public CommentViewModels(TeamBeeDbContext context)
        {
            _context = context;
        }

        public List<CommentViewModels> GetComments(int idpro)
        {
            List<CommentViewModels> product = new List<CommentViewModels>();


            var query = (from c in _context.Comments
                         join u in _context.Users
                         on c.User_Id equals u.Id
                         where c.Product_Id == idpro
                         orderby c.Id descending
                         select new CommentViewModels
                         {
                            
                             Name = u.NameLast +' ' +u.NameFirst,
                             Content = c.Content,
                             Stars = (int) c.Stars,
                             DateCreate = c.DateCreate

                         });
            product = query.ToList();

            return product;
        }
    }


}
