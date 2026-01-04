using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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

        [SupportedOSPlatform("windows")]
        public frmMain()
        {
            InitializeComponent();
            loadComponents();
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        [DllImport("user32.dll")]  public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")] public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll", SetLastError = true)] public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")] public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public static char ToAscii(Keys key, Keys modifiers)
        {
            var outputBuilder = new StringBuilder(2);
            int result = ToAscii((uint)key, 0, GetKeyState(modifiers),
                                 outputBuilder, 0);
            if (result == 1)
                return outputBuilder[0];
            else
                throw new Exception("Invalid key");
        }

        private const byte HighBit = 0x80;
        private static byte[] GetKeyState(Keys modifiers)
        {
            var keyState = new byte[256];
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if ((modifiers & key) == key)
                {
                    keyState[(int)key] = HighBit;
                }
            }
            return keyState;
        }

        [DllImport("user32.dll")] private static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags);
    

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

        [SupportedOSPlatform("windows")]
        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown || e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown)
            {
                // Now you can access both, the key and virtual code
                Keys loggedKey = e.KeyboardData.Key;
                int loggedVkCode = e.KeyboardData.VirtualCode;
                StringBuilder keystr = new StringBuilder();
                ToAscii((uint)e.KeyboardData.VirtualCode,(uint)e.KeyboardData.HardwareScanCode, new byte[256], keystr,(uint)e.KeyboardData.Flags); // = '?'
                Debug.WriteLine($"{loggedKey} | {loggedVkCode} | {keystr}");
                bool reset = false;

                string txt = loggedKey.ToString().ToLower();

                if (e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown || txt == "rmenu" || txt == "lmenu")
                    txt = checkboxValues["chkAlt"] ? "Alt" : "";

                if (loggedVkCode >= 65 && loggedVkCode <= 90) //checking for alphabets
                {
                    char c = (char)loggedVkCode;
                    txt = (!checkboxValues["chkCombined"] || lastfrm == null ||
                           lastfrm.lblKeys == null ||
                           !(lastfrm.lblKeys.Text.ToLower().StartsWith("ctrl") ||
                           lastfrm.lblKeys.Text.ToLower().StartsWith("❖") ||
                           lastfrm.lblKeys.Text.ToLower().StartsWith("alt"))) ? c.ToString() : "";
                }
                else
                {
                    switch (txt)
                    {
                        case "lshiftkey":
                        case "rshiftkey":
                            txt = checkboxValues["chkShift"] ? "Shift" : "";
                            break;
                        case "lcontrolkey":
                        case "rcontrolkey":
                            txt = checkboxValues["chkCTRL"] ? "CTRL" : "";
                            break;
                        case "lwin":
                            txt = checkboxValues["chkWin"] ? "❖" : "";
                            break;
                        case "return":
                            txt = checkboxValues["chkReturn"] ? "Enter" : "";
                            break;
                        case "space":
                            txt = (lastfrm != null && lastfrm.lblKeys != null &&
                                   (lastfrm.lblKeys.Text.ToLower().StartsWith("ctrl") ||
                                   lastfrm.lblKeys.Text.ToLower().StartsWith("❖") ||
                                   lastfrm.lblKeys.Text.ToLower().StartsWith("alt"))) ? "⎵" : " ";
                            break;
                        case "home":
                        case "end":
                        case "insert":
                        case "delete":
                        case "pageup":
                        case "next":
                        case "pause":
                        case "scroll":
                        case "printscreen":
                        case "mediaplayerpause":
                        case "volumemute":
                        case "volumeup":
                        case "volumedown":
                        case "esc":
                            reset = true;
                            break;
                        case "escape":
                            txt = "Esc";
                            break;
                        case "capital":
                            txt = "Caps Lock";
                            break;
                        case "left":
                            txt = checkboxValues["chkArrows"] ? "←" : "";
                            break;
                        case "right":
                            txt = checkboxValues["chkArrows"] ? "→" : "";
                            break;
                        case "up":
                            txt = checkboxValues["chkArrows"] ? "↑" : "";
                            break;
                        case "down":
                            txt = checkboxValues["chkArrows"] ? "↓" : "";
                            break;
                        case "back":
                            txt = checkboxValues["chkBack"] ? "Back" : "";
                            reset = checkboxValues["chkBack"];
                            break;
                        default:
                            if (txt.Length == 2 && txt.StartsWith("d"))
                                txt = checkboxValues["chkNum"] ? txt.Substring(1) : "";
                            else if (txt.StartsWith("oem"))
                                txt = checkboxValues["chkOEM"] ? txt = keystr[0].ToString() : "";

                            if (txt.Contains("numpad"))
                                txt = txt.Replace("numpad", "");
                            break;
                    }
                }

                if (!clock.IsRunning)
                    clock.Start();

                double elapsed = clock.Elapsed.TotalMilliseconds;

                if (elapsed > 1000 || lastfrm == null || reset)
                {
                    Debug.WriteLine("I am in first condition {elapsed >1000 | lastfm = null | reset}");
                    clock.Restart();
                    frmPopup frm = new frmPopup();
                    lastfrm = frm;
                    Label lbl = frm.lblKeys;
                    lbl.Text = txt;
                    if (lbl.Text.Trim().Length != 0)
                    {
                        frm.Show();
                        frm.Bounds = new Rectangle(screen.WorkingArea.Left, screen.Bounds.Height - frm.Height - 100, lbl.Width, lbl.Height);
                    }
                }
                else
                {
                    Debug.WriteLine("I am in 2nd condition not{elapsed >1000 | lastfm = null | reset}");
                    clock.Restart();
                    frmPopup frm = lastfrm;
                    Label lbl = frm.lblKeys;
                    if (lbl == null)
                        return;

                    bool combined = lbl.Text.ToLower().StartsWith("shift") ||
                                    lbl.Text.ToLower().StartsWith("ctrl") ||
                                    lbl.Text.ToLower().StartsWith("❖") ||
                                    lbl.Text.ToLower().StartsWith("alt");

                    if (txt != lbl.Text && !lbl.Text.Contains(txt + " + "))
                    {
                        lbl.Text = lbl.Text + (combined ? " + " : "") + txt;
                        if (!frm.IsDisposed && lbl.Text.Trim().Length != 0)
                        {
                            frm.Show();
                            frm.Bounds = new Rectangle(screen.WorkingArea.Left, screen.Bounds.Height - frm.Height - 100, lbl.Width, lbl.Height);
                        }
                    }
                }
            }


        }

        [SupportedOSPlatform("windows")]
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hooks only into specified Keys (here "A" and "B").
            //_globalKeyboardHook = new GlobalKeyboardHook(new Keys[] { Keys.A, Keys.B });

            // Hooks into all keys.
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
           
        }


        [SupportedOSPlatform("windows")]
        private void loadComponents()
        {
            TableLayoutPanel tlp1 = new TableLayoutPanel() { Dock = DockStyle.Fill };
            tlp1.ColumnCount = 1;
            tlp1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.Controls.Add(tlp1);

            var rowIndex = -1;
            var scale = GetMagnificationScale();
            var rowHeight = 35 * scale;

            rowIndex++;
            tlp1.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            Label lbl = new Label()
            {
                Text = "Display",
                Font = new Font(this.Font, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            tlp1.Controls.Add(lbl, 0, rowIndex);

            rowIndex++;
            tlp1.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
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
            tlp1.Controls.Add(cmbDisplay, 0, rowIndex);


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
            {   "Only show alphabet combined with special keys",
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
                tlp1.Controls.Add(chk,0,rowIndex);
            }

            this.Visible = false;
            ni1.Visible = true;
            this.ShowInTaskbar = false;
            this.Text = $"{Application.ProductName} - {Application.ProductVersion}: Preferences";

            // Assuming tableLayoutPanel1 is the TableLayoutPanel on your form
            int totalHeight = tlp1.RowStyles.Cast<RowStyle>().Sum(row => (int)row.Height);

            // Optionally, add some padding or extra space
            int padding = this.Height-this.ClientRectangle.Height; // You can adjust this value as needed
            int newFormHeight = totalHeight + padding;

            // Set the form's new height
            this.Height = newFormHeight;

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
                if (cmbDisplay.Items.Count > Properties.Settings.Default.Monitor)
                    cmbDisplay.SelectedIndex = Properties.Settings.Default.Monitor;
            }
            else
            {
                cmbDisplay.Enabled = false;
            }

            cmbDisplay.SelectedIndexChanged += (sender, e) => { DrawBorderAroundScreen(cmbDisplay.SelectedIndex); };

        }

        [SupportedOSPlatform("windows")]
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

        [SupportedOSPlatform("windows")]
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        [SupportedOSPlatform("windows")]
        public static async void DrawBorderAroundScreen(int screenIndex)
        {
            if (screenIndex < 0 || screenIndex >= Screen.AllScreens.Length) return;

            Screen screen = Screen.AllScreens[screenIndex];
            using (Form frmBound = new Form())
            {
                frmBound.FormBorderStyle = FormBorderStyle.None;
                frmBound.BackColor = Color.Green;
                frmBound.TransparencyKey = Color.Green;
                frmBound.TopMost = true;
                frmBound.ShowInTaskbar = false;
                frmBound.Bounds = screen.Bounds;
                frmBound.StartPosition = FormStartPosition.Manual;

                // Handle painting safely
                frmBound.Paint += (s, e) =>
                {
                    using (Pen pen = new Pen(Color.Red, 10))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, frmBound.Width, frmBound.Height);
                    }
                };

                frmBound.Show();

                // Wait asynchronously without blocking the UI thread or tying it to the Paint event
                await Task.Delay(2000);
                frmBound.Close();
            }
        }

        [SupportedOSPlatform("windows")]
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closing = true;
            Application.Exit();
        }


        [SupportedOSPlatform("windows")]
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

        [SupportedOSPlatform("windows")]
        private void ni1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

      
    }

    [SupportedOSPlatform("windows")]
    public partial class frmPopup : Form
    {
        double opac = 1;
        public Label lblKeys = new Label();
        protected override bool ShowWithoutActivation { get { return true; } }
        
        [SupportedOSPlatform("windows")]
        protected override CreateParams CreateParams
        {
            get
            {
                //make sure Top Most property on form is set to false otherwise this doesn't work
                int WS_EX_TOPMOST = 0x00000008;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOPMOST;
                return cp;
            }
        }

        [SupportedOSPlatform("windows")]
        public frmPopup()
        {
            FormCollection frms = Application.OpenForms;
            foreach (Form f in frms)
            {
                if (f.GetType() == typeof(frmPopup))
                {
                    f.Top = f.Top - f.Height - 5;
                }
            }

            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.ShowInTaskbar = false;
            lblKeys.Font = new Font(this.Font.Name,32);
            lblKeys.ForeColor = Color.White;
            lblKeys.AutoSize  = true;
            lblKeys.SizeChanged += (sender, e) =>
            {
                this.Bounds = lblKeys.Bounds;
            };
            this.Controls.Add(lblKeys);

            Timer tmr = new Timer()
            {
                Interval = 100
            };
            tmr.Tick += (sender, e) => 
            {
                opac -= 0.1;
                if (opac <= 0)
                    this.Dispose();
                this.Opacity = opac;
                this.Top -= 3;
            };

            Timer tmr2 = new Timer() { Interval = 3000 };
            tmr2.Tick += (sender, e) => { tmr.Start(); };

            tmr2.Start();
        }

    }








}
