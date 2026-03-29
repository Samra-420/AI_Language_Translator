using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AI_Language_Translator
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "LinguaAI";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += OuterForm_Paint;

            Color teal = Color.FromArgb(0, 188, 170);
            Color white = Color.White;

            // ================= PHONE =================
            Panel phone = new Panel();
            phone.Size = new Size(300, 520);
            phone.Location = new Point(300, 60);
            phone.BackColor = Color.Transparent;
            phone.Paint += PhonePanel_Paint;

            // ================= HEADER =================
            Panel header = new Panel();
            header.Size = new Size(300, 60);
            header.Location = new Point(0, 0);
            header.Paint += HeaderPanel_Paint;

            // 🔙 BACK BUTTON → FORMHOME
            Label btnBack = new Label();
            btnBack.Text = "←";
            btnBack.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnBack.ForeColor = white;
            btnBack.AutoSize = true;
            btnBack.Location = new Point(15, 15);
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += (s, e) =>
            {
                FormHome h = new FormHome();
                h.Show();
                this.Hide();
            };
            header.Controls.Add(btnBack);

            // ================= LOGO =================
            Label logo = new Label();
            logo.Text = "🌐";
            logo.Font = new Font("Segoe UI Emoji", 26);
            logo.AutoSize = true;
            logo.Location = new Point(125, 70);

            // ================= MAIN MENU TEXT =================
            Label lblMenu = new Label();
            lblMenu.Text = "Main Menu";
            lblMenu.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblMenu.ForeColor = Color.FromArgb(0, 121, 107);
            lblMenu.AutoSize = true;
            lblMenu.Location = new Point(95, 115);

            // ================= BUTTON COLORS (LIGHT THEME) =================
            Color c1 = Color.FromArgb(200, 245, 240);
            Color c2 = Color.FromArgb(210, 250, 245);  // History button color
            Color c3 = Color.FromArgb(220, 252, 248);
            Color c4 = Color.FromArgb(255, 230, 230);

            // ================= BUTTONS =================
            Button btnTranslate = MakeButton("🌐", "Translate", new Point(40, 160), c1, teal);
            btnTranslate.Click += BtnTranslator_Click;

            // ✅ HISTORY replaces Settings
            Button btnHistory = MakeButton("🕐", "History", new Point(40, 220), c2, teal);
            btnHistory.Click += BtnHistory_Click;

            Button btnAbout = MakeButton("ℹ️", "About", new Point(40, 280), c3, teal);
            btnAbout.Click += BtnAbout_Click;

            Button btnExit = MakeButton("❌", "Exit", new Point(40, 340), c4, Color.Red);
            btnExit.Click += (s, e) => Application.Exit();

            // ================= FOOTER =================
            Label footer = new Label();
            footer.Text = "© 2025 LinguaAI";
            footer.Font = new Font("Segoe UI", 8);
            footer.ForeColor = Color.Gray;
            footer.AutoSize = true;
            footer.Location = new Point(95, 470);

            // ================= ADD =================
            phone.Controls.Add(header);
            phone.Controls.Add(logo);
            phone.Controls.Add(lblMenu);
            phone.Controls.Add(btnTranslate);
            phone.Controls.Add(btnHistory);   // ✅ was btnSettings
            phone.Controls.Add(btnAbout);
            phone.Controls.Add(btnExit);
            phone.Controls.Add(footer);

            this.Controls.Add(phone);
        }

        // ================= BACKGROUND =================
        private void OuterForm_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush bg = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(210, 240, 238),
                Color.FromArgb(150, 210, 205),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(bg, this.ClientRectangle);
            }
        }

        // ================= PHONE =================
        private void PhonePanel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, p.Width - 1, p.Height - 1), 40))
            {
                g.FillPath(new SolidBrush(Color.White), path);
                g.DrawPath(new Pen(Color.FromArgb(0, 188, 170), 2), path);
            }
        }

        // ================= HEADER =================
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRectTop(new Rectangle(0, 0, p.Width, p.Height + 40), 40))
            using (LinearGradientBrush br = new LinearGradientBrush(
                p.ClientRectangle,
                Color.FromArgb(0, 210, 190),
                Color.FromArgb(0, 170, 152),
                LinearGradientMode.Vertical))
            {
                g.FillPath(br, path);
            }
        }

        // ================= BUTTON =================
        private Button MakeButton(string icon, string text, Point loc, Color bg, Color fg)
        {
            Button b = new Button();
            b.Size = new Size(220, 50);
            b.Location = loc;
            b.BackColor = bg;
            b.ForeColor = fg;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;

            b.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, b.Width - 1, b.Height - 1), 25))
                {
                    g.FillPath(new SolidBrush(b.BackColor), path);
                }

                Rectangle iconBox = new Rectangle(10, 10, 30, 30);

                using (SolidBrush br = new SolidBrush(Color.FromArgb(0, 188, 170)))
                    g.FillEllipse(br, iconBox);

                TextRenderer.DrawText(g, icon,
                    new Font("Segoe UI Emoji", 12),
                    iconBox,
                    Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                TextRenderer.DrawText(g, text,
                    new Font("Segoe UI", 10, FontStyle.Bold),
                    new Rectangle(50, 0, 150, 50),
                    b.ForeColor,
                    TextFormatFlags.VerticalCenter);
            };

            b.MouseEnter += (s, e) =>
            {
                b.BackColor = Color.FromArgb(0, 188, 170);
                b.ForeColor = Color.White;
                b.Invalidate();
            };

            b.MouseLeave += (s, e) =>
            {
                b.BackColor = bg;
                b.ForeColor = fg;
                b.Invalidate();
            };

            return b;
        }

        // ================= ROUND =================
        private GraphicsPath RoundedRect(Rectangle r, int d)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private GraphicsPath RoundedRectTop(Rectangle r, int d)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
            p.CloseFigure();
            return p;
        }

        // ================= EVENTS =================
        private void BtnTranslator_Click(object sender, EventArgs e)
        {
            FormTranslator t = new FormTranslator();
            t.Show();
            this.Hide();
        }

        // ✅ NEW: History button handler
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            FormHistory fh = new FormHistory();
            fh.Show();
            this.Hide();
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            FormAbout a = new FormAbout();
            a.Show();
            this.Hide();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {

        }
    }
}