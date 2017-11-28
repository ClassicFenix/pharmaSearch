using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSearch.Parsers
{
    public class HtmlDoc
    {
        public static HtmlDocument GetHtmlDocument(string Url)
        {
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            return web.Load(Url);
        }
    }
}
