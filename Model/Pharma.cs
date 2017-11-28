using PharmaSearch.Parsers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PharmaSearch
{
    class Pharma
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public double Progress { get; set; }
        public Color BackColor { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        ISearch Parser;
        public Pharma(ISearch parser)
        {
            Parser = parser;
        }

        public IList<ProductInfo> Search(string request)
        {
            return Parser.Search(request);
        }
        
    }
}
