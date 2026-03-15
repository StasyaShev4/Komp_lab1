using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Komp_lab1
{
    public partial class Form1 : Form
    {
        Correction correction;
        string filename;
        bool filesave = false;
        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
            correction = new Correction(richTextBox1);
            richTextBox1.VScroll += RichTextBox1_VScroll;
            richTextBox1.TextChanged += RichTextBox1_TextChanged;
            LineNumbers();
            DGInit();
        }
        private void DGInit() 
        {
            DataGridViewColumn[] columns = new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    Name = "ConditionalCode",
                    HeaderText = "Условный код"
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "TypeToken",
                    HeaderText = "Тип лексемы"
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "lexeme",
                    HeaderText = "Лексемы"
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "location",
                    HeaderText = "Местоположение"
                }
            };
            dataGridView1.Columns.AddRange(columns);
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
            richTextBox2.SelectionStart = richTextBox1.GetCharIndexFromPosition(new Point(0, 0));
            richTextBox2.ScrollToCaret();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            LineNumbers();
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
            //MessageBox.Show("Файл открыт");
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
                case TokenType.Separator:
                    return "Разделитель";
                case TokenType.Whitespace:
                    return "Разделитель (пробел)";
                case TokenType.Unknown:
                    return "Неизвестный";
                case TokenType.EndOfFile:
                    return "Конец файла";
                default:
                    return type.ToString();
            }
        }
        private int GetTokenCode(TokenType type, string value = "")
        {
            switch (type)
            {
                case TokenType.Keyword:
                    return 2; 
                case TokenType.Identifier:
                    return 3;
                case TokenType.Variable:
                    return 5;
                case TokenType.Separator:
                    if (value == ";") return 6;
                    if (value == "{") return 4;
                    if (value == "}") return 7;
                    return 0;
                case TokenType.Whitespace:
                    return 1;
                case TokenType.Unknown:
                    return 0;
                default:
                    return 0;
            }
        }
        private void RunLexicalAnalyzer()
        {
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

                foreach (Token token in tokens)
                {
                    string tokenType = GetTokenTypeString(token.Type);
                    int tokenCode = GetTokenCode(token.Type, token.Value);
                    string location = Position(token.Position, token.Value, token.Line);
                    dataGridView1.Rows.Add(tokenCode,tokenType, token.Value, location );
                }
                label1.Text = $"Найдено токенов: {tokens.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при лексическом анализе: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string Position(int pos, string value, int line) 
        {
            int val = value.Length;
            if (value == "(пробел)")
                val = 0;
            if (pos == 0)
                pos = 1;
            if (val == 1)
                val = 0;

            string str = $"строка {line}, {pos}-{pos + val}";
            return str;
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
