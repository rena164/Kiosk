using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Kiosk
{
    public partial class QueueControllerFrm : Form
    {
        SoundPlayer sound = new SoundPlayer("Doorbell.wav");
        SpeechSynthesizer speech = new SpeechSynthesizer();
        public static Queue<string> list = new Queue<string>();

        private bool isClicked = false;
        public static int ticketNumebr;
        public static int totalQueue;
        private string number;

        public static Button refreshButton = new Button();

        public QueueControllerFrm()
        {
            InitializeComponent();

            timer1.Tick += WaitForSpeak;
            timer2.Tick += WaitForClick;

            MonitoringFrm formMonitor = new MonitoringFrm();
            formMonitor.Show();

            refreshButton = btnRefresh;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lblRemaining.Text = "Remaining: " + list.Count().ToString();
            lblAlltickets.Text = "Total    :" + totalQueue.ToString();

            MonitoringFrm.waitingNumber = list.Last().Length > 1 ? "0" + list.Last() : "00" + list.Last();
            MonitoringFrm.myButton.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isClicked)
            {
                isClicked = true;

                sound.Play();
                number = list.Peek().Length > 1 ? "0" + list.Peek() : "00" + list.Peek();
                lblCurrent.Text = number;
                MonitoringFrm.ticketNumber = number;
                MonitoringFrm.buttonRemove.PerformClick();
                list.Dequeue();
                Refresh();

                timer1.Start();
                timer2.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isClicked)
            {
                isClicked = true;

                sound.Play();

                timer1.Start();
                timer2.Start();
            }
        }

        private void WaitForClick(Object sender, EventArgs e)
        {
            isClicked = false;
            timer2.Enabled = false;
        }

        private void WaitForSpeak(Object sender, EventArgs e)
        {
            speech.SpeakAsync("Now serving\n" + number);
            timer1.Enabled = false;
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
