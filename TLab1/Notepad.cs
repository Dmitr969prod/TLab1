using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLab1
{
    public class Notepad
    {
       
        public int current_length = 0;
        public string file = "";
        
       

        public void SaveTab(DocInfo docInfo)
        {
            if (string.IsNullOrEmpty(this.file)) 
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
                    file = saving.FileName;
                    StreamWriter writing = new StreamWriter(saving.FileName);
                    current_length = docInfo.TextBox.Text.Length;
                    writing.Write(docInfo.TextBox.Text);
                    writing.Close();
                }
            }
            else
            {
                StreamWriter writer = new StreamWriter(file);
                current_length = docInfo.TextBox.Text.Length;
                writer.Write(docInfo.TextBox.Text);
                writer.Close();
            }
        }
    }
}
