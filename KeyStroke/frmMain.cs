using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyStroke
{
    public partial class frmMain : Form
    {
        Stopwatch clock = new Stopwatch();
        frmPopup lastfrm = null;
        public frmMain()
        {
            InitializeComponent();
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            // EDT: No need to filter for VkSnapshot anymore. This now gets handled
            // through the constructor of GlobalKeyboardHook(...).
            //if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown &&
            //    e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
            //{
            //    MessageBox.Show("Alt + Print Screen");
            //    e.Handled = true;
            //}

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown | e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown)
            {
                // Now you can access both, the key and virtual code
                Keys loggedKey = e.KeyboardData.Key;
                int loggedVkCode = e.KeyboardData.VirtualCode;
                Debug.WriteLine(loggedKey + " | " + loggedVkCode + " | ");

                string txt = loggedKey.ToString();
                if (e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown | loggedKey.ToString().ToLower() == "rmenu" | loggedKey.ToString().ToLower() == "lmenu")
                {
                    txt = "Alt";
                }
                if (loggedVkCode >= 65 & loggedVkCode <= 90)
                {
                    char c = (char)loggedVkCode;
                    txt = c.ToString();
                } else
                {   //http://www.911fonts.com/font/download_KeystrokesMTRegular_5831.htm
                    if (loggedKey.ToString().ToLower()=="lshiftkey" | loggedKey.ToString().ToLower() == "rshiftkey")
                    {
                        txt = "Shift" ;
                    }
                    if (loggedKey.ToString().ToLower() == "lcontrolkey" | loggedKey.ToString().ToLower() == "rcontrolkey")
                    {
                        txt = "CTRL";
                    }
                    if (loggedKey.ToString().ToLower() == "lwin")
                    {
                        txt = "❖";
                    }
                    if (loggedKey.ToString().ToLower() == "return")
                    {
                        txt = "Enter";
                    }
                    if (loggedKey.ToString().ToLower() == "space")
                    {
                        txt = " ";
                    }
                    if (loggedKey.ToString().ToLower() == "escape")
                    {
                        txt = "Esc";
                    }
                    if (loggedKey.ToString().ToLower() == "capital")
                    {
                        txt = "Caps Lock";
                    }
                    if (loggedKey.ToString().ToLower() == "left")
                    {
                        txt = "←";
                    }
                    if (loggedKey.ToString().ToLower() == "right")
                    {
                        txt = "→";
                    }
                    if (loggedKey.ToString().ToLower() == "up")
                    {
                        txt = "↑";
                    }
                    if (loggedKey.ToString().ToLower() == "down")
                    {
                        txt = "↓";
                    }
                    if (loggedKey.ToString().Length==2 & loggedKey.ToString().StartsWith("D"))
                    {
                        txt = loggedKey.ToString().Substring(1);
                    }
                    if (loggedKey.ToString().StartsWith("Oem"))
                    {
                        txt = loggedKey.ToString().Replace("Oem", "");
                    }

                }

                if (!clock.IsRunning)
                {
                    clock.Start();
                }
                double elapsed = clock.Elapsed.TotalMilliseconds;

                if (elapsed>1000 | lastfrm == null)
                {
                    clock.Restart();
                    frmPopup frm = new frmPopup();
                    lastfrm = frm;
                    Label lbl = (Label)frm.Controls.Find("lblKeys", true).FirstOrDefault();
                    lbl.Text = txt;
                    frm.Show();
                    frm.Left = 0;
                    frm.Height = lbl.Height;
                    frm.Top = Screen.PrimaryScreen.Bounds.Height - frm.Height - 100;
                    frm.Width = lbl.Width;
                } else
                {
                    clock.Restart();
                    frmPopup frm = lastfrm;
                    Label lbl = (Label)frm.Controls.Find("lblKeys", true).FirstOrDefault();
                    if (lbl == null)
                    {
                        return;
                    }
                    bool combined = false;
                    if (lbl.Text.ToLower().StartsWith("shift") | lbl.Text.ToLower().StartsWith("ctrl") | lbl.Text.ToLower().StartsWith("❖") | lbl.Text.ToLower().StartsWith("alt"))
                        {
                        combined = true;
                    }
                    lbl.Text = lbl.Text + (combined? " + ":"") + txt;
                    frm.Show();
                    frm.Left = 0;
                    frm.Height = lbl.Height;
                    frm.Top = Screen.PrimaryScreen.Bounds.Height - frm.Height - 100;
                    frm.Width = lbl.Width;
                }

            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hooks only into specified Keys (here "A" and "B").
            //_globalKeyboardHook = new GlobalKeyboardHook(new Keys[] { Keys.A, Keys.B });

            // Hooks into all keys.
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
            this.Visible = false;
            ni1.Visible = true;
            this.ShowInTaskbar = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
