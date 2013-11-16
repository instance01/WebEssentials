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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HTMLEssentials
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        System.Windows.Threading.DispatcherTimer timer1;

        private void base_loaded(object sender, RoutedEventArgs e)
        {
            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Tick += new EventHandler(dispatcherTimer_Tick);
            timer1.Interval = TimeSpan.FromMilliseconds(100);
            //dispatcherTimer.Interval = new TimeSpan(0,0,1);
            timer1.Start();
        }

        


        


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Button1.Opacity += 0.1;
            Button2.Opacity += 0.1;
            Button3.Opacity += 0.1;
            Button4.Opacity += 0.1;
            if (Button1.Opacity > 0.9)
            {
                timer1.Stop();
            }
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
            if(drag == true){
                this.Top = System.Windows.Forms.Cursor.Position.Y - mousey;
                this.Left = System.Windows.Forms.Cursor.Position.X - mousex;
            }
        }

        private void base_mouseup(object sender, MouseButtonEventArgs e)
        {
            drag = false;
        }

        private void btn1_click(object sender, RoutedEventArgs e)
        {
            // viewbot
            // gets a bunch of proxies and sends simple get requests
            viewbot v = new viewbot();
            v.Show();
        }

        private void btn2_click(object sender, RoutedEventArgs e)
        {
            // youtube playlist extractor
            // extracts all videos of a playlist and plays them
            youtube_playlist y = new youtube_playlist();
            y.Show();
        }

        private void btn3_click(object sender, RoutedEventArgs e)
        {
            // bruteforcer
            // bruteforces a site
        }

        private void btn4_click(object sender, RoutedEventArgs e)
        {
            // anyrequest
            // you can set every header and send a request
        }
    }
}
