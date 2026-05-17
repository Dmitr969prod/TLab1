using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TLab1.Parser;
using System.Runtime.Remoting;
using TLab1.Semantic;

namespace TLab1
{
    public class Notepad
    {

        private DocumentManager _documentManager;
        private Scanner scanner = new Scanner();

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
            
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var textBoxLineNumbers = new System.Windows.Forms.TextBox();
            textBoxLineNumbers.Multiline = true;               
            textBoxLineNumbers.ReadOnly = true;                
            textBoxLineNumbers.ScrollBars = ScrollBars.None;   
            textBoxLineNumbers.BackColor = Color.LightGray;    
            textBoxLineNumbers.Width = 20;                    
            textBoxLineNumbers.Font = textBox.Font;       
            textBoxLineNumbers.Dock = DockStyle.Fill;          
            textBoxLineNumbers.BorderStyle = BorderStyle.None; 
            textBoxLineNumbers.TextAlign = HorizontalAlignment.Right;


            var splitContainer1 = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                IsSplitterFixed = true
                
            };
            splitContainer.Panel1.Controls.Add(splitContainer1);
            splitContainer1.Panel1.Controls.Add(textBoxLineNumbers);
            splitContainer1.SplitterDistance = 20;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Panel2.Controls.Add(textBox);
            splitContainer.Panel2.Controls.Add(dataGrid);
            tabPage.Controls.Add(splitContainer);

            

            
            var docInfo = new DocInfo(tabPage, textBox, dataGrid, splitContainer, textBoxLineNumbers);
            _documentManager.Register(tabPage, docInfo);
            textBox.VScroll += (s, e) => TextBox_VScroll(s, e, textBox, textBoxLineNumbers);
            textBoxLineNumbers.Text = GetLineNumbers(textBox);
            return docInfo;
        }

        private void TextBox_VScroll(object sender, EventArgs e, RichTextBox textBox, System.Windows.Forms.TextBox textBoxLineNumbers)
        {
            
            textBoxLineNumbers.Text = GetLineNumbers(textBox);
        }
        public void SetLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
        }
         public void ChangeLanguage(string lang, Form1 form1)
        {
            SetLanguage(lang);

            var form = new Form1();
            form.Show();
            form1.Hide();
        }
        public string GetLineNumbers(RichTextBox textBox)
        {
/*            var lines = textBox.Lines;
            if (lines.Length == 0)
                return "1";*/

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 100; i++)
                sb.AppendLine(i.ToString());

            return sb.ToString();
        }
        private void UpdateTabTitle(DocInfo docInfo)
        {

            if (docInfo.TabPage != null)
            {
                docInfo.TabPage.Text = Path.GetFileName(docInfo.FileName);
            }
        }

        public void StartProgram(DocInfo docInfo)
        {
            List<Token> tokens = scanner.Analyze(docInfo.TextBox.Text);

            docInfo.DataGrid.Columns.Clear();
            docInfo.DataGrid.Rows.Clear();

            docInfo.DataGrid.Columns.Add("Code", "Условный код");
            docInfo.DataGrid.Columns.Add("Type", "Тип лексемы");
            docInfo.DataGrid.Columns.Add("Leks", "Лексема");
            docInfo.DataGrid.Columns.Add("Place", "Местоположение");

            foreach (Token token in tokens)
            {
                docInfo.DataGrid.Rows.Add(
                    token.Code,
                    token.TypeName,
                    token.Value,
                    token.Location
                );
            }
        }

        public void StartProgram2(DocInfo docInfo)
        {
            List<Token> scanResult = scanner.Analyze(docInfo.TextBox.Text);

            docInfo.DataGrid.Columns.Clear();
            docInfo.DataGrid.Rows.Clear();

            var parser = new Analyzer();
            var result = parser.Parse(scanResult);


            List<ParseError> semanticErrors = new List<ParseError>();

            if (result.ErrorCount == 0)
            {
                SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer();
                semanticErrors = semanticAnalyzer.Analyze(result.AstRoot);
            }

            docInfo.DataGrid.Columns.Add("Leks", "Неверный фрагмент");
            docInfo.DataGrid.Columns.Add("Place", "Местоположение");
            docInfo.DataGrid.Columns.Add("Code", "Описание");

            foreach (ParseError error in semanticErrors)
            {
                int rowIndex = docInfo.DataGrid.Rows.Add(
                    error.InvalidFragment,
                    error.LocationText,
                    error.Message
                );

                docInfo.DataGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
            }

            
            if (semanticErrors.Count == 0)
            {
                AstTreeForm form = new AstTreeForm(result.AstRoot);
                form.ShowDialog();
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

        public void ChangeSize(DocInfo docInfo, float value)
        {
            docInfo.TextBox.Font = new System.Drawing.Font(docInfo.TextBox.Font.FontFamily, value);
            docInfo.DataGrid.Font = new System.Drawing.Font(docInfo.DataGrid.Font.FontFamily, value);
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
