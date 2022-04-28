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
        bool closing = false;
        Screen screen = Screen.PrimaryScreen;
        Label lbl;

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
                bool reset = false;

                string txt = loggedKey.ToString();
                if (e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown | loggedKey.ToString().ToLower() == "rmenu" | loggedKey.ToString().ToLower() == "lmenu")
                {
                    txt =chkAlt.Checked? "Alt":"";
                }
                if (loggedVkCode >= 65 & loggedVkCode <= 90)
                {
                    char c = (char)loggedVkCode;
                    if (!chkCombined.Checked)
                    {
                        txt = c.ToString();
                    } else
                    {
                        if (lastfrm != null)
                        {
                            Label lbl = (Label)lastfrm.Controls.Find("lblKeys", true).FirstOrDefault();
                            if (lbl == null)
                            {
                                txt = "";
                            }
                            else
                            {
                                if (lbl.Text.ToLower().StartsWith("ctrl") | lbl.Text.ToLower().StartsWith("❖") | lbl.Text.ToLower().StartsWith("alt"))
                                {
                                    txt = c.ToString();
                                }
                                else
                                {
                                    txt = "";
                                }
                            }
                        }
                        else
                        {
                            txt = "";
                        }
                    }
                } else
                {   //http://www.911fonts.com/font/download_KeystrokesMTRegular_5831.htm
                    if (loggedKey.ToString().ToLower()=="lshiftkey" | loggedKey.ToString().ToLower() == "rshiftkey")
                    {
                        txt = chkShift.Checked? "Shift":"" ;
                    }
                    if (loggedKey.ToString().ToLower() == "lcontrolkey" | loggedKey.ToString().ToLower() == "rcontrolkey")
                    {
                        txt =chkCTRL.Checked? "CTRL":"";
                    }
                    if (loggedKey.ToString().ToLower() == "lwin")
                    {
                        txt =chkWin.Checked? "❖":"";
                    }
                    if (loggedKey.ToString().ToLower() == "return")
                    {
                        txt = chkReturn.Checked? "Enter":"";
                    }
                    if (loggedKey.ToString().ToLower() == "space")
                    {
                        if (lastfrm != null)
                        {
                            Label lbl = (Label)lastfrm.Controls.Find("lblKeys", true).FirstOrDefault();
                            if (lbl == null)
                            {
                                txt = "";
                            }
                            else
                            {
                                if (lbl.Text.ToLower().StartsWith("ctrl") | lbl.Text.ToLower().StartsWith("❖") | lbl.Text.ToLower().StartsWith("alt"))
                                {
                                    txt = "⎵";
                                }
                                else
                                {
                                    txt = "";
                                }
                            }
                        }
                        else
                        {
                            txt = "";
                        }
                    
                    }
                    if ("home,end,insert,delete,pageup,next,pause,scroll,printscreen,mediaplayerpause,volumemute,volumeup,volumedown,esc".Contains(loggedKey.ToString().ToLower()))
                    {
                        reset = true;
                        Debug.WriteLine("Reset");
                        //txt = "Esc";
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
                        txt = chkArrows.Checked ? "←":"";
                    }
                    if (loggedKey.ToString().ToLower() == "right")
                    {
                        txt = chkArrows.Checked ? "→":"";
                    }
                    if (loggedKey.ToString().ToLower() == "up")
                    {
                        txt =chkArrows.Checked? "↑":"";
                    }
                    if (loggedKey.ToString().ToLower() == "down")
                    {
                        txt = chkArrows.Checked ? "↓":"";
                    }
                    if (loggedKey.ToString().ToLower() == "back")
                    {
                        if (chkBack.Checked)
                        {
                            txt = "Back";
                            reset = true;
                        } else
                        {
                            txt = "";
                        }
                    
                    }
                    if (loggedKey.ToString().Length==2 & loggedKey.ToString().StartsWith("D"))
                    {
                        txt =chkNum.Checked? loggedKey.ToString().Substring(1):"";
                    }
                    if (loggedKey.ToString().StartsWith("Oem"))
                    {
                        txt =chkOEM.Checked? loggedKey.ToString().Replace("Oem", ""):"";
                    }

                }

                if (!clock.IsRunning)
                {
                    clock.Start();
                }
                double elapsed = clock.Elapsed.TotalMilliseconds;

                if (elapsed > 1000 | lastfrm == null | reset | (lbl!=null && "home,end,insert,delete,pageup,next,pause,scroll,printscreen,mediaplayerpause,volumemute,volumeup,volumedown,esc".Contains(lbl.Text.ToLower())))
                {
                    Debug.WriteLine("I am in first condition {elapsed >1000 | lastfm = null | reset}");
                    clock.Restart();
                    frmPopup frm = new frmPopup();
                    lastfrm = frm;
                    lbl = (Label)frm.Controls.Find("lblKeys", true).FirstOrDefault();
                    lbl.Text = txt;
                    Debug.WriteLine($"lbl.text.length = {lbl.Text.Trim().Length}");
                    if (lbl.Text.Trim().Length == 0)
                    {
                        //frm.Visible = false;
                    }else 
                    {
                        frm.Show();
                        frm.Left = screen.WorkingArea.Left;
                        frm.Height = lbl.Height;
                        frm.Top = screen.Bounds.Height - frm.Height - 100;
                        frm.Width = lbl.Width;
                    }
                }
                else
                {
                    Debug.WriteLine("I am in 2nd condition not{elapsed >1000 | lastfm = null | reset}");
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
                    if (txt != lbl.Text & !lbl.Text.Contains(txt + " + "))
                    {
                        lbl.Text = lbl.Text + (combined ? " + " : "") + txt;
                        frm.Show();
                        if (lbl.Text.Trim().Length == 0)
                        {
                            //frm.Visible = false;
                        }
                        else {
                            frm.Show();
                            frm.Left = screen.WorkingArea.Left;
                            frm.Height = lbl.Height;
                            frm.Top = screen.Bounds.Height - frm.Height - 100;
                            frm.Width = lbl.Width;
                        }
                    }
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
            this.Text = Application.ProductName + " - " + Application.ProductVersion;
            chkCombined.Checked = Properties.Settings.Default.onlyCombined;
            chkBack.Checked = Properties.Settings.Default.Back;
            chkReturn.Checked = Properties.Settings.Default.Return;
            chkArrows.Checked = Properties.Settings.Default.Arrows;
            chkShift.Checked = Properties.Settings.Default.Shift;
            chkCTRL.Checked = Properties.Settings.Default.Ctrl;
            chkAlt.Checked = Properties.Settings.Default.Alt;
            chkWin.Checked = Properties.Settings.Default.Win;
            chkOEM.Checked = Properties.Settings.Default.OEM;
            chkNum.Checked = Properties.Settings.Default.Num;

            if (Screen.AllScreens.Count() > 1)
            {
                cmbDisplay.Enabled = true;
                var index = 0;
                foreach (var disp in Screen.AllScreens)
                {
                    cmbDisplay.Items.Add(disp.DeviceName.Replace(".","").Replace("\\","").Replace("/",""));// + "|" + disp.WorkingArea.Left + "," + disp.WorkingArea.Top + "," + disp.WorkingArea.Width + "," + disp.WorkingArea.Height);
                    if (index== Properties.Settings.Default.Monitor)
                    {
                        screen = disp;
                    }
                    index++;
                }
                cmbDisplay.SelectedIndex = Properties.Settings.Default.Monitor;
            }
            else
            {
                cmbDisplay.Enabled = false; 
            }
           
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closing = true;
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("www.arahimi.ca");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closing)
            {
                e.Cancel = true;
                this.Hide();
                ni1.BalloonTipText = Application.ProductName + " is still running in the background. To exit, open the tray icon menu and click on Exit.";
                ni1.ShowBalloonTip(1000);
            }
        }

        private void ni1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void chkCombined_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.onlyCombined = chkCombined.Checked;
            Properties.Settings.Default.Save();
        }

        private void cmbDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Monitor = cmbDisplay.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void chkBack_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Back = chkBack.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkReturn_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Return = chkReturn.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkArrows_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Arrows = chkArrows.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkShift_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Shift = chkShift.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkCTRL_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Ctrl = chkCTRL.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkAlt_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Alt = chkAlt.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkWin_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Win = chkWin.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkOEM_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OEM = chkOEM.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkNum_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Num = chkNum.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
