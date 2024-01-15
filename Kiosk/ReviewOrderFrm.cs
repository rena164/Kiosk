using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kiosk
{
    public partial class ReviewOrderFrm : Form
    { 
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSURCQ7\SQLEXPRESS;Initial Catalog=Kiosk;Integrated Security=True;");
        SqlCommand cmd;
        SqlDataReader reader;

        private int index = 0;
        private int time = 0;
        private int panel3Top = 800;

        public ReviewOrderFrm()
        {
            InitializeComponent();

            int totalOrder = 0;

            conn.Open();
            string query = "SELECT * FROM StockInfo ORDER BY ID";
            cmd = new SqlCommand(query, conn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Panel
                Panel panel = new Panel();
                panel.Size = new Size(490, 80);
                panel.Margin = new Padding(5, 5, 5, 5);
                panel.BackColor = Color.White;

                // Name of item
                Label label1 = new Label();
                label1.BackColor = Color.White;
                label1.Text = reader["Item"].ToString();
                Font fontt = new Font("Cambria", 12);
                label1.Font = new Font(fontt, FontStyle.Bold);
                label1.AutoSize = true;
                label1.Location = new Point(35, 5);

                // Quantity of item
                Label label2 = new Label();
                label2.BackColor = Color.White;
                label2.Text = reader["Quantity"].ToString() + "X";
                label2.Font = new Font(fontt, FontStyle.Bold);
                label2.AutoSize = true;
                label2.Location = new Point(5, 5);

                // Total price of each cart
                Label label3 = new Label();
                label3.BackColor = Color.White;
                label3.Text = "₱" + reader["Price"].ToString();
                label3.Font = new Font(fontt, FontStyle.Bold);
                label3.AutoSize = true;
                label3.Location = new Point(445, 5);

                // Line
                Label line = new Label();
                line.BackColor = Color.White;
                line.Text = "_________________________________________________________________________________";
                line.AutoSize = true;
                line.Location = new Point(0, 20);

                // Remove button 
                Button button = new Button();
                button.Name = reader["ID"].ToString();
                button.BackColor = Color.ForestGreen;
                button.FlatStyle = FlatStyle.Flat;
                button.Size = new Size(120, 30);
                button.Text = "Remove";
                button.ForeColor = Color.White;
                Font font = new Font("Cambria", 12);
                button.Font = new Font(font, FontStyle.Bold);
                button.AutoSize = true;
                button.Location = new Point(5, 40);
                button.Parent = panel;
                button.Click += Global_Button_Click;


                //
                PictureBox backgroundLine = new PictureBox();
                backgroundLine.Image = Properties.Resources.LineForAddRemoveBtn;
                backgroundLine.Location = new Point(222, 36);
                backgroundLine.SendToBack();

                // Add quantity button
                Button buttonAdd = new Button();
                buttonAdd.Name = reader["ID"].ToString();
                buttonAdd.BackColor = Color.Red;
                buttonAdd.FlatStyle = FlatStyle.Flat;
                buttonAdd.Size = new Size(31, 30);
                buttonAdd.Text = "+";
                buttonAdd.ForeColor = Color.White;
                Font newoFont = new Font("Cambria", 14);
                buttonAdd.Font = new Font(newoFont, FontStyle.Bold);
                buttonAdd.Location = new Point(267, 40);
                buttonAdd.Parent = panel;
                buttonAdd.FlatAppearance.BorderSize = 0;
                buttonAdd.Click += Global_Button_Click_Add;

                Label lblQuantity = new Label();
                lblQuantity.BackColor = Color.White;
                lblQuantity.Text = reader["Quantity"].ToString();
                lblQuantity.Font = new Font(fontt, FontStyle.Bold);
                lblQuantity.AutoSize = true;
                lblQuantity.Location = new Point(241, 46);

                // Remove quantity button
                Button buttonRemove = new Button();
                buttonRemove.Name = reader["ID"].ToString();
                buttonRemove.BackColor = Color.Red;
                buttonRemove.FlatStyle = FlatStyle.Flat;
                buttonRemove.Size = new Size(31, 30);
                buttonRemove.Image = Properties.Resources.RemoveSign;
                buttonRemove.ForeColor = Color.White;
                Font removeFont = new Font("Cambria", 18);
                buttonRemove.Font = new Font(removeFont, FontStyle.Bold);
                buttonRemove.Location = new Point(200, 40);
                buttonRemove.Parent = panel;     
                buttonRemove.FlatAppearance.BorderSize = 0;
                buttonRemove.Click += Global_Button_Click_Remove;
                if (Convert.ToInt32(reader["Quantity"]) == 1)
                {
                    buttonRemove.Enabled = false;
                } 
                else
                {
                    buttonRemove.Enabled = true;
                }

                // Add to Controls
                panel.Controls.Add(label1);
                panel.Controls.Add(label2);
                panel.Controls.Add(label3);
                panel.Controls.Add(line);
                panel.Controls.Add(button);
                panel.Controls.Add(backgroundLine);
                panel.Controls.Add(buttonAdd);
                panel.Controls.Add(lblQuantity);
                panel.Controls.Add(buttonRemove);

                buttonRemove.BringToFront();
                lblQuantity.BringToFront();
                buttonAdd.BringToFront();
                flowLayoutPanel1.Controls.Add(panel);

                // Compute the total order            
                totalOrder += Convert.ToInt32(reader["Price"]);
            }

            conn.Close();

            lbTotal.Text = "Total: ₱" + totalOrder.ToString();

            timer2.Interval = 1;
            timer2.Tick += Move2;
            timer2.Start();

            label1.Hide();
        }

        private void Global_Button_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "DELETE FROM StockInfo WHERE ID = @id";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", ((Button)sender).Name);
            cmd.ExecuteNonQuery();
            conn.Close();

            flowLayoutPanel1.Controls.Remove(((Button)sender).Parent);


            int totalOrder = 0;
            conn.Open();
            cmd = new SqlCommand("SELECT * FROM StockInfo ORDER BY ID", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                totalOrder += Convert.ToInt32(reader["Price"]);
            }
            conn.Close();

            lbTotal.Text = "Total: ₱" + totalOrder.ToString();


            conn.Open();
            cmd = new SqlCommand("SELECT * FROM StockInfo", conn);
            reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Form4 form4 = new Form4();
                form4.Show();
                form4.Location = this.Location;
                this.Hide();
            }
            conn.Close();
        }

        private void Global_Button_Click_Add(object sender, EventArgs e)
        {
            conn.Open();
            string query = @"DECLARE @UpdateQty INT = (SELECT Quantity FROM StockInfo WHERE ID = @id);
                            UPDATE StockInfo SET Quantity = @UpdateQty + 1, Price = Price + (Price / Quantity)  WHERE ID = @id;";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", ((Button)sender).Name);
            cmd.ExecuteNonQuery();
            conn.Close();

            ReviewOrderFrm reviewOrderFrm = new ReviewOrderFrm();
            reviewOrderFrm.Show();
            reviewOrderFrm.Location = this.Location;
            this.Close();
        }

        private void Global_Button_Click_Remove(object sender, EventArgs e)
        {
            conn.Open();
            string query = @"DECLARE @UpdateQty INT = (SELECT Quantity FROM StockInfo WHERE ID = @id);
                            UPDATE StockInfo SET Quantity = @UpdateQty - 1, Price = Price - (Price / Quantity)  WHERE ID = @id;";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", ((Button)sender).Name);
            cmd.ExecuteNonQuery();
            conn.Close();

            ReviewOrderFrm reviewOrderFrm = new ReviewOrderFrm();
            reviewOrderFrm.Show();
            reviewOrderFrm.Location = this.Location;
            this.Close();
        }

        private void Move2(Object sender, EventArgs e)
        {
            panel3Top -= 3;
            panel3.Top = panel3Top;

            if (panel3Top <= 734)
            {
                timer2.Enabled = false;
                label1.Show();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            form4.Location = this.Location;
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            printDocument1.PrinterSettings.PrintToFile = true;
            printDocument1.PrinterSettings.PrintFileName = @"C:\Users\rena0\Pictures\Reciept\receipt.pdf";
            printDocument1.Print();

            NumberOrderFrm n = new NumberOrderFrm();
            n.Show();
            n.Location = this.Location;
            this.Hide();

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            string line1 = "----------------------------------------";
            e.Graphics.DrawString(line1, new Font("Cambria", 38, FontStyle.Regular), Brushes.Black, new Point(30, 50));

            string label1 = "Your order number is";
            e.Graphics.DrawString(label1, new Font("Cambria", 26, FontStyle.Regular), Brushes.Black, new Point(225, 110));

            string label2 = "001";
            e.Graphics.DrawString(label2, new Font("Cambria", 72, FontStyle.Regular), Brushes.Black, new Point(320, 160));

            string line2 = "----------------------------------------";
            e.Graphics.DrawString(line2, new Font("Cambria", 38, FontStyle.Regular), Brushes.Black, new Point(30, 260));
        }
    }
}
