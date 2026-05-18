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
        private Notepad notepad;
        private RegexSearchClass regexSearch;

        //private RichTextBox richTextBoxEditor;
        //private DataGridView dataGridViewResults;
        /*private ComboBox comboBoxSearchType;
        private Button buttonSearch;*/

        private DocumentManager manager;
        //private Notepad notepad;
        private Scanner scanner = new Scanner();
        Information info = new Information();

        //private RichTextBox richTextBoxEditor;
        //private DataGridView dataGridViewResults;
        private ComboBox comboBoxRegex;
        //private Label labelRegex;
        //private Label labelCount;
        //private Button buttonRegexSearch;
        //private Button buttonAutomatonSearch;

        public Form1()
        {
            InitializeComponent();

            regexSearch = new RegexSearchClass(
    richTextBoxEditor,
    dataGridViewResults,
    comboBoxSearchType,
    labelCount,
    labelRegex);
        }
        private void comboBoxRegex_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRegexLabel();
        }

        private void UpdateRegexLabel()
        {
            if (comboBoxRegex.SelectedItem == null)
                return;

            switch (comboBoxRegex.SelectedItem.ToString())
            {
                case "Цитаты":
                    labelRegex.Text = "Регулярное выражение: /'[^']*'/";
                    break;

                case "Идентификаторы":
                    labelRegex.Text = "Регулярное выражение: /\\b[a-zA-Z$_][a-zA-Z0-9]*\\b/";
                    break;

                case "Автомобильные номера":
                    labelRegex.Text = "Регулярное выражение: /^([АВЕКМНОРСТУХ])(\\d{3})([АВЕКМНОРСТУХ]{2})(\\d{2,3})$/";
                    break;
            }
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

        /*private void toolStripButton4_Click(object sender, EventArgs e)
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
        }*/

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            notepad.OpenFile();
        }

        /*private void toolStripButton5_Click(object sender, EventArgs e)
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
        }*/

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

       /* private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.ChangeSize(doc, (float)numericUpDown1.Value);
        }*/

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

        /*private void toolStripButton10_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.StartProgram2(doc);
        }*/

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click_1(object sender, EventArgs e)
        {
            regexSearch.PerformRegexSearch();
        }

        private void buttonAutomatonSearch_Click_1(object sender, EventArgs e)
        {
            regexSearch.PerformAutomatonSearch();
        }
    }
}

