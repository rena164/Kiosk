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
    public partial class MonitoringFrm : Form
    {
        public static string ticketNumber = "000";
        public static string waitingNumber;

        public static Button myButton;
        public static Button buttonRemove;

        public MonitoringFrm()
        {
            InitializeComponent();

            timer1.Interval = 1;
            timer1.Tick += UpdateScreen;
            timer1.Start();

            timer2.Interval = 1;
            timer2.Tick += MoveTheLabel;
            timer2.Start();

            myButton = button1;
            buttonRemove = button2;
        }

        private void UpdateScreen(Object sender, EventArgs e)
        {
            label1.Text = ticketNumber;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Panel panel = new Panel();
            panel.BackColor = Color.FromArgb(41, 39, 40);
            panel.Size = new Size(420, 57);

            Label label = new Label();
            label.Text = waitingNumber;
            label.ForeColor = Color.Red;
            label.Font = new Font("Cambria", 20);
            label.Location = new Point(190, 15);
            label.AutoSize = true;

            panel.Controls.Add(label);
            flowLayoutPanel1.Controls.Add(panel);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.RemoveAt(0);
        }

        private int lblQueueSmartLeft = 1300;

        private void MoveTheLabel(object sender, EventArgs e)
        {
            lblQueueSmartLeft -= 1;
            lblQueueSmart.Left = lblQueueSmartLeft;
            if (lblQueueSmartLeft <= 30)
            {
                lblQueueSmartLeft = 1300;
            }
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

    }
}
