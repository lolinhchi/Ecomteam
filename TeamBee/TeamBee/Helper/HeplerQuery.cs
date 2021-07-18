using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.ViewModels;

namespace TeamBee.Helper
{
    public static class HelperQuery
    {
        public static void Test(Func<IQueryable<ItemProductsViewModel>, IOrderedQueryable<ItemProductsViewModel>> param)
        {
            var test = 0;
        }
    }
}
