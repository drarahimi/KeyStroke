namespace KeyStroke
{
    // Setting keys for the visibility checkboxes, shared between UI construction
    // and key-display logic so a typo fails to compile instead of failing silently.
    internal static class CheckboxSettings
    {
        public const string Combined = "chkCombined";
        public const string Back = "chkBack";
        public const string Return = "chkReturn";
        public const string Arrows = "chkArrows";
        public const string Shift = "chkShift";
        public const string Ctrl = "chkCTRL";
        public const string Alt = "chkAlt";
        public const string Win = "chkWin";
        public const string Oem = "chkOEM";
        public const string Num = "chkNum";

        public static readonly (string Key, string Label)[] All =
        {
            (Combined, "Only show alphabet combined with special keys"),
            (Back, "Back"),
            (Return, "Return/Enter"),
            (Arrows, "Arrows"),
            (Shift, "Shift"),
            (Ctrl, "CTRL"),
            (Alt, "Alt"),
            (Win, "Windows Key"),
            (Oem, "OEM ({ } \\ ; ' ...)"),
            (Num, "Numbers (1, 2, 3, ...)")
        };
    }
}
