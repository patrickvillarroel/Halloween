using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloweenTown
{
    public partial class Scream : Form
    {
        private Timer closeTimer;
        public Scream()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            SoundPlayer background = new SoundPlayer("G:\\Descargas\\screamer.wav");
            background.Play();

            closeTimer = new Timer();
            closeTimer.Interval = 4000;
            closeTimer.Tick += CloseScreamer;
            closeTimer.Start();
        }

        private void CloseScreamer(object sender, EventArgs e)
        {
            closeTimer.Stop();
            this.Close();
        }
    }
}
