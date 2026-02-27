using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLab1
{
    public class Notepad
    {
        public int tabCounter = 1;
        public void CreateNewTab(TabControl tabControl1)
        {
            var tabPage = new TabPage($"Новый документ{tabCounter++}");
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }
    }
}
