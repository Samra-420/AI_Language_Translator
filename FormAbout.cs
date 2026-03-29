using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AI_Language_Translator
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "LinguaAI – About";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(220, 242, 240);
            this.Paint += OuterForm_Paint;

            // ── Colors ────────────────────────────────
            Color teal = Color.FromArgb(0, 188, 170);
            Color white = Color.White;
            Color lightTeal = Color.FromArgb(178, 235, 230);

            // Step colors
            Color[] stepColors = new Color[]
            {
                Color.FromArgb(255, 107, 107),  // Red
                Color.FromArgb(255, 193, 107), // Orange
                Color.FromArgb(129, 199, 132), // Green
                Color.FromArgb(100, 181, 246), // Blue
                Color.FromArgb(206, 147, 216), // Purple
                Color.FromArgb(255, 138, 128)  // Coral
            };

            string[] icons = new string[]
            {
                "✏️", "🌐", "🔄", "⚡", "📄", "📱"
            };

            // ── Phone Frame (fully rounded) ───────────
            Panel phone = new Panel();
            phone.Size = new Size(280, 500);
            phone.Location = new Point(310, 75);
            phone.BackColor = white;
            phone.Paint += (s, pe) =>
            {
                Panel pnl = (Panel)s;
                Graphics g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Draw fully rounded rectangle with consistent radius
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1), 35))
                {
                    g.FillPath(Brushes.White, path);
                    using (Pen pen = new Pen(Color.FromArgb(0, 188, 170), 2f))
                        g.DrawPath(pen, path);
                }
            };

            // ── Header ────────────────────────────────
            Panel header = new Panel();
            header.Size = new Size(280, 75);
            header.Location = new Point(0, 0);
            header.BackColor = teal;
            header.Paint += (s, pe) =>
            {
                Panel pnl = (Panel)s;
                Graphics g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Top rounded corners only for header
                using (GraphicsPath path = RoundedRectTop(new Rectangle(0, 0, pnl.Width, pnl.Height), 35))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 188, 170)))
                    g.FillPath(brush, path);
            };

            // Back button
            Button btnBack = new Button();
            btnBack.Text = "←";
            btnBack.Size = new Size(32, 32);
            btnBack.Location = new Point(10, 21);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.BackColor = Color.Transparent;
            btnBack.ForeColor = white;
            btnBack.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += (s, e) => { new FormMenu().Show(); this.Hide(); };
            header.Controls.Add(btnBack);

            Label lblTitle = new Label();
            lblTitle.Text = "About App";
            lblTitle.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
            lblTitle.ForeColor = white;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(75, 14);
            header.Controls.Add(lblTitle);

            Label lblSub = new Label();
            lblSub.Text = "How the app works";
            lblSub.Font = new Font("Segoe UI", 8f, FontStyle.Italic);
            lblSub.ForeColor = Color.FromArgb(210, 245, 242);
            lblSub.AutoSize = true;
            lblSub.BackColor = Color.Transparent;
            lblSub.Location = new Point(85, 47);
            header.Controls.Add(lblSub);

            // ── App Logo / Icon Area ──────────────────
            Panel logoCircle = new Panel();
            logoCircle.Size = new Size(72, 72);
            logoCircle.Location = new Point(104, 88);
            logoCircle.BackColor = Color.Transparent;
            logoCircle.Paint += (s, pe) =>
            {
                Graphics g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(teal))
                    g.FillEllipse(b, 0, 0, 71, 71);

                using (Font iconFont = new Font("Segoe UI Emoji", 28f))
                {
                    TextRenderer.DrawText(g, "🌐", iconFont,
                        new Rectangle(0, 0, 72, 72),
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };

            // App name under logo
            Label lblAppName = new Label();
            lblAppName.Text = "LinguaAI";
            lblAppName.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblAppName.ForeColor = teal;
            lblAppName.AutoSize = false;
            lblAppName.Size = new Size(280, 28);
            lblAppName.Location = new Point(0, 168);
            lblAppName.TextAlign = ContentAlignment.MiddleCenter;
            lblAppName.BackColor = Color.Transparent;

            // ── Thin divider ──────────────────────────
            Panel div1 = new Panel();
            div1.Size = new Size(220, 1);
            div1.Location = new Point(30, 200);
            div1.BackColor = lightTeal;

            // ── Steps with colorful boxes ───────
            string[] stepTitles = new string[]
            {
                "Enter Text",
                "Select Languages",
                "Swap Languages",
                "Translate",
                "View Output",
                "Mobile Interface"
            };

            string[] stepDescriptions = new string[]
            {
                "Type or paste your text in the input field",
                "Choose source and target languages from dropdowns",
                "Quickly swap languages with one click",
                "Click the Translate button for instant results",
                "Translated text appears in the output box",
                "Clean mobile-style interface for easy use"
            };

            int startY = 220;
            for (int i = 0; i < stepTitles.Length; i++)
            {
                // Step container panel
                Panel stepPanel = new Panel();
                stepPanel.Size = new Size(240, 60);
                stepPanel.Location = new Point(20, startY);
                stepPanel.BackColor = Color.White;
                stepPanel.Cursor = Cursors.Hand;

                // Add hover effect
                stepPanel.MouseEnter += (s, e) =>
                {
                    Panel p = (Panel)s;
                    p.BackColor = Color.FromArgb(248, 248, 248);
                    p.Invalidate();
                };
                stepPanel.MouseLeave += (s, e) =>
                {
                    Panel p = (Panel)s;
                    p.BackColor = Color.White;
                    p.Invalidate();
                };

                // Icon circle with color
                Panel iconCircle = new Panel();
                iconCircle.Size = new Size(36, 36);
                iconCircle.Location = new Point(8, 12);
                iconCircle.BackColor = stepColors[i % stepColors.Length];
                iconCircle.Paint += (circleSender, circleEvent) =>
                {
                    Graphics g = circleEvent.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    using (SolidBrush brush = new SolidBrush(stepColors[i % stepColors.Length]))
                    {
                        g.FillEllipse(brush, 0, 0, 35, 35);
                    }

                    using (Font iconFont = new Font("Segoe UI Emoji", 14f))
                    {
                        TextRenderer.DrawText(g, icons[i % icons.Length], iconFont,
                            new Rectangle(0, 0, 36, 36),
                            Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                };

                // Step title
                Label stepTitle = new Label();
                stepTitle.Text = stepTitles[i];
                stepTitle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                stepTitle.ForeColor = stepColors[i % stepColors.Length];
                stepTitle.AutoSize = false;
                stepTitle.Size = new Size(180, 20);
                stepTitle.Location = new Point(52, 10);
                stepTitle.TextAlign = ContentAlignment.MiddleLeft;

                // Step description
                Label stepDesc = new Label();
                stepDesc.Text = stepDescriptions[i];
                stepDesc.Font = new Font("Segoe UI", 7.5f);
                stepDesc.ForeColor = Color.Gray;
                stepDesc.AutoSize = false;
                stepDesc.Size = new Size(180, 32);
                stepDesc.Location = new Point(52, 30);
                stepDesc.TextAlign = ContentAlignment.TopLeft;

                stepPanel.Controls.Add(iconCircle);
                stepPanel.Controls.Add(stepTitle);
                stepPanel.Controls.Add(stepDesc);

                phone.Controls.Add(stepPanel);
                startY += 70;
            }

            // ── Bottom Bar ───────────────────────────
            Panel bottomBar = new Panel();
            bottomBar.Size = new Size(280, 38);
            bottomBar.Location = new Point(0, 462);
            bottomBar.BackColor = teal;
            bottomBar.Paint += (s, pe) =>
            {
                Panel pnl = (Panel)s;
                Graphics g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Bottom rounded corners only
                using (GraphicsPath path = RoundedRectBottom(new Rectangle(0, 0, pnl.Width, pnl.Height), 35))
                using (SolidBrush fill = new SolidBrush(Color.FromArgb(0, 150, 136)))
                    g.FillPath(fill, path);

                using (Font f = new Font("Segoe UI", 8f))
                {
                    TextRenderer.DrawText(g, "© 2026 LinguaAI", f,
                        new Rectangle(0, 0, pnl.Width, pnl.Height),
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };

            // ── Assemble ──────────────────────────────
            phone.Controls.Add(header);
            phone.Controls.Add(logoCircle);
            phone.Controls.Add(lblAppName);
            phone.Controls.Add(div1);
            phone.Controls.Add(bottomBar);

            this.Controls.Add(phone);
        }

        // ────────── Paint Handlers ───────────────
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

        // ────────── Helpers ───────────────
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
            p.AddLine(b.X + b.Width, b.Y + r, b.X + b.Width, b.Y + b.Height);
            p.AddLine(b.X + b.Width, b.Y + b.Height, b.X, b.Y + b.Height);
            p.AddLine(b.X, b.Y + b.Height, b.X, b.Y + r);
            p.CloseFigure();
            return p;
        }

        private GraphicsPath RoundedRectBottom(Rectangle b, int r)
        {
            int d = r * 2;
            GraphicsPath p = new GraphicsPath();
            p.AddLine(b.X, b.Y, b.X + b.Width, b.Y);
            p.AddLine(b.X + b.Width, b.Y, b.X + b.Width, b.Y + b.Height - r);
            p.AddArc(b.X + b.Width - d, b.Y + b.Height - d, d, d, 0, 90);
            p.AddArc(b.X, b.Y + b.Height - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}