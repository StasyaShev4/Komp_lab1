using System;
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
        }
        private void LineNumbers()
        {
            try
            {
                Point pt = new Point(0, 0);

                int firstIndex = richTextBox1.GetCharIndexFromPosition(pt);
                int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex);
                pt.X = ClientRectangle.Width;
                pt.Y = ClientRectangle.Height;

                int lastIndex = richTextBox1.GetCharIndexFromPosition(pt);
                int lastLine = richTextBox1.GetLineFromCharIndex(lastIndex);

                richTextBox2.Text = "";
                richTextBox2.SelectionAlignment = HorizontalAlignment.Center;

                for (int i = firstLine; i <= lastLine + 1; i++)
                {
                    richTextBox2.Text += (i + 1).ToString() + "\n";
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при нумерации строк: " + ex.Message);
            }
        }
        private void RichTextBox1_VScroll(object sender, EventArgs e)
        {
            LineNumbers();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            LineNumbers();
        }
        private void CheckingForChanges(object sender, EventArgs e) 
        {
            if (IsFileContentChanged())
            {
                filesave = true;
            }

            if (filesave)
            {
                string message, caption = "";
                bool a = false;
                if (!(filename == null || filename == ""))
                {
                    message = string.Format("Файл не сохранен. Вы хотите сохранить изменения в {0}", filename);
                    a = true;
                }
                else
                {
                    message = "Файл не сохранен. Сохранить?";
                    a = false;
                }

                DialogResult result = MessageBox.Show(message, caption,
                                          MessageBoxButtons.YesNoCancel,
                                          MessageBoxIcon.Question,
                                          MessageBoxDefaultButton.Button1);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (a)
                            saveTSM_Click(sender, e);
                        else
                            butt_save_file_Click(sender, e);
                        Application.Exit();
                        break;
                    case DialogResult.No:
                        Application.Exit();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else
            {
                Application.Exit();
            }
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
        private void butt_help_Click(object sender, EventArgs e)
        {
            CallingHelp_Click(sender, e);
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

        private void OutputTSM_Click(object sender, EventArgs e)
        {
            CheckingForChanges(sender, e);
        }

        private void Output_Click(object sender, EventArgs e)
        {
            CheckingForChanges(sender, e);
        }

       
    }
}
