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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TLab1
{
    public partial class Form1 : Form
    {

        private DocumentManager manager;
        private Notepad notepad;
        Information info = new Information();



        public Form1()
        {
            InitializeComponent();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
            manager = new DocumentManager(tabControl1);
            notepad = new Notepad(manager);

        }



        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            if (notepad.CommitChanges())
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            notepad.CreateNew();

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.CreateNew();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            notepad.OpenFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.SaveTab(doc);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.SaveTab(doc);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.SaveTab(doc);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            notepad.OpenFile();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileUndo(doc.TextBox);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileRedo(doc.TextBox);
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileUndo(doc.TextBox);
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileRedo(doc.TextBox);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileCopy(doc.TextBox);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileCut(doc.TextBox);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FilePaste(doc.TextBox);
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileCut(doc.TextBox);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileCopy(doc.TextBox);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FilePaste(doc.TextBox);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileClear(doc.TextBox);
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.FileSelectAll(doc.TextBox);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            info.about();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            info.about();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            info.AboutInstructions();
        }

        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            info.AboutInstructions();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.ChangeSize(doc, (float)numericUpDown1.Value);
        }


    }
}
