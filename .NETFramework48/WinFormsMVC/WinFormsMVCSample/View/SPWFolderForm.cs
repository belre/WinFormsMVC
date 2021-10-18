using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WinFormsMVCSample.View
{
    public partial class SPWFolderForm : WinFormsMVC.View.BaseForm
    {
        public string RootDrive
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            protected set;
        }

        public SPWFolderForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void SPWFolderForm_Load(object sender, EventArgs e)
        {
            TreeNode node = new TreeNode()
            {
                Text = RootDrive,
                Tag = RootDrive
            };
            GetAllDirectories(node, RootDrive);

            treeView1.Nodes.Add(node);
            treeView1.ExpandAll();
        }

        private void GetAllDirectories(TreeNode root, string current_dir)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(current_dir))
                {
                    {
                        TreeNode child = new TreeNode(Path.GetFileName(dir));
                        child.Tag = dir;
                        root.Nodes.Add(child);
                        GetAllDirectories(child, dir);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var paths = Directory.GetFiles(e.Node.Tag.ToString());
            listBox1.Items.Clear();
            foreach (var path in paths)
            {
                listBox1.Items.Add(Path.GetFileName(path));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            FilePath = string.Format(@"{0}\{1}", treeView1.SelectedNode.Tag, listBox1.SelectedItem.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SPWFolderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        // https://stackoverflow.com/questions/21198668/treenode-selected-backcolor-while-treeview-not-focused
        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }
    }
}
