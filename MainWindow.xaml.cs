using PharmaSearch.Model;
using PharmaSearch.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PharmaSearch
{
    public partial class MainWindow : Window
    {
        List<Pharma> PharmaList;
        List<Drug> dictionary = new List<Drug>();

        void InitializePharmaList()
        {
            PharmaList = new List<Pharma>
            {
                new Pharma(new ParserFamily()) { Name = "Семейная", IsActive = true,  BackColor=Color.FromRgb(255, 100, 100)},
                new Pharma(new ParserFromStore()) { Name = "Аптека от склада", IsActive = true },
                new Pharma(new ParserPharmaKopeck()) { Name = "Фармакопейка", IsActive = true }
            };
            PharmaGrid.ItemsSource = PharmaList;
        }

        void InitializeDrugList()
        {
            SearchControl.Items.Clear();
            
            using (FileStream fs = new FileStream("dictionary.txt", FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fs))
                {
                    string line;
                    while (( line = sr.ReadLine()) != null)
                    {
                        dictionary.Add(new Drug() { Name = line.TrimEnd() });
                    }
                }
            }
            SearchControl.ItemsSource = dictionary;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializePharmaList();
            InitializeDrugList();
        }

        public delegate void Invoke();

        private void Dispatch(Invoke invokeMethod)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, invokeMethod);
        }

        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
            btSearch.IsEnabled = false;
            btSearch.Content = "Идёт поиск...";
            dictionary.Add(new Drug() { Name = SearchControl.Text });
            var t = new Task(
                delegate
                {
                    string searchStr = "";
                    Dispatch(delegate { searchStr = SearchControl.Text; });

                    var list = new List<ProductInfo>();

                    for (int i = 0; i < PharmaList.Count; i++)
                    {
                        if (PharmaList[i].IsActive)
                        list.AddRange(PharmaList[i].Search(searchStr));
                    }
                    for (int i = list.Count - 1; i > 0; i--)
                    {
                        if (list[i].Price == 0)
                            list.RemoveAt(i);
                    }

                    Dispatch(delegate { GridResult.ItemsSource = list; });
                    Dispatch(delegate { btSearch.IsEnabled = true; btSearch.Content = "Найти"; });
                }
            );
            t.Start();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream("dictionary.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var row in dictionary)
                        sw.WriteLine(row.Name);
                }
            }
        }

        private void SearchControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btSearch_Click(btSearch, new RoutedEventArgs());
        }
    }
    
}
