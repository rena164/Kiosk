using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Kiosk
{
    public partial class AddOrderFrm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSURCQ7\SQLEXPRESS;Initial Catalog=Kiosk;Integrated Security=True;");
        SqlCommand cmd;

        public static List<string> nameList = new List<string>();
        public static List<int> quantityList = new List<int>();
        public static List<int> totalPrice = new List<int>();

        public string item = "";
        public int price = 0;
        private int quantity = 1;

        int pbContainerLeft = -80;
        int panel3Top = 770;
        int panel2Left = 250;

        public AddOrderFrm()
        {
            InitializeComponent();

            lbName.Hide();
            panel2.Hide();
            panel3.Hide();

            timer1.Interval = 15;
            timer1.Tick += Move1;
            timer1.Start();

        }

        private void AddOrderFrm_Load(object sender, EventArgs e)
        {
            switch(item)
            {
                case "Apple Pie":
                    pbContainer.Image = Properties.Resources.Apple_Pie_180;
                    lbName.Text = item + "\n₱" + price.ToString();  
                    break;
                case "Croissant":
                    pbContainer.Image = Properties.Resources.Pastry1_180_;
                    lbName.Text = item + "\n₱" + price.ToString();
                    break;
                case "Chocolate Cake":
                    pbContainer.Image = Properties.Resources.Cake1_180;
                    lbName.Text = item + "\n₱" + price.ToString();
                    break;
                case "Cuisine":
                    pbContainer.Image = Properties.Resources.Pastry2_180;
                    lbName.Text = item + "\n₱" + price.ToString();
                    break; 
                case "Sugar Cake":
                    pbContainer.Image = Properties.Resources.Sugar_Cake_1801;
                    lbName.Text = item + "\n₱" + price.ToString();
                    break; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (quantity > 1)
            {
                quantity--;
                lbNumber.Text = quantity.ToString();

                if (quantity == 1)
                {
                    button1.Enabled = false;
                }
            }
        } //-

        private void button2_Click(object sender, EventArgs e)
        {
            quantity++;
            lbNumber.Text = quantity.ToString();

            if (quantity > 1)
            {
                button1.Enabled = true;
            }
        } //+

        private void button9_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            form4.Location = this.Location;
            this.Hide();
        } 

        private void button10_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = @"DECLARE @ItemName VARCHAR(100) = @Item;
                             DECLARE @Quantity INT = @Qty;
                             DECLARE @Price INT = @P;

                             IF EXISTS (SELECT 1 FROM StockInfo WHERE Item = @ItemName)
                             BEGIN
                                UPDATE StockInfo
                                SET Price = Price + @Price,
                                Quantity = Quantity + @Quantity
                                WHERE Item = @ItemName;
                             END
                             ELSE
                             BEGIN
                                INSERT INTO StockInfo (Item, Price, Quantity)
                                VALUES (@ItemName, @Price, @Quantity);
                             END";

            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Item", item);
            cmd.Parameters.AddWithValue("@Qty", quantity);
            cmd.Parameters.AddWithValue("@P", price * quantity);
            cmd.ExecuteNonQuery();
            conn.Close();

            Form4 form4 = new Form4();
            form4.Show();
            form4.Location = this.Location;
            this.Hide();
        } 

        private void Move1(Object sender, EventArgs e)
        {
            pbContainerLeft += 5;
            pbContainer.Left = pbContainerLeft;

            if(pbContainerLeft >= 13)
            {
                timer1.Enabled = false;
                lbName.Show();

                timer3.Interval = 15;
                timer3.Tick += Move3;
                timer3.Start();

                panel2.Show();
            }
        }

        private void Move2(Object sender, EventArgs e)
        {
            panel3Top -= 2;
            panel3.Top = panel3Top;

            if (panel3Top <= 734)
            {
                timer2.Enabled = false;
            }
        }

        private void Move3(Object sender, EventArgs e)
        {
            panel2Left -= 5;
            panel2.Left = panel2Left;
            
            if(panel2Left <= 200)
            {
                timer3.Enabled = false;

                timer2.Interval = 15;
                timer2.Tick += Move2;
                timer2.Start();

                panel3.Show();
            }

        }

    }
}
