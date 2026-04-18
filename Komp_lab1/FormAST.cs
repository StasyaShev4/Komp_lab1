using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Komp_lab1
{
    partial class FormAST : Form
    {
        public FormAST()
        {
            InitializeComponent();
        }
        public void LoadImage(string path)
        {
            if (File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    pictureBox1.Image = Image.FromStream(stream);
                }
            }
        }

        public void BuildTree(List<StructDeclNode> structs)
        {
            treeViewAST.Nodes.Clear();

            foreach (var s in structs)
            {
                TreeNode structNode = new TreeNode($"Struct: {s.Name}");

                TreeNode fieldsNode = new TreeNode("Fields");

                foreach (var f in s.Fields)
                {
                    TreeNode fieldNode = new TreeNode("Field");

                    fieldNode.Nodes.Add($"Name: {f.Name}");
                    fieldNode.Nodes.Add($"Type: {f.Type}");

                    if (f.Value != null)
                        fieldNode.Nodes.Add($"Value: {f.Value}");

                    fieldsNode.Nodes.Add(fieldNode);
                }

                structNode.Nodes.Add(fieldsNode);
                treeViewAST.Nodes.Add(structNode);
            }

            treeViewAST.ExpandAll();
        }
    }
}
