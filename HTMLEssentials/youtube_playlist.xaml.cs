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
using System.Threading;
using System.Text.RegularExpressions;

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

            //this.videoelement.Source = new Uri("http://www.youtube.com/v/9KX1OQDLjAY");
            /*media1.LoadedBehavior = MediaState.Manual;
            media1.Source = new Uri(@"https://youtube.googleapis.com/v/9KX1OQDLjAY");
            media1.Play();*/
        }

        public static List<string> links = new List<string>();

        public HtmlAgilityPack.HtmlDocument downloadSource(string url)
        {
            string res = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
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
            string target = textbox1.Text;
            if (textbox1.Text != "")
            {
                Thread t = new Thread(() => getPage(target));
                t.Start();
                fetchbtn.IsEnabled = false;
                // todo: start playing songs
            }
        }

        public void getPage(string target)
        {
            parsePage(downloadSource(target));
        }

        public void parsePage(HtmlAgilityPack.HtmlDocument doc)
        {
            string source = doc.DocumentNode.InnerHtml;

            //string reg = @"data-video-id="".*"" data-video-clip-end=""None""";
            //MessageBox.Show(reg);

            foreach (Match m in Regex.Matches(source, @"data-video-id="".*"" data-video-clip-end=""None"""))
            {
                string videoid_ = m.Groups[0].Value;
                string videoid = videoid_.Substring(videoid_.IndexOf("=") + 2, videoid_.IndexOf('"', videoid_.IndexOf("=") + 2) - videoid_.IndexOf("=") - 2);
                this.Dispatcher.BeginInvoke(new Action(() => { listbox1.Items.Add(videoid); links.Add(videoid); }));
            }
            if (links.Count < 1)
            {
                MessageBox.Show("Failed getting any urls");
                this.Dispatcher.BeginInvoke(new Action(() => { fetchbtn.IsEnabled = true; }));
                return;
            }
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                fetchbtn.IsEnabled = true;
                Form1 f = new Form1();
                f.urls = links.ToArray();
                f.Show();
                f.start(links.ToArray());
            })); 
        }


        // form movement

        Boolean drag = false;
        int mousex, mousey;

        private void base_mousedown(object sender, MouseButtonEventArgs e)
        {
            drag = true;
            mousex = System.Windows.Forms.Cursor.Position.X - Convert.ToInt32(this.Left);
            mousey = System.Windows.Forms.Cursor.Position.Y - Convert.ToInt32(this.Top);
        }

        private void base_mousemove(object sender, MouseEventArgs e)
        {
            if (drag == true)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - mousey;
                this.Left = System.Windows.Forms.Cursor.Position.X - mousex;
            }
        }

        private void base_mouseup(object sender, MouseButtonEventArgs e)
        {
            drag = false;
        }
    }

}
