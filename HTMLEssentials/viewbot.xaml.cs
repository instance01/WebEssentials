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
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using HtmlAgilityPack;

namespace HTMLEssentials
{
    public partial class viewbot : Window
    {
        public viewbot()
        {
            InitializeComponent();
        }


        Dictionary<string, int> proxylist = new Dictionary<string, int>();

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

        public void parsePage(HtmlAgilityPack.HtmlDocument doc)
        {
            string source = doc.DocumentNode.InnerHtml;

            foreach (Match m in Regex.Matches(source, @"<td><span>([\s\S]*?)(\d+)</td>"))
            {
                string port = m.Groups[2].Value;
                string ip = m.Groups[1].Value;

                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(ip);

                HtmlNode styles = html.DocumentNode.SelectSingleNode("style");
                List<string> allstyles = styles.InnerText.Split('.').ToList();

                string ip2 = ip;

                foreach (string line in allstyles)
                {
                    if (line.Length != 1 && line.Contains("display:none"))
                    {
                        string regex = line.Substring(0, line.IndexOf('{'));
                        string cond = string.Format(@"<span class=""{0}"">(.*?)</span>", regex);
                        ip2 = Regex.Replace(ip2, cond, "").Trim();
                    }

                    ip2 = ip2.Replace("." + line, "");
                }

                ip2 = Regex.Replace(ip2, @"(<(div|span) style=""display:none"">\d+.*?>|<.*?>)", "").Trim();
                this.Dispatcher.BeginInvoke(new Action(() => { updateTextbox(ip2, port); })); 
                //textbox3.AppendText(string.Format("{0}:{1}\n", ip2, port));
                proxylist.Add(ip2, Convert.ToInt32(port));
            }

            this.Dispatcher.BeginInvoke(new Action(() => { updateButtons(); })); 
            //startbtn.IsEnabled = true;
        }

        public void connectThroughProxy(string target, string proxy, int proxyport)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target);
            WebProxy myproxy = new WebProxy(proxy, proxyport);
            myproxy.BypassProxyOnLocal = false;
            request.Proxy = myproxy;
            request.Method = "GET";
            request.KeepAlive = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        public void updateTextbox(string ip, string port)
        {
            textbox3.AppendText(string.Format("{0}:{1}\n", ip, port));
        }

        public void updateButtons()
        {
            startbtn.IsEnabled = true;
            fetchproxiesbtn.IsEnabled = true;
        }

        private void start_click(object sender, RoutedEventArgs e)
        {
            if (textbox1.Text != "")
            {
                int count = Convert.ToInt32(textbox2.Text);
                string target = textbox1.Text;
                Thread t = new Thread(() => visit(count, target));
                t.Start();
                startbtn.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Target?");
            }
        }

        private void fetchproxies_click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(new ThreadStart(getProxies));
            t.Start();
            fetchproxiesbtn.IsEnabled = false;
        }

        public void visit(int countt, string target)
        {
            int count = countt;
            foreach (string proxy in proxylist.Keys)
            {
                count -= 1;
                if (count < 0)
                {
                    return;
                }
                connectThroughProxy(target, proxy, proxylist[proxy]);
            }
            this.Dispatcher.BeginInvoke(new Action(() => { label4.Content = "last finished time (" + Convert.ToString(count) + "): " + DateTime.Now.ToString(); startbtn.IsEnabled = true; })); 
            //label4.Content = "last finished time (" + Convert.ToString(count) + "): " + DateTime.Now.ToString();
        }

        public void getProxies()
        {
            parsePage(downloadSource());
        }
    }
}
