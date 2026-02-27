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
            splitContainer.Panel1.Controls.Add( textBox );
            splitContainer.Panel2.Controls.Add( dataGrid );
            tabPage.Controls.Add( splitContainer );
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }
    }
}
