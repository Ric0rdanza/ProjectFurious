using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ProjectFurious
{
    public partial class Detail : Form
    {
        private string[] Messages;
        private string[] Views;
        private string[] Urls;
        public Detail()
        {
            InitializeComponent();
        }
        public Detail(string[] Messages, string[] Views, string[] Urls)
        {
            InitializeComponent();
            this.Messages = Messages;
            this.Views = Views;
            this.Urls = Urls;
        }
        private void Detail_Load(object sender, EventArgs e)
        {
            TableLayoutPanel table = new TableLayoutPanel();
            table.Dock = DockStyle.Top;
            panel1.Controls.Add(table);
            table.ColumnCount = 2;
            table.Height = table.RowCount * 40;

            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, table.Width * 0.80f));
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, table.Width * 0.20f));


            for (int i = 0; i < Messages.Length; i++)
            {
                table.RowCount++;
                table.Height = table.RowCount * 40;
                table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40));
                int p = table.RowCount - 1;

                LinkLabel item = new LinkLabel();
                item.Text = Messages[i];
                item.Links[0].LinkData = "https://s.weibo.com/" + Urls[i];
                item.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ItemClicked);

                item.Width = 633;
                item.Height = 40;
                item.Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular);
                item.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                table.Controls.Add(item, 0, i);

                Label views = new Label();
                views.Text = Views[i];
                views.Height = 40;
                views.Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular);
                views.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                table.Controls.Add(views, 1, i);
            }
        }
        private void ItemClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            //重写LinkLabel.LinkClicked事件
            string target = e.Link.LinkData as string;
            if(target != null)
            {
                System.Diagnostics.Process.Start(target);
            }
        }
    }
}
