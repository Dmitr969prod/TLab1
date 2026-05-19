using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TLab1.AST;

namespace TLab1
{
    public class DocInfo
    {
        public AstNode LastAstRoot { get; set; }
        public bool LastAstIsValid { get; set; }
        public string FileName { get; set; }
        public RichTextBox TextBox { get; set; }
        public DataGridView DataGrid { get; set; }
        public SplitContainer SplitContainer { get; set; }
        public TabPage TabPage { get; set; }
        public TextBox TextBoxLineNumbers { get; set; }
        public bool IsModified { get; set; }

        private void TextBoxChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }
        public DocInfo(TabPage tab, RichTextBox textBox, DataGridView dataGrid, SplitContainer splitContainer, TextBox textBoxLineNumbers)
        {
            TabPage = tab;
            FileName = null;
            TextBox = textBox;
            DataGrid = dataGrid;
            SplitContainer = splitContainer;
            //RuchTextBox sa = (R)tabpage.Controls.Find("name", true)[0];
            textBox.TextChanged += TextBoxChanged;
            TextBoxLineNumbers = textBoxLineNumbers;
        }
    }
}
