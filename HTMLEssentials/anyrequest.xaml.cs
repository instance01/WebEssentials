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
    /// Interaktionslogik für anyrequest.xaml
    /// </summary>
    public partial class anyrequest : Window
    {
        public anyrequest()
        {
            InitializeComponent();
        }

        private void sendbtn_click(object sender, RoutedEventArgs e)
        {
            string target = targettextbox.Text;
            if (!target.Contains("http"))
            {
                target = "http://" + target;
            }
            string proxy = proxytextbox.Text;
            int proxyport = Convert.ToInt32(proxyporttextbox.Text);
            string useragent = useragenttextbox.Text;
            string host = hosttextbox.Text;
            string contenttype = contenttypetextbox.Text;
            string cookiekey = cookiekeytextbox.Text;
            string cookieval = cookievaltextbox.Text;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target);
            if (proxy != "")
            {
                WebProxy myproxy = new WebProxy(proxy, proxyport);
                myproxy.BypassProxyOnLocal = false;
                request.Proxy = myproxy;
            }
            request.Method = "GET";
            request.KeepAlive = false;
            request.UserAgent = useragent;
            if (host != "")
            {
                request.Host = host;
            }
            request.ContentType = contenttype;
            if (cookiekey != "" && cookieval != "")
            {
                cookieContainer.Add(new Cookie(cookiekey, cookieval));
            }
            request.CookieContainer = cookieContainer;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string html;
            using (Stream strmresponse = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(strmresponse, Encoding.UTF8))
                {
                    html = reader.ReadToEnd();
                }
            }

            responsetextbox.Text = html;
        }
    }
}
