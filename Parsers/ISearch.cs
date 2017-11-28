using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSearch.Parsers
{
    public interface ISearch
    {
        IList<ProductInfo> Search(string SearchString);
    }
}
