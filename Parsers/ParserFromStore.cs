using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSearch.Parsers
{
    class ParserFromStore : ISearch
    {
        public const string BaseUrl = "http://www.apteka-ot-sklada.ru";
        public const string SearchUrl = BaseUrl + "/catalog?q=";

        public IList<ProductInfo> Search(string SearchString)
        {
            string url = SearchUrl + SearchString.Replace(' ', '+');
            return LoadResult(HtmlDoc.GetHtmlDocument(url));
        }
        
        IList<ProductInfo> LoadResult(HtmlDocument HD)
        {
            var ResultCollection = HD.DocumentNode.SelectNodes("//a[@class='" + "product-name" + "']"); //Наименования
            var resultList = new List<ProductInfo>();
            foreach (var result in ResultCollection)
            {
                resultList.Add(new ProductInfo() { PharmaName = "Аптека от склада", Name = result.InnerText, Link = BaseUrl + result.Attributes["href"].Value });
            }

            ResultCollection = HD.DocumentNode.SelectNodes("//span[@class='" + "product-price" + "']"); //Цены
            int i = 0;
            foreach (var result in ResultCollection)
            {
                resultList[i].Price = Convert.ToDouble(result.InnerText.Replace('.', ','));
                i++;
            }
            
            return resultList;
        }
    }
}
