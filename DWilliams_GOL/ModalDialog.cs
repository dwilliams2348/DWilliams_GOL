using System;
using System.Windows.Forms;

namespace DWilliams_GOL
{
    public partial class ModalDialog : Form
    {
        //settings to send to main form
        //universe size, time between generations
        public int xSize;
        public int ySize;
        public int genInterval;

        private void ModalDialog_Load(object sender, EventArgs e)
        {
            xUniverse.Text = xSize.ToString();
            yUniverse.Text = ySize.ToString();
            timerInterval.Text = genInterval.ToString();
            this.Text = "Settings Menu";
        }
        public ModalDialog(int x, int y, int genInt)
        {
            InitializeComponent();

            xSize = x;
            ySize = y;
            genInterval = genInt;

            xUniverse.Text = x.ToString();
            yUniverse.Text = y.ToString();
            timerInterval.Text = genInt.ToString();
        }

        private void xUniverse_TextChanged(object sender, EventArgs e)
        {
            if (xUniverse.Text != "")
            {
                xSize = Convert.ToInt32(xUniverse.Text);
            }
        }

        private void yUniverse_TextChanged(object sender, EventArgs e)
        {
            if (yUniverse.Text != "")
            {
                ySize = Convert.ToInt32(yUniverse.Text);
            }
        }

        private void timerInterval_TextChanged(object sender, EventArgs e)
        {
            if (timerInterval.Text != "")
            {
                genInterval = Convert.ToInt32(timerInterval.Text);
            }
        }
    }
}
