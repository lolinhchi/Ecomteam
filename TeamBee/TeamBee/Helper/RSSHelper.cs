using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using TeamBee.Models;
namespace TeamBee.Helper
{
    public class RSSHelper
    {
        public static List<Item> read(string url)
        {
            List<Item> listItems = new List<Item>();
            try
            {
                XPathDocument doc = new XPathDocument(url);
                XPathNavigator navigator = doc.CreateNavigator();
                XPathNodeIterator nodes = navigator.Select("//item");
                while (nodes.MoveNext())
                {
                    XPathNavigator node = nodes.Current;
                    string s = node.SelectSingleNode("description").Value;
                    int x = s.LastIndexOf("=");
                    string ss = s.Substring(x + 1);
                    x = ss.IndexOf(">");
                    ss = ss.Substring(0, x - 1);
                    listItems.Add(new Item
                    {
                        Description = ss,
                        Size =ss.Trim().Length,
                        Guid = node.SelectSingleNode("guid").Value,
                        Link = node.SelectSingleNode("link").Value,
                        PubDate = node.SelectSingleNode("pubDate").Value,
                        Title = node.SelectSingleNode("title").Value
                    });
                }
            }
            catch
            {
                listItems = null;
            }
            return listItems;
        }
    }
}
