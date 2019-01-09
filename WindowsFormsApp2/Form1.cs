using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Threading;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        static string urltrack = "";
        static string trackname = "";
        public static readonly string DIR_BASE = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string DIR_DL = DIR_BASE + "Downloads";
        bool IsFirstClick = true;
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Start()
        {
            string url = "http://retrowave.ru";
            var parse = APIParse();
            string name = parse["body"]["tracks"][0]["title"].ToString();
            string fixedname = name.Replace("вЂ“", "–");
            string track = parse["body"]["tracks"][0]["streamUrl"].ToString();
            string art = parse["body"]["tracks"][0]["artworkUrl"].ToString();
            wplayer.URL = url + track;
            urltrack = wplayer.URL;
            trackname = fixedname + ".mp3";
            label1.Text = "Now playing: " + fixedname;
            pictureBox1.Load(url + art);
            wplayer.controls.play();
            button1.Text = "Next";
            wplayer.settings.volume = trackBar1.Value;
            timer1.Start();
        }
        public void Stop()
        {
            pictureBox1.Load("http://retrowave.ru/img/share.png");
            button1.Text = "Play";
            label1.Text = "Title";
            wplayer.controls.stop();
        }
        static JObject APIParse()
        {
            Random rnd = new Random();
            string cursor = rnd.Next(0, 399).ToString();
            var s = cursor;
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");
            string jsonResponse = wc.DownloadString("http://retrowave.ru/api/v1/tracks?cursor=" + s + "&limit=1");
            JObject parse = JObject.Parse(jsonResponse);
            return parse;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wplayer.controls.pause();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            wplayer.settings.volume = trackBar1.Value;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double t = Math.Floor(wplayer.controls.currentPosition);
            double d = Math.Floor(wplayer.currentMedia.duration);
            int time = Convert.ToInt32(d);
            int timer = Convert.ToInt32(t);
            progressBar1.Maximum = time;
            progressBar1.Value = timer;
            int sec = timer;
            int minutes = sec / 60;
            int newSec = sec - minutes * 60;
            int hour = minutes / 60;
            int newMinnutes = minutes - hour * 60;
            TimeSpan times = new TimeSpan(hour, newMinnutes, newSec);

            int secs = time;
            int minutess = secs / 60;
            int newSecs = secs - minutess * 60;
            int hours = minutess / 60;
            int newMinnutess = minutess - hours * 60;
            TimeSpan timess = new TimeSpan(hours, newMinnutess, newSecs);
            label2.Text = times.ToString() + "/" + timess;
            string login = label2.Text.Split('/')[0].Trim();
            string password = label2.Text.Split('/')[1].Trim();
            if (password != "00:00:00")
            {
                if (login == password)
                {
                    string url = "http://retrowave.ru";
                    var parse = APIParse();
                    string name = parse["body"]["tracks"][0]["title"].ToString();
                    string fixedname = name.Replace("вЂ“", "–");
                    string track = parse["body"]["tracks"][0]["streamUrl"].ToString();
                    string art = parse["body"]["tracks"][0]["artworkUrl"].ToString();
                    wplayer.URL = url + track;
                    label1.Text = "Now playing: " + fixedname;
                    pictureBox1.Load(url + art);
                    wplayer.controls.play();
                }
                else { }
            }
                if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    label3.Text = "RETROWAVE Radio is playing!";
                }
                else
                {
                    label3.Text = "RETROWAVE Radio is NOT playing!";
                }
        }

        private void progressBar1_Click_1(object sender, EventArgs e)
        {
            double t = Math.Floor(wplayer.controls.currentPosition);
            int timer = Convert.ToInt32(t);
            progressBar1.Value = timer;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                {
                    try
                    {
                        if (!Directory.Exists(DIR_DL))
                        {
                            Directory.CreateDirectory(DIR_DL);

                        }
                        if (label3.Text == "RETROWAVE Radio is NOT playing!")
                        {
                            MessageBox.Show("Нечего сохранять :(");
                        }
                        if (label3.Text == "RETROWAVE Radio is playing!")
                        {
                            WebClient wc = new WebClient();
                            wc.DownloadFile(urltrack, string.Format(DIR_DL + "\\" + trackname, Guid.NewGuid().ToString()));
                            MessageBox.Show("Трек успешно сохранен в " + DIR_DL + "\\" + trackname);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Что-то пошло не так.\nТекст ошибки: " + ex.Message);
                    }
                }
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IsFirstClick) //IsFirstClick = true
            {
                wplayer.controls.pause();
                button4.Text = "Play";
                IsFirstClick = false;
            }
            else // IsFirstClick = false
            {
                wplayer.controls.play();
                button4.Text = "Pause";
                IsFirstClick = true;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
        }
    }
    }
