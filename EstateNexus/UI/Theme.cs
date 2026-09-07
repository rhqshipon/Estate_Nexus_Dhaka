using System;
using System.Drawing;
using System.Windows.Forms;

namespace EstateNexus.UI
{
    public static class Theme
    {
        // Theme Colors
        public static readonly Color Background = ColorTranslator.FromHtml("#DFF5FA");
        public static readonly Color Surface = Color.White;
        public static readonly Color Primary = ColorTranslator.FromHtml("#2F6FA8");
        public static readonly Color Accent = ColorTranslator.FromHtml("#6FCBCB");
        public static readonly Color Danger = ColorTranslator.FromHtml("#9C6B6B");
        public static readonly Color Navy = ColorTranslator.FromHtml("#163A5F");
        public static readonly Color TextDark = ColorTranslator.FromHtml("#1F2D4A");
        public static readonly Color RowAlt = ColorTranslator.FromHtml("#F3FAFC");

        // Fonts
        public static readonly Font TitleFont = new Font("Cambria", 18f, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font ButtonFont = new Font("Cambria", 10.5f, FontStyle.Bold);

        public static void ApplyForm(Form form)
        {
            if (form == null) return;

            form.BackColor = Background;

            // Center form if not a child/dialog with existing center setting
            if (form.StartPosition == FormStartPosition.WindowsDefaultLocation || form.StartPosition == FormStartPosition.WindowsDefaultBounds)
            {
                form.StartPosition = FormStartPosition.CenterScreen;
            }

            ApplyControlStyles(form.Controls);
        }

        private static void ApplyControlStyles(Control.ControlCollection controls)
        {
            if (controls == null) return;

            foreach (Control ctrl in controls)
            {
                if (ctrl is DataGridView dgv)
                {
                    dgv.BackgroundColor = Surface;
                    dgv.BorderStyle = BorderStyle.None;
                    dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dgv.GridColor = Color.FromArgb(220, 230, 240);
                    dgv.RowHeadersVisible = false;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgv.ReadOnly = true;

                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Primary;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                    dgv.ColumnHeadersHeight = 34;

                    dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                    dgv.DefaultCellStyle.ForeColor = TextDark;
                    dgv.DefaultCellStyle.SelectionBackColor = Accent;
                    dgv.DefaultCellStyle.SelectionForeColor = TextDark;

                    dgv.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
                    dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextDark;
                    dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Accent;
                    dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextDark;
                }
                else if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = ButtonFont;
                    btn.Cursor = Cursors.Hand;

                    string text = (btn.Text ?? "").ToLowerInvariant();
                    string name = (btn.Name ?? "").ToLowerInvariant();

                    bool isDanger = text.Contains("delete") || text.Contains("reject") ||
                                    text.Contains("clear") || text.Contains("cancel") ||
                                    text.Contains("suspend") || text.Contains("logout") ||
                                    name.Contains("delete") || name.Contains("reject") ||
                                    name.Contains("clear") || name.Contains("cancel") ||
                                    name.Contains("suspend") || name.Contains("logout");

                    if (isDanger)
                    {
                        btn.BackColor = Danger;
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Primary;
                        btn.ForeColor = Color.White;
                    }
                }
                else if (ctrl is TextBox tb)
                {
                    tb.BackColor = Surface;
                    tb.ForeColor = TextDark;
                }
                else if (ctrl is ComboBox cb)
                {
                    cb.BackColor = Surface;
                    cb.ForeColor = TextDark;
                }
                else if (ctrl is NumericUpDown nud)
                {
                    nud.BackColor = Surface;
                    nud.ForeColor = TextDark;
                }
                else if (ctrl is Label lbl)
                {
                    // If label has large title font
                    if (lbl.Font.Size >= 14)
                    {
                        lbl.Font = new Font("Cambria", lbl.Font.Size >= 18 ? 20f : 16f, FontStyle.Bold);
                        lbl.ForeColor = Navy;
                    }
                    else
                    {
                        // Preserve error/status colors (Red/Green)
                        if (lbl.ForeColor != Color.Red &&
                            lbl.ForeColor != Color.DarkRed &&
                            lbl.ForeColor != Color.DarkGreen &&
                            lbl.ForeColor != Color.Green)
                        {
                            lbl.ForeColor = TextDark;
                        }
                    }
                }
                else if (ctrl is TabPage tp)
                {
                    tp.BackColor = Background;
                }
                else if (ctrl is TabControl tc)
                {
                    tc.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                }
                else if (ctrl is GroupBox gb)
                {
                    gb.ForeColor = Navy;
                    gb.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                }

                // Recurse into child controls (panels, groupboxes, tab pages, etc.)
                if (ctrl.HasChildren)
                {
                    ApplyControlStyles(ctrl.Controls);
                }
            }
        }
    }
}
