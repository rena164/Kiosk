using System;
using System.Collections;
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
    public partial class Form4 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSURCQ7\SQLEXPRESS;Initial Catalog=Kiosk;Integrated Security=True;");
        SqlCommand cmd;
        SqlDataReader reader;

        int panelCorderTop = -370;
        int flowLayoutPanelLeft = -240;
        int panel3Top = 770;

        public Form4()
        {
            InitializeComponent();

            label1.Hide();

            timer1.Interval = 5;
            timer1.Tick += Move1;
            timer1.Start();

            timer2.Interval = 1;
            timer2.Tick += Move2;
            timer2.Start();

            timer3.Interval = 1;
            timer3.Tick += Move3;
            timer3.Start();

            scrollMenu.AutoScroll = false;
        }

        private void Move1(Object sender, EventArgs e)
        {
            panelCorderTop += 12;
            panelCorner.Top = panelCorderTop;

            if (panelCorderTop >= 96)
            {
                timer1.Enabled = false;
            }
        }
        private void Move2(Object sender, EventArgs e)
        {
            flowLayoutPanelLeft += 16;
            scrollMenu.Left = flowLayoutPanelLeft;

            if (flowLayoutPanelLeft >= 141)
            {
                label1.Show();
                timer2.Enabled = false;
                scrollMenu.AutoScroll = true;
            }
        }

        private void Move3(Object sender, EventArgs e)
        {
            panel3Top -= 2;
            panel3.Top = panel3Top;

            if (panel3Top <= 734)
            {
                timer3.Enabled = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            form1.Location = this.Location;
            this.Hide();
        }


        private void btnApplePie_Click(object sender, EventArgs e)
        {
            AddOrder("Apple Pie", 149);
        }

        private void btnCroissant_Click(object sender, EventArgs e)
        {
            AddOrder("Croissant", 119);
        }

        private void AddOrder(string item, int price)
        {
            AddOrderFrm addOrderFrm = new AddOrderFrm();
            addOrderFrm.item = item;
            addOrderFrm.price = price;
            addOrderFrm.Show();
            addOrderFrm.Location = this.Location;
            this.Hide();
        }



        private void Form4_Load(object sender, EventArgs e)
        {
            conn.Open();
            string query = "SELECT * FROM StockInfo ORDER BY ID";
            cmd = new SqlCommand(query, conn);
            reader = cmd.ExecuteReader();

            if(reader.HasRows)
            {
                int totalOrder = 0;
                int item = 0;
                while (reader.Read())
                {
                    totalOrder += Convert.ToInt32(reader["Price"]);
                    item += Convert.ToInt32(reader["Quantity"]);
                }
                lbEmpty.Left = 10;
                lbEmpty.Text = $"Total Order: {totalOrder}  |  ITEM: {item}";
                button10.Enabled = true;
            }
            else
            {
                lbEmpty.Text = "Your order is Empty";
                button10.Enabled = false;
            }
            conn.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReviewOrderFrm reviewOrderFrm = new ReviewOrderFrm();
            reviewOrderFrm.Show();
            reviewOrderFrm.Location = this.Location;
            this.Hide();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddOrder("Chocolate Cake", 299);
        }

        private void btnEmpy1_Click(object sender, EventArgs e)
        {
            AddOrder("Cuisine", 249);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddOrder("Sugar Cake", 299);
        }
    }
}
