namespace Komp_lab1
{
    partial class FormAST
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewAST = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewAST
            // 
            this.treeViewAST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAST.Location = new System.Drawing.Point(0, 0);
            this.treeViewAST.Name = "treeViewAST";
            this.treeViewAST.Size = new System.Drawing.Size(800, 450);
            this.treeViewAST.TabIndex = 0;
            // 
            // FormAST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.treeViewAST);
            this.Name = "FormAST";
            this.Text = "FormAST";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewAST;
    }
}