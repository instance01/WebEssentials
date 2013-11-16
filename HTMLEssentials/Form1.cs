using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HTMLEssentials
{
    public partial class Form1 : Form
    {

        public string[] urls;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*if (urls.Length < 1)
            {
                urls = youtube_playlist.links.ToArray();
                axShockwaveFlash1.Movie = "https://www.youtube.com/v/" + urls[0];
                axShockwaveFlash1.Play();
            }
            else
            {
                axShockwaveFlash1.Movie = "https://www.youtube.com/v/" + urls[0];
                axShockwaveFlash1.Play();
            }*/
           
            
        }


        internal void start(string[] urllist)
        {
            if (urllist.Length < 1)
            {
                MessageBox.Show("sucks");
                urls = youtube_playlist.links.ToArray();

                this.axShockwaveFlash1.Movie = "https://www.youtube.com/v/" + urls[0] + "&autoplay=1";
                this.axShockwaveFlash1.Play();
            }
            else
            {
                this.axShockwaveFlash1.Movie = "https://www.youtube.com/v/" + urllist[0] + "&autoplay=1";
                this.axShockwaveFlash1.Play();
            }
        }
    }
}
