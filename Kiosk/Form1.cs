using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kiosk
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSURCQ7\SQLEXPRESS;Initial Catalog=Kiosk;Integrated Security=True;");
        SqlCommand cmd;

        int buttonTop = 580;
        int labelLeft = 170;
        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1;
            timer1.Tick += Move1;
            timer1.Start();
            label1.Hide();

            conn.Open();
            string query = "DBCC CHECKIDENT (StockInfo, RESEED, 0); DELETE FROM StockInfo;";
            cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void Move1(Object sender, EventArgs e)
        {
            buttonTop -= 5;
            button1.Top = buttonTop;

            if(buttonTop <= 511)
            {
                timer1.Enabled = false;
                label1.Show();
            }
        }

        private void Move2(Object sender, EventArgs e)
        {
            labelLeft -= 2;
            label1.Left = labelLeft;

            if(labelLeft <= 126)
            {
                button1.Show();
                timer2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            form2.Location = this.Location;
            this.Hide();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
