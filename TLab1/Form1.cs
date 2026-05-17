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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

namespace TLab1
{


    public partial class Form1 : Form
    {

        private DocumentManager manager;
        /*private Notepad notepad;*/
        private Scanner scanner = new Scanner();
        Information info = new Information();
        private RegexClass regexClass;
        Information info1 = new Information();



        public Form1()
        {
            InitializeComponent();

            scanner.Analyze("for (i in 0..9) ");


            regexClass = new RegexClass(
                    richTextBox2,
                    dataGridView2,
                    comboBox1,
                    label1
                );

        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
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

    

        private void текстToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

       /* private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.ChangeLanguage("ru", this);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notepad.ChangeLanguage("en", this);
        }*/

        /*private void toolStripButton10_Click(object sender, EventArgs e)
        {
            var doc = manager.GetDocument(tabControl1.SelectedTab);
            notepad.StartProgram2(doc);
        }*/

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonRegexSearch_Click_1(object sender, EventArgs e)
        {
          

            regexClass.PerformSearch();
        }

        private void buttonRegexClear_Click_1(object sender, EventArgs e)
        {
            regexClass.ClearResults();
        }
    }
}

