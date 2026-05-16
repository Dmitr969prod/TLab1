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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TLab1
{


    public partial class Form1 : Form
    {

        private DocumentManager manager;
        private Notepad notepad;
        private Scanner scanner = new Scanner();
        Information info = new Information();
        


        public Form1()
        {
            InitializeComponent();

            scanner.Analyze("for (i in 0..9) ");

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
            manager = new DocumentManager(tabControl1);
            notepad = new Notepad(manager);
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;

        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                var doc = notepad.CreateNew();
                doc.FileName = file;
                
                doc.TextBox.Text = File.ReadAllText(file);
            }
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

        private void текстToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.ChangeLanguage("ru", this);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.ChangeLanguage("en", this);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.StartProgram2(doc);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

