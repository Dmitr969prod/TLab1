using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLab1
{
    public class DocInfo
    {
        public string FileName { get; set; }
        public RichTextBox TextBox { get; set; }
        public DataGridView DataGrid { get; set; }
        public SplitContainer SplitContainer { get; set; }
        public bool IsModified { get; set; }

        private void TextBoxChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }
        public DocInfo(RichTextBox textBox, DataGridView dataGrid, SplitContainer splitContainer)
        {
            FileName = null;
            TextBox = textBox;
            DataGrid = dataGrid;
            SplitContainer = splitContainer;

            textBox.TextChanged += TextBoxChanged;
        }
    }
}
