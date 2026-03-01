using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TLab1
{
    public class Notepad
    {

        private DocumentManager _documentManager;

        public Notepad(DocumentManager manager)
        {
            _documentManager = manager;
        }
       
        public int current_length = 0;
        public string file = "";
        public int tabCounter = 1;

        public bool CommitChanges()
        {
            _documentManager.AllDocuments.ToList().ForEach(d =>
            {
                if (d.IsModified)
                {
                    DialogResult dlg = MessageBox.Show($"Сохранить изменения в {d.TabPage.Text}?", "Предупреждение", MessageBoxButtons.YesNo);
                    if (dlg == DialogResult.Yes)
                    {
                        SaveTab(d);
                    }
                }
            });
            return true;
        }

        public DocInfo CreateNew()
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

                var docInfo = new DocInfo(tabPage, textBox, dataGrid, splitContainer);
                _documentManager.Register(tabPage, docInfo);
                return docInfo;
        }



        private void UpdateTabTitle(DocInfo docInfo)
        {

            if (docInfo.TabPage != null)
            {
                docInfo.TabPage.Text = Path.GetFileName(docInfo.FileName);
            }
        }

        public void SaveTab(DocInfo docInfo)
        {

            if (string.IsNullOrEmpty(docInfo.FileName)) 
            {
                SaveFileDialog saving = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "Сохранить как",
                    FileName = "Новый документ"


                };
                if (saving.ShowDialog() == DialogResult.OK)
                {
                    docInfo.FileName = saving.FileName;
                    StreamWriter writing = new StreamWriter(saving.FileName);
                    current_length = docInfo.TextBox.Text.Length;
                    writing.Write(docInfo.TextBox.Text);
                    writing.Close();

                    UpdateTabTitle(docInfo);
                }
            }
            else
            {
                StreamWriter writer = new StreamWriter(docInfo.FileName);
                current_length = docInfo.TextBox.Text.Length;
                writer.Write(docInfo.TextBox.Text);
                writer.Close();
                UpdateTabTitle(docInfo);
            }
        }

        public void OpenFile()
        {

            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Открыть",
                FileName = ""
            };

            if (open.ShowDialog() == DialogResult.OK)
            {
                DocInfo docInfo = CreateNew();
                
                docInfo.FileName = string.Format("{0}", Path.GetFileNameWithoutExtension(open.FileName));
                StreamReader reader = new StreamReader(open.FileName);
                docInfo.TextBox.Text = reader.ReadToEnd();
                reader.Close();

                UpdateTabTitle(docInfo);
            }
        }
        public void FileUndo(RichTextBox r)
        {
            if (r.CanUndo)
            {
                r.Undo();
            }
        }
        public void FileRedo(RichTextBox r)
        {
            if (r.CanRedo)
            {
                if (r.RedoActionName != "Delete")
                    r.Redo();
            }
        }
        public void FileCut(RichTextBox r)
        {
            if (r.SelectedText.Length > 0) r.Cut();
        }
        public void FileCopy(RichTextBox r)
        {
            if (r.SelectedText.Length > 0) r.Copy();
        }
        public void FilePaste(RichTextBox r)
        {
            r.Paste();
        }
        public void FileClear(RichTextBox r)
        {
            r.Clear();
        }
        public void FileSelectAll(RichTextBox r)
        {
            r.SelectAll();
        }
    }
}
