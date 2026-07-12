using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyStroke
{
    [SupportedOSPlatform("windows")]
    public partial class frmMain : Form
    {
        Stopwatch clock = new Stopwatch();
        frmPopup lastfrm = null;
        bool closing = false;
        Screen screen = Screen.PrimaryScreen;
        ComboBox cmbDisplay;
        // Dictionary to store checkbox values
        private Dictionary<string, bool> checkboxValues = new Dictionary<string, bool>();
        // Tracks keys that are currently held down to prevent auto-repeat
        private HashSet<Keys> _heldKeys = new HashSet<Keys>();
        public frmMain()
        {
            InitializeComponent();
            loadComponents();
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        [DllImport("user32.dll")]   public static extern short GetAsyncKeyState(Keys vKey);

        [SupportedOSPlatform("windows")]
        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            Keys key = e.KeyboardData.Key;

            // --- NEW LOGIC START ---

            // 1. Handle Key Up (User released the key)
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp ||
                e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyUp)
            {
                // Remove it from our tracker so it can be pressed again later
                _heldKeys.Remove(key);
                return;
            }

            // 2. Handle Key Down (User pressed the key)
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown ||
                e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown)
            {
                // Check if we are already holding this key. 
                // If yes, it's a Windows Auto-Repeat -> IGNORE IT.
                if (_heldKeys.Contains(key))
                {
                    return;
                }

                // Otherwise, mark it as held
                _heldKeys.Add(key);
            }
            else
            {
                // Ignore any other weird states
                return;
            }

            // --- NEW LOGIC END ---

            // 3. Translate Key to Readable String
            string keyText = GetKeyDisplayText(e);

            // If the key was disabled by a checkbox (returned null/empty), stop here.
            if (string.IsNullOrEmpty(keyText)) return;

            // 4. Handle Timing
            if (!clock.IsRunning) clock.Start();
            bool isTimedOut = clock.Elapsed.TotalMilliseconds > 1000;
            clock.Restart();

            // 5. Decide: Create New or Append?
            bool isTerminator = IsTerminatingKey(e.KeyboardData.Key);

            if (isTimedOut || lastfrm == null || lastfrm.IsDisposed || isTerminator)
            {
                CreateNewPopup(keyText);
            }
            else
            {
                AppendToPopup(keyText);
            }
        }

        // Helper: purely for translating keys to text based on your preferences
        private string GetKeyDisplayText(GlobalKeyboardHookEventArgs e)
        {
            Keys key = e.KeyboardData.Key;
            int vkCode = e.KeyboardData.VirtualCode;

            // A. Check Modifiers first (These always show up)
            if (IsModifier(key))
            {
                switch (key)
                {
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        return checkboxValues[CheckboxSettings.Shift] ? "Shift" : "";
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        return checkboxValues[CheckboxSettings.Ctrl] ? "CTRL" : "";
                    case Keys.LMenu:
                    case Keys.RMenu:
                        return checkboxValues[CheckboxSettings.Alt] ? "Alt" : "";
                    case Keys.LWin:
                    case Keys.RWin:
                        return checkboxValues[CheckboxSettings.Win] ? "❖" : "";
                }
            }

            // B. Check Special Keys (These usually show up regardless of modifiers, e.g. Enter)
            switch (key)
            {
                case Keys.Return: return checkboxValues[CheckboxSettings.Return] ? "Enter" : "";
                case Keys.Escape: return "Esc";
                case Keys.Back: return checkboxValues[CheckboxSettings.Back] ? "Back" : "";
                case Keys.Space: return "⎵";
                case Keys.Left: return checkboxValues[CheckboxSettings.Arrows] ? "←" : "";
                case Keys.Right: return checkboxValues[CheckboxSettings.Arrows] ? "→" : "";
                case Keys.Up: return checkboxValues[CheckboxSettings.Arrows] ? "↑" : "";
                case Keys.Down: return checkboxValues[CheckboxSettings.Arrows] ? "↓" : "";
                case Keys.Home: return "Home";
                case Keys.End: return "End";
                case Keys.Delete: return "Delete";
                case Keys.PageUp: return "PageUp";
                case Keys.PageDown: return "PageDown";
            }

            // --- LOGIC FOR "ONLY SHOW COMBINED" ---
            // If the checkbox is checked, we verify if a modifier is held down.
            bool hideUnlessModified = checkboxValues.ContainsKey(CheckboxSettings.Combined) && checkboxValues[CheckboxSettings.Combined];

            if (hideUnlessModified)
            {
                // Check standard modifiers
                bool isCtrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
                bool isAlt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;

                // Check Windows Key using API (0x8000 checks the high-order bit for 'currently down')
                bool isWin = (GetAsyncKeyState(Keys.LWin) & 0x8000) != 0 ||
                              (GetAsyncKeyState(Keys.RWin) & 0x8000) != 0;

                // Note: We usually DO NOT count Shift as a "combiner" for this feature 
                // because Shift+A is just typing a capital letter.
                // If NO modifier is held, return empty to suppress the OSD.
                if (!isCtrl && !isAlt && !isWin)
                {
                    return "";
                }
            }

            // C. Check Alphanumeric (A-Z)
            if (vkCode >= 65 && vkCode <= 90)
            {
                return ((char)vkCode).ToString();
            }

            // D. Check Numbers/OEM
            string name = key.ToString();
            if (name.StartsWith("D") && name.Length == 2 && char.IsDigit(name[1]))
                return checkboxValues[CheckboxSettings.Num] ? name.Substring(1) : "";

            if (name.StartsWith("NumPad"))
                return name.Replace("NumPad", "");

            if (name.StartsWith("Oem"))
            {
                return checkboxValues[CheckboxSettings.Oem] ? GetOemGlyph(key) : "";
            }

            return "";
        }

        // Maps OEM virtual keys to their unshifted US-layout glyph.
        private static string GetOemGlyph(Keys key)
        {
            switch (key)
            {
                case Keys.OemSemicolon: return ";";
                case Keys.Oemplus: return "=";
                case Keys.Oemcomma: return ",";
                case Keys.OemMinus: return "-";
                case Keys.OemPeriod: return ".";
                case Keys.OemQuestion: return "/";
                case Keys.Oemtilde: return "`";
                case Keys.OemOpenBrackets: return "[";
                case Keys.OemPipe: return "\\";
                case Keys.OemCloseBrackets: return "]";
                case Keys.OemQuotes: return "'";
                case Keys.OemBackslash: return "\\";
                default: return "?";
            }
        }

        private void CreateNewPopup(string text)
        {
            frmPopup frm = new frmPopup();
            lastfrm = frm; // Update state
            frm.lblKeys.Text = text;

            PositionAndShow(frm);
        }

        private void AppendToPopup(string text)
        {
            string current = lastfrm.lblKeys.Text;

            // Logic: Do we add a "+" or just the letter?
            // Check if the current display is a Modifier (e.g., "CTRL")
            bool isModifierHeld = current.Equals("CTRL") || current.Equals("Shift") ||
                                  current.Equals("Alt") || current.Equals("❖");

            if (isModifierHeld)
            {
                lastfrm.lblKeys.Text += " + " + text;
            }
            else
            {
                lastfrm.lblKeys.Text += text;
            }

            PositionAndShow(lastfrm);
        }

        private void PositionAndShow(frmPopup frm)
        {
            if (frm.lblKeys.Text.Trim().Length == 0) return;

            frm.Show();
            // Recalculate bounds based on new text size
            Screen targetScreen = screen ?? Screen.PrimaryScreen;
            frm.Bounds = new Rectangle(
                targetScreen.WorkingArea.Left,
                targetScreen.WorkingArea.Top + targetScreen.WorkingArea.Height - frm.Height - 100,
                frm.lblKeys.PreferredWidth, // Use PreferredWidth for auto-sizing
                frm.lblKeys.PreferredHeight
            );
        }

        // Helpers
        private bool IsModifier(Keys k) =>
            k == Keys.LControlKey || k == Keys.RControlKey ||
            k == Keys.LShiftKey || k == Keys.RShiftKey ||
            k == Keys.LMenu || k == Keys.RMenu ||
            k == Keys.LWin || k == Keys.RWin;

        private bool IsTerminatingKey(Keys k) =>
            k == Keys.Enter || k == Keys.Escape || k == Keys.Back;

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hooks into all keys.
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
           
        }


        private void loadComponents()
        {
            TableLayoutPanel tlp1 = new TableLayoutPanel() { Dock = DockStyle.Fill };
            tlp1.ColumnCount = 1;
            tlp1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.Controls.Add(tlp1);

            var rowIndex = -1;
            var rowHeight = 30;

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
            tlp1.RowStyles.Add(new RowStyle(SizeType.AutoSize, rowHeight));
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


            foreach (var (key, label) in CheckboxSettings.All)
            {
                rowIndex++;
                tlp1.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                CheckBox chk = new CheckBox()
                {
                    Name = key,
                    Text = label,
                    Dock = DockStyle.Fill
                };
                chk.CheckedChanged += CheckBox_CheckedChanged;

                // Load the saved value from Properties.Settings
                try
                {
                    if (Properties.Settings.Default[key] != null)
                    {
                        chk.Checked = (bool)Properties.Settings.Default[key];
                        checkboxValues[key] = chk.Checked;
                    }
                }
                catch { }

                // Add the checkbox to the form
                tlp1.Controls.Add(chk, 0, rowIndex);
            }

            this.Visible = false;
            ni1.Visible = true;
            this.ShowInTaskbar = false;
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            this.Text = $"{Application.ProductName} - {version}: Preferences";

            // Assuming tableLayoutPanel1 is the TableLayoutPanel on your form
            int totalHeight = tlp1.RowStyles.Cast<RowStyle>().Sum(row => (int)row.Height);

            // Optionally, add some padding or extra space
            int padding = this.Height - this.ClientRectangle.Height; // You can adjust this value as needed
            int newFormHeight = totalHeight + padding;

            // Set the form's new height
            this.Height = newFormHeight;

            tlp1.RowCount = rowIndex + 1;

            // Clear existing items to prevent duplicates if run multiple times
            cmbDisplay.Items.Clear();

            Screen[] allScreens = Screen.AllScreens;

            // 1. Populate the ComboBox
            for (int i = 0; i < allScreens.Length; i++)
            {
                // Wrap the screen in our helper class
                var item = new ScreenDisplayItem
                {
                    Screen = allScreens[i],
                    SystemIndex = i
                };
                cmbDisplay.Items.Add(item);
            }

            // 2. Handle Selection Logic
            if (cmbDisplay.Items.Count > 0)
            {
                int savedIndex = Properties.Settings.Default.Monitor;

                // Safety check: Does the saved index actually exist? (e.g. user unplugged a monitor)
                if (savedIndex >= 0 && savedIndex < cmbDisplay.Items.Count)
                {
                    cmbDisplay.SelectedIndex = savedIndex;
                }
                else
                {
                    cmbDisplay.SelectedIndex = 0; // Default to first available
                }
            }

            // 3. Toggle Availability
            // Disable the box if there is only 1 screen, but keep it visible so they see info
            cmbDisplay.Enabled = (allScreens.Length > 1);

            // 4. Clean Event Handling
            // Remove first to ensure we don't subscribe twice if this function runs again
            cmbDisplay.SelectedIndexChanged -= OnScreenSelectionChanged;
            cmbDisplay.SelectedIndexChanged += OnScreenSelectionChanged;

            // Force the update immediately so the 'screen' variable is set correctly on load
            OnScreenSelectionChanged(cmbDisplay, EventArgs.Empty);

        }

        private void OnScreenSelectionChanged(object sender, EventArgs e)
        {
            if (cmbDisplay.SelectedItem is ScreenDisplayItem selectedItem)
            {
                // Update your global variable
                this.screen = selectedItem.Screen;

                // Visual Feedback: Draw the border so they know which one they picked
                // We use SystemIndex because your DrawBorder function likely expects the raw index
                DrawBorderAroundScreen(selectedItem.SystemIndex);
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
    public class ScreenDisplayItem
    {
        public Screen Screen { get; set; }
        public int SystemIndex { get; set; }

        // This overrides what the ComboBox displays
        public override string ToString()
        {
            string label = $"Display {SystemIndex + 1}";

            if (Screen.Primary)
                label += " (Main)";

            // Adding resolution helps users identify screens quickly
            label += $" - {Screen.Bounds.Width}x{Screen.Bounds.Height}";

            return label;
        }
    }


    [SupportedOSPlatform("windows")]
    public partial class frmPopup : Form
    {
        // Use double for calculation, but float is native to Opacity
        private double opac = 1.0;
        public Label lblKeys = new Label();

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                int WS_EX_TOPMOST = 0x00000008;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOPMOST;
                return cp;
            }
        }

        public frmPopup()
        {

            // 1. Setup UI
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.ShowInTaskbar = false;

            // Setup Label
            lblKeys.Font = new Font(this.Font.Name, 32);
            lblKeys.ForeColor = Color.White;
            lblKeys.AutoSize = true;
            lblKeys.Location = Point.Empty; // Ensure it starts at 0,0 inside the form

            // --- FIX 1: Prevent Jump ---
            // Only update the size of the form, preserve the Screen Location
            lblKeys.SizeChanged += (sender, e) =>
            {
                this.ClientSize = lblKeys.Size;
            };
            this.Controls.Add(lblKeys);

            // 2. Handle Stacking (Move previous toasts up)
            // usage of ToList() prevents collection modified errors if forms close during loop
            foreach (Form f in Application.OpenForms.Cast<Form>().ToList())
            {
                // We verify it is a popup AND it is not THIS current new popup
                if (f.GetType() == typeof(frmPopup) && f != this)
                {
                    f.Top -= (this.Height + 5);
                }
            }

            // --- FIX 2: Smooth Fading ---
            // 15ms interval is approx 60 FPS
            Timer tmrAnimation = new Timer { Interval = 15 };

            tmrAnimation.Tick += (sender, e) =>
            {
                // Decrease opacity by a smaller amount for smoothness
                opac -= 0.02;
                this.Top -= 1; // Slight upward float effect

                if (opac <= 0)
                {
                    tmrAnimation.Stop();
                    this.Close(); // Close is generally safer than Dispose inside a form
                }
                else
                {
                    this.Opacity = opac;
                }
            };

            // Delay timer to start the animation
            Timer tmrDelay = new Timer { Interval = 3000 };
            tmrDelay.Tick += (sender, e) =>
            {
                tmrDelay.Stop();
                tmrAnimation.Start();
            };

            tmrDelay.Start();
        }
    }








}
