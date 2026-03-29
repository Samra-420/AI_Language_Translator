using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AI_Language_Translator
{
    public partial class FormHistory : Form
    {
        private readonly Color teal = Color.FromArgb(0, 188, 170);
        private readonly Color white = Color.White;

        private FlowLayoutPanel recordsPanel;
        private Label lblEmpty;
        private TextBox searchBox;
        private List<TranslationRecord> allRecords = new List<TranslationRecord>();

        public FormHistory()
        {
            InitializeComponent();
            SetupForm();
            LoadHistory();
        }

        // ═══════════════════════════════════════════════
        //  SETUP
        // ═══════════════════════════════════════════════
        private void SetupForm()
        {
            this.Text = "LinguaAI – History";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += OuterForm_Paint;

            // ── PHONE ──────────────────────────────────
            Panel phone = new Panel();
            phone.Size = new Size(300, 540);
            phone.Location = new Point(300, 55);
            phone.BackColor = Color.Transparent;
            phone.Paint += PhonePanel_Paint;

            // ── HEADER ─────────────────────────────────
            Panel header = new Panel();
            header.Size = new Size(300, 60);
            header.Location = new Point(0, 0);
            header.Paint += HeaderPanel_Paint;

            Label btnBack = new Label();
            btnBack.Text = "←";
            btnBack.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnBack.ForeColor = white;
            btnBack.AutoSize = true;
            btnBack.Location = new Point(12, 15);
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += (s, e) => { new FormMenu().Show(); this.Hide(); };
            header.Controls.Add(btnBack);

            Label lblTitle = new Label();
            lblTitle.Text = "Translation History";
            lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTitle.ForeColor = white;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(68, 19);
            header.Controls.Add(lblTitle);

            // 🗑 Clear All
            Label btnClearAll = new Label();
            btnClearAll.Text = "🗑";
            btnClearAll.Font = new Font("Segoe UI Emoji", 13);
            btnClearAll.AutoSize = true;
            btnClearAll.Location = new Point(263, 15);
            btnClearAll.Cursor = Cursors.Hand;
            btnClearAll.Click += BtnClearAll_Click;
            header.Controls.Add(btnClearAll);

            // ── SEARCH ─────────────────────────────────
            Panel searchWrapper = new Panel();
            searchWrapper.Size = new Size(260, 34);
            searchWrapper.Location = new Point(20, 70);
            searchWrapper.BackColor = Color.Transparent;
            searchWrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var gp = RoundedRect(new Rectangle(0, 0, searchWrapper.Width - 1, searchWrapper.Height - 1), 17))
                using (var pen = new Pen(Color.FromArgb(0, 188, 170), 1.5f))
                {
                    e.Graphics.FillPath(new SolidBrush(Color.FromArgb(235, 250, 248)), gp);
                    e.Graphics.DrawPath(pen, gp);
                }
            };

            Label searchIcon = new Label();
            searchIcon.Text = "🔍";
            searchIcon.Font = new Font("Segoe UI Emoji", 9);
            searchIcon.AutoSize = true;
            searchIcon.Location = new Point(8, 6);
            searchWrapper.Controls.Add(searchIcon);

            searchBox = new TextBox();
            searchBox.BorderStyle = BorderStyle.None;
            searchBox.Font = new Font("Segoe UI", 9.5f);
            searchBox.BackColor = Color.FromArgb(235, 250, 248);
            searchBox.ForeColor = Color.Gray;
            searchBox.Text = "Search translations…";
            searchBox.Size = new Size(210, 20);
            searchBox.Location = new Point(32, 8);
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search translations…")
                { searchBox.Text = ""; searchBox.ForeColor = Color.FromArgb(40, 40, 40); }
            };
            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                { searchBox.Text = "Search translations…"; searchBox.ForeColor = Color.Gray; }
            };
            searchBox.TextChanged += (s, e) => FilterRecords(searchBox.Text);
            searchWrapper.Controls.Add(searchBox);

            // ── SCROLL CONTAINER ───────────────────────
            Panel scrollContainer = new Panel();
            scrollContainer.Size = new Size(272, 390);
            scrollContainer.Location = new Point(14, 116);
            scrollContainer.BackColor = Color.Transparent;
            scrollContainer.AutoScroll = true;

            recordsPanel = new FlowLayoutPanel();
            recordsPanel.FlowDirection = FlowDirection.TopDown;
            recordsPanel.WrapContents = false;
            recordsPanel.AutoSize = true;
            recordsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            recordsPanel.Width = 256;
            recordsPanel.BackColor = Color.Transparent;
            scrollContainer.Controls.Add(recordsPanel);

            // ── EMPTY STATE ────────────────────────────
            lblEmpty = new Label();
            lblEmpty.Text = "🕐\r\n\r\nNo history yet.\r\nTranslate something!";
            lblEmpty.Font = new Font("Segoe UI", 10);
            lblEmpty.ForeColor = Color.FromArgb(180, 180, 180);
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Size = new Size(260, 160);
            lblEmpty.Location = new Point(20, 190);
            lblEmpty.Visible = false;

            // ── FOOTER ─────────────────────────────────
            Label footer = new Label();
            footer.Text = "© 2025 LinguaAI";
            footer.Font = new Font("Segoe UI", 8);
            footer.ForeColor = Color.Gray;
            footer.AutoSize = true;
            footer.Location = new Point(100, 510);

            // ── ASSEMBLE ───────────────────────────────
            phone.Controls.Add(header);
            phone.Controls.Add(searchWrapper);
            phone.Controls.Add(scrollContainer);
            phone.Controls.Add(lblEmpty);
            phone.Controls.Add(footer);
            this.Controls.Add(phone);
        }

        // ═══════════════════════════════════════════════
        //  LOAD & FILTER
        // ═══════════════════════════════════════════════
        private void LoadHistory()
        {
            allRecords = HistoryManager.Load();
            RenderRecords(allRecords);
        }

        private void FilterRecords(string query)
        {
            if (query == "Search translations…" || string.IsNullOrWhiteSpace(query))
            { RenderRecords(allRecords); return; }

            string q = query.ToLower();
            var filtered = allRecords.FindAll(r =>
                r.OriginalText.ToLower().Contains(q) ||
                r.TranslatedText.ToLower().Contains(q) ||
                r.FromLanguage.ToLower().Contains(q) ||
                r.ToLanguage.ToLower().Contains(q));

            RenderRecords(filtered);
        }

        private void RenderRecords(List<TranslationRecord> records)
        {
            recordsPanel.Controls.Clear();

            if (records == null || records.Count == 0)
            {
                lblEmpty.Visible = true;
                return;
            }

            lblEmpty.Visible = false;

            for (int i = 0; i < records.Count; i++)
            {
                int capturedIndex = allRecords.IndexOf(records[i]); // real index for delete
                recordsPanel.Controls.Add(CreateCard(records[i], capturedIndex));
            }
        }

        // ═══════════════════════════════════════════════
        //  CARD
        // ═══════════════════════════════════════════════
        private Panel CreateCard(TranslationRecord record, int realIndex)
        {
            Panel card = new Panel();
            card.Size = new Size(252, 100);
            card.Margin = new Padding(0, 0, 0, 8);
            card.BackColor = Color.Transparent;
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var gp = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 14))
                using (var pen = new Pen(Color.FromArgb(200, 235, 232), 1.5f))
                {
                    e.Graphics.FillPath(new SolidBrush(Color.FromArgb(245, 252, 251)), gp);
                    e.Graphics.DrawPath(pen, gp);
                }
            };

            // Language pill  e.g.  "English → Urdu"
            Label lblLang = new Label();
            lblLang.Text = $"{record.FromLanguage}  →  {record.ToLanguage}";
            lblLang.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            lblLang.ForeColor = teal;
            lblLang.AutoSize = true;
            lblLang.Location = new Point(10, 8);

            // Timestamp
            Label lblTime = new Label();
            lblTime.Text = record.Timestamp.ToString("MMM dd  hh:mm tt");
            lblTime.Font = new Font("Segoe UI", 7f);
            lblTime.ForeColor = Color.FromArgb(160, 160, 160);
            lblTime.AutoSize = true;
            lblTime.Location = new Point(10, 26);

            // Original text
            Label lblOriginal = new Label();
            lblOriginal.Text = Truncate(record.OriginalText, 32);
            lblOriginal.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            lblOriginal.ForeColor = Color.FromArgb(50, 50, 50);
            lblOriginal.AutoSize = false;
            lblOriginal.Size = new Size(200, 18);
            lblOriginal.Location = new Point(10, 46);

            // Translated text
            Label lblTranslated = new Label();
            lblTranslated.Text = Truncate(record.TranslatedText, 36);
            lblTranslated.Font = new Font("Segoe UI", 8.5f);
            lblTranslated.ForeColor = Color.FromArgb(0, 140, 120);
            lblTranslated.AutoSize = false;
            lblTranslated.Size = new Size(200, 18);
            lblTranslated.Location = new Point(10, 66);

            // 🗑 Delete button
            Label btnDelete = new Label();
            btnDelete.Text = "✕";
            btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnDelete.ForeColor = Color.FromArgb(200, 80, 80);
            btnDelete.AutoSize = true;
            btnDelete.Location = new Point(228, 6);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += (s, e) =>
            {
                HistoryManager.DeleteRecord(realIndex);
                LoadHistory(); // refresh
            };

            // Copy icon
            Label btnCopy = new Label();
            btnCopy.Text = "📋";
            btnCopy.Font = new Font("Segoe UI Emoji", 9);
            btnCopy.AutoSize = true;
            btnCopy.Location = new Point(228, 68);
            btnCopy.Cursor = Cursors.Hand;
            btnCopy.Click += (s, e) =>
            {
                Clipboard.SetText($"{record.OriginalText}\r\n→ {record.TranslatedText}");
                ToolTip tt = new ToolTip();
                tt.Show("Copied!", card, card.Width / 2, -20, 1200);
            };

            card.Controls.Add(lblLang);
            card.Controls.Add(lblTime);
            card.Controls.Add(lblOriginal);
            card.Controls.Add(lblTranslated);
            card.Controls.Add(btnDelete);
            card.Controls.Add(btnCopy);

            return card;
        }

        // ═══════════════════════════════════════════════
        //  CLEAR ALL
        // ═══════════════════════════════════════════════
        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            if (allRecords.Count == 0) return;

            var result = MessageBox.Show(
                "Clear all translation history?",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                HistoryManager.ClearAll();
                LoadHistory();
            }
        }

        // ═══════════════════════════════════════════════
        //  PAINT HELPERS
        // ═══════════════════════════════════════════════
        private void OuterForm_Paint(object sender, PaintEventArgs e)
        {
            using (var bg = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(210, 240, 238),
                Color.FromArgb(150, 210, 205),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(bg, this.ClientRectangle);
            }
        }

        private void PhonePanel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedRect(new Rectangle(0, 0, p.Width - 1, p.Height - 1), 40))
            {
                e.Graphics.FillPath(Brushes.White, path);
                e.Graphics.DrawPath(new Pen(Color.FromArgb(0, 188, 170), 2), path);
            }
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedRectTop(new Rectangle(0, 0, p.Width, p.Height + 40), 40))
            using (var br = new LinearGradientBrush(
                p.ClientRectangle,
                Color.FromArgb(0, 210, 190),
                Color.FromArgb(0, 170, 152),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillPath(br, path);
            }
        }

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

        private string Truncate(string text, int max) =>
            text != null && text.Length > max ? text.Substring(0, max) + "…" : text ?? "";

        private void FormHistory_Load(object sender, EventArgs e) { }
    }
}