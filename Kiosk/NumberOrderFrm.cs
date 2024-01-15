using System;
using System.Collections;
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
    public partial class NumberOrderFrm : Form
    {
        private int size = 8;
        public NumberOrderFrm()
        {
            InitializeComponent();

            QueueControllerFrm.ticketNumebr++;
            QueueControllerFrm.list.Enqueue(QueueControllerFrm.ticketNumebr.ToString());
            QueueControllerFrm.totalQueue++;
            QueueControllerFrm.refreshButton.PerformClick();
        }
        private void NumberOrderFrm_Load(object sender, EventArgs e)
        {      
            timer1.Interval = 5;
            timer1.Tick += Move;
            timer1.Start();

            timer2.Interval = 10000;
            timer2.Tick += CallForm1;
            //timer2.Start();
        }

        private void Move(Object sender, EventArgs e)
        {
            size += 1;
            label1.Font = new Font("Cambria", size, FontStyle.Regular);

            if (size >= 22)
            {
                timer1.Enabled = false;
            }

            label1.Text = "Your order number is \n001";
        }

        private void CallForm1(Object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            form1.Location = this.Location;
            this.Hide();

            timer2.Enabled = false;
        }
    }
}
