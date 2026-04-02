using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Komp_lab1
{
    public partial class Form1 : Form
    {
        Correction correction;
        string filename;
        bool filesave = false;
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_VSCROLL = 0x115;
        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
            richTextBox2.ReadOnly = true;
            richTextBox2.Enabled = false;
            richTextBox2.ScrollBars = RichTextBoxScrollBars.None;
            correction = new Correction(richTextBox1);
            richTextBox1.VScroll += RichTextBox1_VScroll;
            richTextBox1.KeyDown += RichTextBox1_KeyDown;
            richTextBox1.PreviewKeyDown += RichTextBox1_PreviewKeyDown;
            richTextBox1.TextChanged += RichTextBox1_TextChanged;
            dataGridView1.CellClick += DataGridView1_CellClick;
            LineNumbers();
            DGInit();
        }
        private void DGInit() 
        {
            DataGridViewColumn[] columns = new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    Name = "Fragment",
                    HeaderText = "Фрагмент"
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Location",
                    HeaderText = "Местоположение"
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Message",
                    HeaderText = "Описание"
                }
            };

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.AddRange(columns);

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridView1.Columns["Fragment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Location"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 12);
        }
        private void LineNumbers()
        {
            try
            {
                richTextBox2.Clear();
                int lineCount = richTextBox1.Lines.Length;
                richTextBox2.SelectionAlignment = HorizontalAlignment.Center;

                for (int i = 0; i < lineCount; i++)
                {
                    richTextBox2.AppendText((i + 1).ToString() + "\n");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при нумерации строк: " + ex.Message);
            }
        }
        private void RichTextBox1_VScroll(object sender, EventArgs e)
        {
            int firstIndex = richTextBox1.GetCharIndexFromPosition(new Point(0, 0));
            int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex);

            int index = richTextBox2.GetFirstCharIndexFromLine(firstLine);

            if (index >= 0)
            {
                richTextBox2.SelectionStart = index;
                richTextBox2.ScrollToCaret();
            }
        }
        private void RichTextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true; 
            }
        }
        private void RichTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (e.Shift)
                {
                    int cursorPos = richTextBox1.SelectionStart;
                    if (cursorPos >= 4)
                    {
                        richTextBox1.SelectionStart = cursorPos - 4;
                        richTextBox1.SelectionLength = 4;
                        if (richTextBox1.SelectedText == "    ")
                        {
                            richTextBox1.SelectedText = "";
                        }
                        else
                        {
                            richTextBox1.SelectionStart = cursorPos;
                            richTextBox1.SelectionLength = 0;
                        }
                    }
                }
                else
                {
                    richTextBox1.SelectedText = "    ";
                }
            }
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            LineNumbers();
            RichTextBox1_VScroll(null, null);
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) 
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];

            if (row.Tag is SyntaxError err)
            {
                richTextBox1.Focus();
                richTextBox1.SelectionStart = err.Position;
                richTextBox1.SelectionLength = err.Fragment.Length;
                richTextBox1.ScrollToCaret();
            }
            else if (row.Tag is Token token)
            {
                richTextBox1.Focus();
                richTextBox1.SelectionStart = token.Position;
                richTextBox1.SelectionLength = token.Value.Length;
                richTextBox1.ScrollToCaret();
            }
        }
        private bool CheckingForChanges()
        {
            if (IsFileContentChanged())
            {
                string message;
                bool a = false;

                if (!(filename == null || filename == ""))
                {
                    message = $"Файл не сохранен. Вы хотите сохранить изменения в {filename}?";
                    a = true;
                }
                else
                {
                    message = "Файл не сохранен. Сохранить?";
                }

                DialogResult result = MessageBox.Show(
                    message,
                    "Сохранение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (a)
                        saveTSM_Click(null, null);
                    else
                        butt_save_file_Click(null, null);
                }

                if (result == DialogResult.Cancel)
                    return false;
            }

            return true;
        }
        private bool IsFileContentChanged() 
        {
            try 
            {
                if (filename == null || filename == "")
                {
                    bool a = !string.IsNullOrEmpty(richTextBox1.Text);
                    return a;
                }

                string fileContent = System.IO.File.ReadAllText(filename);
                return fileContent != richTextBox1.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");

                return true;
            }
        }
        private void butt_new_file_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            filesave = false;
            richTextBox1.Clear();
        }
        private void butt_save_file_Click(object sender, EventArgs e)
        {            
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filename = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(filename, richTextBox1.Text);
            MessageBox.Show("Файл сохранен");
        }
        private void butt_open_file_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filename = openFileDialog1.FileName;
            string fileText = System.IO.File.ReadAllText(filename);
            richTextBox1.Clear();
            richTextBox1.Text = fileText;
            filesave = false;
            label1.Text = filename;
        }
        private void createTSM_Click(object sender, EventArgs e)
        {
            butt_new_file_Click(sender, e);
        }
        private void openTSM_Click(object sender, EventArgs e)
        {
            butt_open_file_Click(sender, e);
        }
        private void SaveAsTSM_Click(object sender, EventArgs e)
        {
            butt_save_file_Click(sender, e);
        }
        private void saveTSM_Click(object sender, EventArgs e)
        {
            if (!(filename == null || filename == ""))
                System.IO.File.WriteAllText(filename, richTextBox1.Text);
            else
                butt_save_file_Click(sender, e);

        }
        private void CallingHelp_Click(object sender, EventArgs e)
        {
            string url = "https://docs.google.com/document/d/1qRyjYO0fhZQAdFL5vFgSz6H5AtVZflcTwC2yOMWvJPo/edit?usp=sharing";
            Process.Start(url);
        }
        private void AboutProgram_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/StasyaShev4/Komp_lab1/blob/master/README.md";
            Process.Start(url);
        }
        private void butt_help_Click(object sender, EventArgs e)
        {
            CallingHelp_Click(sender, e);
        }
        private void butt_about_program_Click(object sender, EventArgs e)
        {
            AboutProgram_Click(sender, e);
        }

        private void butt_cancel_Click(object sender, EventArgs e)
        {
            correction.Cancel();
        }
        private void butt_repeat_Click(object sender, EventArgs e)
        {
            correction.Repeat();
        }
        private void butt_copy_Click(object sender, EventArgs e)
        {
            correction.Copy();
        }
        private void butt_cut_Click(object sender, EventArgs e)
        {
            correction.Cut();
        }

        private void butt_paste_Click(object sender, EventArgs e)
        {
            correction.Paste();
        }

        private void cancelTSM_Click(object sender, EventArgs e)
        {
            correction.Cancel();
        }

        private void repeatTSM_Click(object sender, EventArgs e)
        {
            correction.Repeat();
        }

        private void cutTSM_Click(object sender, EventArgs e)
        {
            correction.Cut();
        }

        private void copyTSM_Click(object sender, EventArgs e)
        {
            correction.Copy();
        }

        private void pasteTSM_Click(object sender, EventArgs e)
        {
            correction.Paste();
        }

        private void removeTSM_Click(object sender, EventArgs e)
        {
            correction.Remove();
        }

        private void selectallTSM_Click(object sender, EventArgs e)
        {
            correction.Select_all();
        }
         private void Output_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CheckingForChanges())
                e.Cancel = true;
        }


        private string GetTokenTypeString(TokenType type) 
        {
            switch (type)
            {
                case TokenType.Keyword:
                    return "Ключевое слово";
                case TokenType.Identifier:
                    return "Идентификатор";
                case TokenType.Variable:
                    return "Переменная";
                case TokenType.Operator:
                    return "Оператор";
                case TokenType.Separator:
                    return "Разделитель";
                case TokenType.Whitespace:
                    return "Разделитель (пробел)";
                case TokenType.Unknown:
                    return "Неизвестный";
                default:
                    return type.ToString();
            }
        }
        private string GetLocation(int position, string value, int line)
        {
            int length = value.Length;
            if (value == " " || value == "\t" || value == "\n" || value == "\r" || value == "(пробел)")
                length = 1;

            if (length == 1)
                return $"строка {line}, позиция {position + 1}";
            else
                return $"строка {line}, {position + 1}-{position + length}";
        }
        private void RunLexicalAnalyzer()
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.DeselectAll();
            dataGridView1.Rows.Clear();
            try
            {
                string inputText = richTextBox1.Text;

                if (string.IsNullOrWhiteSpace(inputText))
                {
                    MessageBox.Show("Введите текст для анализа!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LexicalAnalyzer analyzer = new LexicalAnalyzer(inputText);
                List<Token> tokens = analyzer.Analize();
                dataGridView1.Rows.Clear();

                Parser parser = new Parser(tokens);
                parser.Parse();

                dataGridView1.Rows.Clear();

                foreach (var err in parser.Errors)
                {
                    int rowIndex = dataGridView1.Rows.Add(
                        err.Fragment,
                        $"строка {err.Line}, позиция {err.Position + 1}",
                        err.Message
                    );

                    dataGridView1.Rows[rowIndex].Tag = err;

                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }
                label2.Text = $"Найдено ошибок: {parser.Errors.Count}";

                foreach (Token token in tokens)
                {
                    if (token.Type == TokenType.Whitespace || token.Type == TokenType.EndOfFile)
                        continue;

                    string tokenTypeDesc = GetTokenTypeString(token.Type);
                    string location = GetLocation(token.Position, token.Value, token.Line);

                    int rowIndex = dataGridView1.Rows.Add(
                        token.Value,
                        location,
                        tokenTypeDesc
                    );
                    dataGridView1.Rows[rowIndex].Tag = token;

                    if (token.Type == TokenType.Unknown)
                    {
                        richTextBox1.SelectionStart = token.Position;
                        richTextBox1.SelectionLength = token.Value.Length;
                        richTextBox1.SelectionColor = Color.Red;
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MistyRose;
                    }
                }
                label3.Text = $"Найдено токенов: {tokens.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void butt_run_Click(object sender, EventArgs e)
        {
            RunLexicalAnalyzer();
        }

        private void RunTSM_Click(object sender, EventArgs e)
        {
            RunLexicalAnalyzer();
        }
    }
}
