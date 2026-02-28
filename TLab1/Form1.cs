using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TLab1
{
    public partial class Form1 : Form
    {
        private Dictionary<TabPage, DocInfo> _documents = new Dictionary<TabPage, DocInfo>();

        Notepad notepad = new Notepad();
        Information info = new Information();
        
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
            if (_documents == null || _documents.Count == 0)
            {
                notepad.CreateNewTab(_documents, tabControl1);
            }

            notepad.OpenFile(_documents, _documents[tabControl1.SelectedTab]);
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

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_documents == null || _documents.Count == 0)
            {
                notepad.CreateNewTab(_documents, tabControl1);
            }

            notepad.OpenFile(_documents, _documents[tabControl1.SelectedTab]);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            notepad.FileUndo(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            notepad.FileRedo(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FileUndo(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FileRedo(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            notepad.FileCopy(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            notepad.FileCut(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            notepad.FilePaste(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FileCut(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FileCopy(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FilePaste(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FileClear(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.FileSelectAll(_documents[tabControl1.SelectedTab].TextBox);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            info.about();
        }
    }
}
