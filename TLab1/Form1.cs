using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLab1
{
    public partial class Form1 : Form
    {
        private Dictionary<TabPage, DocInfo> _documents = new Dictionary<TabPage, DocInfo>();

        Notepad notepad = new Notepad();
        public int tabCounter = 1;
        public Form1()
        {
            InitializeComponent();
        }
        public void CreateNewTab(TabControl tabControl1)
        {
            var tabPage = new TabPage($"Новый документ{tabCounter++}");

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
            };
            var textBox = new RichTextBox
            {
                Dock = DockStyle.Fill
            };
            var dataGrid = new DataGridView
            {
                Dock = DockStyle.Fill
            };
            splitContainer.Panel1.Controls.Add(textBox);
            splitContainer.Panel2.Controls.Add(dataGrid);
            tabPage.Controls.Add(splitContainer);

            var docInfo = new DocInfo(textBox, dataGrid, splitContainer);
            _documents[tabPage] = docInfo;

            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CreateNewTab(tabControl1);
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTab(tabControl1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            notepad.SaveTab(_documents[tabControl1.SelectedTab]);
        }
    }
}
