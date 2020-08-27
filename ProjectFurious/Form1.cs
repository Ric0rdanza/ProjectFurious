using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Timers;

namespace ProjectFurious
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private string[] Messages;
        private string[] Views;
        private string[] Urls;

        private int controller;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        private void CallPython()
        {
            //运行python脚本，获取热搜榜
            Process p = new Process();
            string Argument = "WeiboHot.py";
            p.StartInfo.FileName = "python.exe";
            p.StartInfo.Arguments = Argument;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.Close();
        }
        private void GetJson()
        {
            //读取热搜榜
            string JsonFile = "./WeiboHot.json";
            using (System.IO.StreamReader file = System.IO.File.OpenText(JsonFile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JArray o = (JArray)JToken.ReadFrom(reader);
                    this.Messages = new string[50];
                    this.Views = new string[50];
                    this.Urls = new string[50];
                    for (int i = 0; i < 50; i++)
                    {
                        Messages[i] = o[0][i].ToString();
                        Views[i] = o[1][i].ToString();
                        Urls[i] = o[2][i].ToString();
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //滚动字幕的容器
            this.panel1.Controls.Add(this.label1);
            this.label1.Left = this.panel1.Right;
            this.controller = 0;

            this.CallPython();
            this.GetJson();
            
            this.label1.Text = Messages[this.controller];
            this.timer1.Interval = 1;
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //字幕滚动
            this.label1.Left -= 3;

            if (this.label1.Right <= this.panel1.Left)
            {
                this.label1.Left = this.panel1.Right;
                this.controller++;
                if(this.controller == 50)
                {
                    this.timer1.Stop();
                    this.controller = 0;
                    TimerStopped();
                }
                else
                {
                    this.label1.Text = Messages[this.controller];
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Detail Detail = new Detail(Messages, Views, Urls);
            Detail.ShowDialog();
        }
        private void TimerStopped()
        {
            //过timer2.Interval时间再次获取热搜榜，单位是毫秒
            this.timer2.Interval = 60000;
            this.timer2.Start();
        }
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            this.label1.Left = this.panel1.Right;
            CallPython();
            GetJson();
            this.label1.Text = Messages[this.controller];
            this.timer1.Start();
            this.timer2.Stop();
        }
    }
}
