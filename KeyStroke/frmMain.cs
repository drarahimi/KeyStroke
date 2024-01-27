using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        ComboBox cmbDisplay;
        // Dictionary to store checkbox values
        private Dictionary<string, bool> checkboxValues = new Dictionary<string, bool>();

        public frmMain()
        {
            InitializeComponent();
            loadComponents();
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();

        public static float GetMagnificationScale()
        {
            // Set process as DPI-aware
            SetProcessDPIAware();

            // Get the screen DPI
            IntPtr hdc = GetDC(IntPtr.Zero);
            int dpi = GetDeviceCaps(hdc, 88); // 88 corresponds to LOGPIXELSX
            ReleaseDC(IntPtr.Zero, hdc);

            // Calculate the scale based on the default DPI (96)
            float scale = dpi / 96f;

            return scale;
        }

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

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
                    txt =checkboxValues["chkAlt"]? "Alt":"";
                }
                if (loggedVkCode >= 65 & loggedVkCode <= 90)
                {
                    char c = (char)loggedVkCode;
                    if (!checkboxValues["chkCombined"])
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
                        txt = checkboxValues["chkShift"]? "Shift":"" ;
                    }
                    if (loggedKey.ToString().ToLower() == "lcontrolkey" | loggedKey.ToString().ToLower() == "rcontrolkey")
                    {
                        txt = checkboxValues["chkCTRL"]? "CTRL":"";
                    }
                    if (loggedKey.ToString().ToLower() == "lwin")
                    {
                        txt = checkboxValues["chkWin"]? "❖":"";
                    }
                    if (loggedKey.ToString().ToLower() == "return")
                    {
                        txt = checkboxValues["chkReturn"]? "Enter":"";
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
                        txt = checkboxValues["chkArrows"]? "←":"";
                    }
                    if (loggedKey.ToString().ToLower() == "right")
                    {
                        txt = checkboxValues["chkArrows"]? "→":"";
                    }
                    if (loggedKey.ToString().ToLower() == "up")
                    {
                        txt = checkboxValues["chkArrows"]? "↑":"";
                    }
                    if (loggedKey.ToString().ToLower() == "down")
                    {
                        txt = checkboxValues["chkArrows"]? "↓":"";
                    }
                    if (loggedKey.ToString().ToLower() == "back")
                    {
                        if (checkboxValues["chkBack"])
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
                        txt = checkboxValues["chkNum"]? loggedKey.ToString().Substring(1):"";
                    }
                    if (loggedKey.ToString().StartsWith("Oem"))
                    {
                        txt = checkboxValues["chkOEM"]? loggedKey.ToString().Replace("Oem", ""):"";
                    }

                }

                if (!clock.IsRunning)
                {
                    clock.Start();
                }
                double elapsed = clock.Elapsed.TotalMilliseconds;

                //if (elapsed > 1000 | lastfrm == null | reset | ("home,end,insert,delete,pageup,next,pause,scroll,printscreen,mediaplayerpause,volumemute,volumeup,volumedown,esc".Contains(lbl.Text.ToLower())))
                    if (elapsed > 1000 | lastfrm == null | reset )
                    {
                        Debug.WriteLine("I am in first condition {elapsed >1000 | lastfm = null | reset}");
                    clock.Restart();
                    frmPopup frm = new frmPopup();
                    lastfrm = frm;
                    Label lbl = (Label)frm.Controls.Find("lblKeys", true).FirstOrDefault();
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
           
        }

        private void loadComponents()
        {
            TableLayoutPanel tlp1 = new TableLayoutPanel() { Dock = DockStyle.Fill };
            tlp1.ColumnCount = 2;
            tlp1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            tlp1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            this.Controls.Add(tlp1);

            var rowIndex = -1;
            var scale = GetMagnificationScale();
            var rowHeight = 35 * scale;

            rowIndex++;
            tlp1.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            Label lbl = new Label()
            {
                Text = "Prefrences",
                Font = new Font(this.Font, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };
            tlp1.Controls.Add(lbl, 0, rowIndex);

            rowIndex++;
            tlp1.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            lbl = new Label()
            {
                Text = "Display",
                Font = new Font(this.Font, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };
            tlp1.Controls.Add(lbl, 0, rowIndex);

            cmbDisplay = new ComboBox()
            {
                Name = "cmbDisplay",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Sorted = true
            };
            cmbDisplay.SelectedIndexChanged += (sender, e) =>
            {
                Properties.Settings.Default.Monitor = cmbDisplay.SelectedIndex;
                Properties.Settings.Default.Save();
            };
            tlp1.Controls.Add(cmbDisplay, 1, rowIndex);


            List<string> checkboxNames = new List<string>
            {
                "chkCombined",
                "chkBack",
                "chkReturn",
                "chkArrows",
                "chkShift",
                "chkCTRL",
                "chkAlt",
                "chkWin",
                "chkOEM",
                "chkNum"
            };
            List<string> checkboxTexts = new List<string>
            {   "Don't show alphabet unless combined with special keys",
                "Back",
                "Return/Enter",
                "Arrows",
                "Shift",
                "CTRL",
                "Alt",
                "Windows Key",
                "OEM ({ } \\ ; ' ...)",
                "Numbers (1, 2, 3, ...)"
            };

            if (checkboxNames.Count != checkboxTexts.Count)
            {
                throw new ArgumentException("The number of checkbox names and texts should be the same.");
            }

            for (int i = 0; i < checkboxNames.Count; i++)
            {
                rowIndex++;
                tlp1.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                CheckBox chk = new CheckBox()
                {
                    Name = checkboxNames[i],
                    Text = checkboxTexts[i],
                    Dock = DockStyle.Fill
                };
                chk.CheckedChanged += CheckBox_CheckedChanged;

                // Load the saved value from Properties.Settings
                try
                {
                    if (Properties.Settings.Default[checkboxNames[i]] != null)
                    {
                        chk.Checked = (bool)Properties.Settings.Default[checkboxNames[i]];
                        checkboxValues[checkboxNames[i]] = chk.Checked;
                    }
                } catch{ }

                // Add the checkbox to the form
                tlp1.Controls.Add(chk,1,rowIndex);
            }
        

            this.Visible = false;
            ni1.Visible = true;
            this.ShowInTaskbar = false;
            this.Text = Application.ProductName + " - " + Application.ProductVersion;

            tlp1.RowCount = rowIndex + 1;

            if (Screen.AllScreens.Count() > 1)
            {
                cmbDisplay.Enabled = true;
                var index = 0;
                foreach (var disp in Screen.AllScreens)
                {
                    cmbDisplay.Items.Add(disp.DeviceName.Replace(".", "").Replace("\\", "").Replace("/", ""));// + "|" + disp.WorkingArea.Left + "," + disp.WorkingArea.Top + "," + disp.WorkingArea.Width + "," + disp.WorkingArea.Height);
                    if (index == Properties.Settings.Default.Monitor)
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

        // Event handler for CheckBox.CheckedChanged event
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            // Save the value to Properties.Settings
            Properties.Settings.Default[checkBox.Name] = checkBox.Checked;
            Properties.Settings.Default.Save();

            // Update the value in the dictionary
            checkboxValues[checkBox.Name] = checkBox.Checked;
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

      
    }
}
