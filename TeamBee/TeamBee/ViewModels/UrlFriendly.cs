using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamBee.ViewModels
{
    public static class UrlFriendly
    {
        public static string ToUrlFriendly(this string tieuDe)
        {
            string str = tieuDe.ToLower().Trim();

            //thay thế tiếng Việt
            str = Regex.Replace(str, @"[áàảạãăắằẳẵặâấầẩẫậ]", "a");
            str = Regex.Replace(str, @"[éèẻẽẹ]", "e");
            str = Regex.Replace(str, @"[êếềểễệ]", "e");
            str = Regex.Replace(str, @"[đ]", "d");
            str = Regex.Replace(str, @"[íìỉĩị]", "i");
            str = Regex.Replace(str, @"[óòỏõọ]", "o");
            str = Regex.Replace(str, @"[ôốồổỗộ]", "o");
            str = Regex.Replace(str, @"[ơớờởỡợ]", "o");
            str = Regex.Replace(str, @"[ýỳỷỹỵ]", "y");
            str = Regex.Replace(str, @"[úùủũụ]", "u");
            str = Regex.Replace(str, @"[ưứừửữự]", "u");

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim();
            str = Regex.Replace(str, @"\s", "-");
            string[] arr = str.Split('-');
            string ret = "";
            foreach (var item in arr)
            {
                if (item.Length > 0)
                {
                    ret += item + '-';
                }
            }
            ret = ret.TrimEnd(ret[ret.Length - 1]);
            return ret;
        }
    }
}
