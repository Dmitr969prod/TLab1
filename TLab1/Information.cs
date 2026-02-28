using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLab1
{
    public class Information
    {
        public void about()
        {
            MessageBox.Show("Легкий и быстрый текстовый редактор для повседневных задач");
        }

        public void AboutInstructions()
        {
            string htmlFilePath = @"https://github.com/Dmitr969prod/TLab1/blob/develop/TLab1/README.md";
            Process.Start("explorer.exe", htmlFilePath);
        }
    }
}
