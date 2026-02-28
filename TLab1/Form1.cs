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
        
        public Form1()
        {
            InitializeComponent();
        }
       

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            notepad.CreateNewTab(_documents, tabControl1);
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.CreateNewTab(_documents, tabControl1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            notepad.SaveTab(_documents, _documents[tabControl1.SelectedTab]);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.SaveTab(_documents, _documents[tabControl1.SelectedTab]);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.SaveTab(_documents, _documents[tabControl1.SelectedTab]);
        }
    }
}
