using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace KeyStroke
{

    public partial class frmPopup : Form
    {
        double opac = 1;

        protected override bool ShowWithoutActivation { get { return true; } }

        protected override CreateParams CreateParams
        {
            get
            {
                //make sure Top Most property on form is set to false
                //otherwise this doesn't work
                int WS_EX_TOPMOST = 0x00000008;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOPMOST;
                return cp;
            }
        }
        public frmPopup()
        {
            InitializeComponent();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        private void frmPopup_Load(object sender, EventArgs e)
        {
            int total = 0;
            FormCollection fc = Application.OpenForms;
            foreach (Form frmp in fc)
            {
                if (frmp.Name == "frmPopup")
                {
                    total++;
                    frmp.Top = frmp.Top - frmp.Height - 50;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            opac -= 0.1;
            this.Opacity = opac;
            this.Top -= 3;
            if (opac <= 0)
            {
                this.Close();
            }
        }

        private void lblKeys_Click(object sender, EventArgs e)
        {

        }

        private void lblKeys_SizeChanged(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            using (Pen selPen = new Pen(Color.White))
            {
                g.DrawRectangle(selPen,lblKeys.Left, lblKeys.Top, lblKeys.Width, lblKeys.Height);
            }
        }



    }



}
