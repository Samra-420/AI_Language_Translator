using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AI_Language_Translator
{
    public partial class FormHome : Form
    {
        Panel phone;

        public FormHome()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "LinguaAI";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Paint += OuterForm_Paint;

            Color teal = Color.FromArgb(0, 188, 170);
            Color darkTeal = Color.FromArgb(0, 150, 136);
            Color white = Color.White;

            // PHONE PANEL
            phone = new Panel();
            phone.Size = new Size(280, 500);
            phone.BackColor = Color.Transparent;
            phone.Paint += PhonePanel_Paint;

            // HEADER (NO TEXT)
            Panel header = new Panel();
            header.Size = new Size(280, 70);
            header.BackColor = teal;
            header.Paint += HeaderPanel_Paint;

            // GLOBE
            Label lblGlobe = new Label();
            lblGlobe.Text = "🌍";
            lblGlobe.Font = new Font("Segoe UI Emoji", 55f);
            lblGlobe.AutoSize = true;
            lblGlobe.Location = new Point(78, 90);

            // NAME
            Label lblName = new Label();
            lblName.Text = "LinguaAI";
            lblName.Font = new Font("Segoe UI", 20f, FontStyle.Bold);
            lblName.ForeColor = darkTeal;
            lblName.AutoSize = true;
            lblName.Location = new Point(78, 220);

            // TAGLINE
            Label lblTag = new Label();
            lblTag.Text = "Translate anything, instantly.";
            lblTag.Font = new Font("Segoe UI", 8f, FontStyle.Italic);
            lblTag.ForeColor = Color.FromArgb(100, 180, 170);
            lblTag.AutoSize = true;
            lblTag.Location = new Point(48, 255);

            // BUTTON (FULL ROUND)
            Button btnStart = new Button();
            btnStart.Text = "Get Started";
            btnStart.Size = new Size(200, 45);
            btnStart.Location = new Point(40, 300);
            btnStart.BackColor = teal;
            btnStart.ForeColor = white;
            btnStart.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Cursor = Cursors.Hand;
            btnStart.Paint += BtnStart_Paint;

            btnStart.MouseEnter += (s, e) => { btnStart.BackColor = darkTeal; btnStart.Invalidate(); };
            btnStart.MouseLeave += (s, e) => { btnStart.BackColor = teal; btnStart.Invalidate(); };
            btnStart.Click += BtnStart_Click;

            // FOOTER
            Panel bottomBar = new Panel();
            bottomBar.Size = new Size(280, 40);
            bottomBar.Location = new Point(0, 460);
            bottomBar.BackColor = teal;
            bottomBar.Paint += BottomBar_Paint;

            // ADD CONTROLS
            phone.Controls.Add(header);
            phone.Controls.Add(lblGlobe);
            phone.Controls.Add(lblName);
            phone.Controls.Add(lblTag);
            phone.Controls.Add(btnStart);
            phone.Controls.Add(bottomBar);

            this.Controls.Add(phone);

            CenterPhone();
            this.Resize += (s, e) => CenterPhone();
        }

        // CENTER PHONE
        private void CenterPhone()
        {
            phone.Left = (this.ClientSize.Width - phone.Width) / 2;
            phone.Top = (this.ClientSize.Height - phone.Height) / 2;
        }

        // BACKGROUND
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

        // PHONE (ROUNDED LIKE MENU)
        private void PhonePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, phone.Width - 1, phone.Height - 1), 40))
            {
                g.FillPath(new SolidBrush(Color.White), path);
                g.DrawPath(new Pen(Color.FromArgb(0, 188, 170), 2f), path);
            }
        }

        // HEADER ROUND TOP
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRectTop(new Rectangle(0, 0, pnl.Width, pnl.Height + 40), 40))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 188, 170)))
            {
                g.FillPath(brush, path);
            }
        }

        // BUTTON FULL ROUND (PILL SHAPE)
        private void BtnStart_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRect(
                new Rectangle(0, 0, btn.Width - 1, btn.Height - 1),
                btn.Height / 2)) // FULL ROUND
            {
                g.FillPath(new SolidBrush(btn.BackColor), path);

                TextRenderer.DrawText(
                    g,
                    btn.Text,
                    btn.Font,
                    btn.ClientRectangle,
                    Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        // FOOTER
        private void BottomBar_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRectBottom(new Rectangle(0, -40, pnl.Width, pnl.Height + 40), 40))
            {
                g.FillPath(new SolidBrush(Color.FromArgb(0, 188, 170)), path);
            }

            TextRenderer.DrawText(
                g,
                "© 2025 LinguaAI",
                new Font("Segoe UI", 8f),
                pnl.ClientRectangle,
                Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // HELPERS
        private GraphicsPath RoundedRect(Rectangle b, int r)
        {
            int d = r * 2;
            GraphicsPath p = new GraphicsPath();
            p.AddArc(b.X, b.Y, d, d, 180, 90);
            p.AddArc(b.X + b.Width - d, b.Y, d, d, 270, 90);
            p.AddArc(b.X + b.Width - d, b.Y + b.Height - d, d, d, 0, 90);
            p.AddArc(b.X, b.Y + b.Height - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private GraphicsPath RoundedRectTop(Rectangle b, int r)
        {
            int d = r * 2;
            GraphicsPath p = new GraphicsPath();
            p.AddArc(b.X, b.Y, d, d, 180, 90);
            p.AddArc(b.X + b.Width - d, b.Y, d, d, 270, 90);
            p.AddLine(b.X + b.Width, b.Y + b.Height, b.X, b.Y + b.Height);
            p.CloseFigure();
            return p;
        }

        private GraphicsPath RoundedRectBottom(Rectangle b, int r)
        {
            int d = r * 2;
            GraphicsPath p = new GraphicsPath();
            p.AddLine(b.X, b.Y, b.X + b.Width, b.Y);
            p.AddArc(b.X + b.Width - d, b.Y + b.Height - d, d, d, 0, 90);
            p.AddArc(b.X, b.Y + b.Height - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // CLICK
        private void BtnStart_Click(object sender, EventArgs e)
        {
            FormMenu menu = new FormMenu();
            menu.Show();
            this.Hide();
        }
    }
}