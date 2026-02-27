using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Komp_lab1
{
    internal class Correction
    {
        private RichTextBox richTextBox;

        public Correction(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }
        public void Cancel() {
            if (richTextBox.CanUndo) { richTextBox.Undo(); }
        }
        public void Repeat() {
            if (richTextBox.CanRedo) { richTextBox.Redo(); }
        }
        public void Cut() {
            if (!string.IsNullOrEmpty(richTextBox.SelectedText)) {
                richTextBox.Cut();
            }    
        }
        public void Copy() {
            if (!string.IsNullOrEmpty(richTextBox.SelectedText))
            {
                richTextBox.Copy();
            }
        }
        public void Paste() {
            if (Clipboard.ContainsText()) {
                richTextBox.Paste();
            }
        }
        public void Remove() {
            if (!string.IsNullOrEmpty(richTextBox.SelectedText))
            {
                richTextBox.SelectedText = string.Empty;
            }
        }
        public void Select_all() {
            richTextBox.SelectAll();
            richTextBox.Focus();
        }


    }
}
