using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSearch.Parsers
{
    public class ParserFamily : ISearch
    {
        private const string BaseUrl = "http://apteka-omsk.ru";
        public const string SearchUrl = BaseUrl + "/medicine_price_search?title=";
        string url = "";
        public IList<ProductInfo> Search(string SearchString)
        {
            url = SearchUrl + SearchString.Replace(' ', '+');
            return LoadResult(HtmlDoc.GetHtmlDocument(url));
        }

        IList<ProductInfo> LoadResult(HtmlDocument HD)
        {
            var ResultCollection = HD.DocumentNode.SelectNodes("//tbody");
            var resultList = new List<ProductInfo>();

            foreach (var row in ResultCollection[0].ChildNodes)
            {
                if (!row.HasChildNodes) continue;
                if (row.InnerText.IndexOf("ничего не найдено") > -1) break;
                

                try
                {
                    resultList.Add(new ProductInfo()
                    {
                        Name = row.ChildNodes[0].InnerText,
                        Price = Convert.ToDouble(row.ChildNodes[1].InnerText.Replace('.', ',')),
                        PharmaName = "Семейная " + row.ChildNodes[2].InnerText, Link = url
                    });
                }
                catch(FormatException ex)
                {
                    //writelog
                }
            }
            return resultList;
        }
    }
}
