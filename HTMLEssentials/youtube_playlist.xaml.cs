using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace HTMLEssentials
{
    /// <summary>
    /// Interaktionslogik für youtube_playlist.xaml
    /// </summary>
    public partial class youtube_playlist : Window
    {
        public youtube_playlist()
        {
            InitializeComponent();
        }


        public HtmlAgilityPack.HtmlDocument downloadSource()
        {
            string res = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.hidemyass.com/proxy-list/");
            req.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream stream = response.GetResponseStream();
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            { res = reader.ReadToEnd(); }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(new StringReader(res));
            return doc;
        }

        private void fetchbtn_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
