using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kiosk
{
    public partial class Form2 : Form
    {
        int buttonTop = 300;
        string diningLocation;
        public Form2()
        {
            InitializeComponent();

            timer1.Interval = 1;
            timer1.Tick += Move1;
            timer1.Start();

            label1.Hide();
        }

        private void Move1(Object sender, EventArgs e)
        {

            buttonTop -= 5;

            button1.Top = buttonTop;
            button2.Top = buttonTop;

            if(buttonTop <= 187)
            {
                timer1.Enabled = false;
                label1.Show();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            form4.Location = this.Location;
            this.Hide();
            diningLocation = "Dine-In";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            form4.Location = this.Location;
            this.Hide();
            diningLocation = "Take-Out";
        }


    }
}
