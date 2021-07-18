using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using TeamBee.Helper;

namespace TeamBee.Controllers
{
    public class RssController : Controller
    {
        public IActionResult Index()
        {
            string url = "https://tuoitre.vn/rss/suc-khoe.rss";
            ViewBag.listItems = RSSHelper.read(url);
            return View();
            // gửi file thôi  thả cái lol
        }
    }
}
