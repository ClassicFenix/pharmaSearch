using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSearch.Parsers
{
    public class ParserPharmaKopeck: ISearch
    {
        public const string BaseUrl = "https://farmakopeyka.ru";
        public const string SearchUrl = BaseUrl + "/search/?q=";
        string url = "";
        public IList<ProductInfo> Search(string SearchString)
        {
            url = SearchUrl + SearchString.Replace(' ', '+');
            return LoadResult(HtmlDoc.GetHtmlDocument(url));
        }

        IList<ProductInfo> LoadResult(HtmlDocument HD)
        {
            var ResultCollection = HD.DocumentNode.SelectNodes("//div[@class='" + "title" + "']"); //Name
            var resultList = new List<ProductInfo>();
            for (int i = 0; i < ResultCollection.Count; i++)
            {
                if (ResultCollection[i].InnerText.Trim() != "Поиск")
                {
                    resultList.Add(new ProductInfo() { PharmaName = "Фармакопейка", Name = ResultCollection[i].InnerText });
                }
            }

            var PriceNodeCollection = HD.DocumentNode.SelectNodes("//div[@class='" + "col-sm-6 price" + "']"); //Price
            
            var PriceCollection = HD.DocumentNode.SelectNodes("//span[@class='" + "current-price" + "']"); //Наименование

            if (PriceCollection != null)
            {
                for (int i = 0, j = 0; i < PriceNodeCollection.Count; i++)
                {
                    if (PriceNodeCollection[i].InnerText != "")
                    {
                        string s = PriceCollection[j].InnerText.Split(new char[] { ' ' })[0];
                        resultList[i].Price = Convert.ToDouble(s.Replace('.',','));
                        j++;
                    }
                }

                var ImageCollection = HD.DocumentNode.SelectNodes("//div[@class='" + "part-1" + "']"); //Images and links
                for (int i = 0; i < ImageCollection.Count; i++)
                {
                    try
                    {
                        resultList[i].Link = BaseUrl + ImageCollection[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "");
                    }
                    catch
                    {
                        resultList[i].Link = url;
                    }
                     
                }
            }
            for (int i = resultList.Count-1; i>0; i--)
            {
                if (resultList[i].Price == 0)
                    resultList.RemoveAt(i);
            }
            return resultList;
        }
    }
}
