using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AI_Language_Translator
{
    public partial class FormTranslator : Form
    {
        Panel phone;
        Panel header;

        // Controls
        TextBox txtInput;
        TextBox txtOutput;
        ComboBox cmbFrom;
        ComboBox cmbTo;
        Button btnTranslate;

        public FormTranslator()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Translator";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Paint += OuterForm_Paint;

            Color teal = Color.FromArgb(0, 188, 170);
            Color white = Color.White;

            // PHONE PANEL
            phone = new Panel();
            phone.Size = new Size(280, 500);
            phone.BackColor = Color.Transparent;
            phone.Paint += PhonePanel_Paint;

            // HEADER
            header = new Panel();
            header.Size = new Size(280, 90);
            header.BackColor = Color.Transparent;
            header.Paint += HeaderPanel_Paint;

            Label lblTitle = new Label();
            lblTitle.Text = "Translator";
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = white;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(85, 30);

            // BACK BUTTON
            Button btnBack = new Button();
            btnBack.Text = "←";
            btnBack.Size = new Size(40, 30);
            btnBack.Location = new Point(10, 25);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.BackColor = Color.Transparent;
            btnBack.ForeColor = white;
            btnBack.Click += (s, e) =>
            {
                FormMenu m = new FormMenu();
                m.Show();
                this.Hide();
            };

            header.Controls.Add(lblTitle);
            header.Controls.Add(btnBack);

            // INPUT
            txtInput = new TextBox();
            txtInput.Multiline = true;
            txtInput.Size = new Size(220, 80);
            txtInput.Location = new Point(30, 110);
            txtInput.Font = new Font("Segoe UI", 10f);
            txtInput.BorderStyle = BorderStyle.None;
            txtInput.BackColor = Color.FromArgb(240, 250, 249);

            Panel inputPanel = CreateRoundedPanel(txtInput);

            // LANGUAGES
            cmbFrom = new ComboBox();
            cmbFrom.Items.AddRange(new string[] { "English", "Urdu", "Arabic", "French", "Spanish", "German", "Chinese", "Japanese" });
            cmbFrom.SelectedIndex = 0;
            cmbFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFrom.Size = new Size(100, 30);
            cmbFrom.Location = new Point(30, 210);

            cmbTo = new ComboBox();
            cmbTo.Items.AddRange(new string[] { "Urdu", "English", "Arabic", "French", "Spanish", "German", "Chinese", "Japanese" });
            cmbTo.SelectedIndex = 1;
            cmbTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTo.Size = new Size(100, 30);
            cmbTo.Location = new Point(150, 210);

            // SWAP BUTTON
            Button btnSwap = new Button();
            btnSwap.Text = "⇄";
            btnSwap.Size = new Size(40, 30);
            btnSwap.Location = new Point(120, 210);
            btnSwap.FlatStyle = FlatStyle.Flat;
            btnSwap.FlatAppearance.BorderSize = 0;
            btnSwap.BackColor = Color.FromArgb(220, 242, 240);
            btnSwap.Click += (s, e) =>
            {
                int tempIndex = cmbFrom.SelectedIndex;
                cmbFrom.SelectedIndex = cmbTo.SelectedIndex;
                cmbTo.SelectedIndex = tempIndex;
            };

            // TRANSLATE BUTTON
            btnTranslate = new Button();
            btnTranslate.Text = "Translate";
            btnTranslate.Size = new Size(200, 45);
            btnTranslate.Location = new Point(40, 260);
            btnTranslate.BackColor = teal;
            btnTranslate.ForeColor = white;
            btnTranslate.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnTranslate.FlatStyle = FlatStyle.Flat;
            btnTranslate.FlatAppearance.BorderSize = 0;
            btnTranslate.Paint += BtnRound_Paint;
            btnTranslate.Click += async (s, e) => await TranslateTextAsync();

            // OUTPUT
            txtOutput = new TextBox();
            txtOutput.Multiline = true;
            txtOutput.Size = new Size(220, 80);
            txtOutput.Location = new Point(30, 320);
            txtOutput.Font = new Font("Segoe UI", 10f);
            txtOutput.BorderStyle = BorderStyle.None;
            txtOutput.BackColor = Color.FromArgb(240, 250, 249);
            txtOutput.ReadOnly = true;

            Panel outputPanel = CreateRoundedPanel(txtOutput);

            // ADD CONTROLS
            phone.Controls.Add(header);
            phone.Controls.Add(inputPanel);
            phone.Controls.Add(cmbFrom);
            phone.Controls.Add(cmbTo);
            phone.Controls.Add(btnSwap);
            phone.Controls.Add(btnTranslate);
            phone.Controls.Add(outputPanel);

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

        // PHONE PANEL
        private void PhonePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, phone.Width - 1, phone.Height - 1), 40))
            {
                g.FillPath(Brushes.White, path);
                g.DrawPath(new Pen(Color.FromArgb(0, 188, 170), 2f), path);
            }
        }

        // HEADER PANEL
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int radius = 40;
            int d = radius * 2;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(pnl.Width - d, 0, d, d, 270, 90);
                path.AddLine(pnl.Width, pnl.Height, 0, pnl.Height);
                path.CloseFigure();

                using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 188, 170)))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        // ROUNDED BUTTON
        private void BtnRound_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), btn.Height / 2))
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

        // ROUNDED PANEL
        private Panel CreateRoundedPanel(Control inner)
        {
            Panel panel = new Panel();
            panel.Size = inner.Size;
            panel.Location = inner.Location;
            panel.BackColor = Color.Transparent;

            panel.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 15))
                {
                    g.FillPath(new SolidBrush(Color.FromArgb(240, 250, 249)), path);
                    g.DrawPath(new Pen(Color.FromArgb(200, 230, 225)), path);
                }
            };

            inner.Location = new Point(10, 10);
            panel.Controls.Add(inner);

            return panel;
        }

        // HELPER FOR ROUNDED RECTANGLE
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

        // ✅ TRANSLATE + SAVE HISTORY
        private async Task TranslateTextAsync()
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter text to translate.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Store language names BEFORE async call (UI thread safe)
            string fromLangName = cmbFrom.SelectedItem.ToString();
            string toLangName = cmbTo.SelectedItem.ToString();
            string fromLang = GetLanguageCode(fromLangName);
            string toLang = GetLanguageCode(toLangName);

            txtOutput.Text = "Translating...";
            btnTranslate.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                    string encodedInput = Uri.EscapeDataString(input);
                    string url = $"https://api.mymemory.translated.net/get?q={encodedInput}&langpair={fromLang}|{toLang}";

                    HttpResponseMessage response = await client.GetAsync(url);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception($"API Error: {response.StatusCode}");

                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        if (doc.RootElement.TryGetProperty("responseData", out JsonElement responseData) &&
                            responseData.TryGetProperty("translatedText", out JsonElement translatedElement))
                        {
                            string translated = translatedElement.GetString();

                            if (string.IsNullOrEmpty(translated) || translated == input)
                            {
                                txtOutput.Text = "Translation not available for this language pair.";
                            }
                            else
                            {
                                txtOutput.Text = translated.Trim();

                                // ✅ SAVE TO HISTORY
                                HistoryManager.AddRecord(
                                    input,
                                    translated.Trim(),
                                    fromLangName,
                                    toLangName
                                );
                            }
                        }
                        else
                        {
                            txtOutput.Text = "Unable to parse translation response.";
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network error: {ex.Message}\n\nPlease check your internet connection.",
                    "Translation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOutput.Text = "";
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Translation request timed out. Please try again.",
                    "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOutput.Text = "";
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Failed to parse API response: {ex.Message}",
                    "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOutput.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Translation failed: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOutput.Text = "";
            }
            finally
            {
                btnTranslate.Enabled = true;
            }
        }

        private string GetLanguageCode(string lang)
        {
            switch (lang)
            {
                case "English": return "en";
                case "Urdu": return "ur";
                case "Arabic": return "ar";
                case "French": return "fr";
                case "Spanish": return "es";
                case "German": return "de";
                case "Chinese": return "zh";
                case "Japanese": return "ja";
                default: return "en";
            }
        }

        private void FormTranslator_Load(object sender, EventArgs e) { }
    }
}