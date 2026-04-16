using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLab1
{
    public class DocumentManager
    {
        private readonly TabControl _tabs;
        private readonly Dictionary<TabPage, DocInfo> _documents;

        public DocumentManager(TabControl tabs)
        {
            _tabs = tabs;
            _documents = new Dictionary<TabPage, DocInfo>();
        }

        public DocInfo Current
        {
            get
            {
                if (_tabs.SelectedTab == null)
                {
                    return null;
                }
                DocInfo docInfo;
                _documents.TryGetValue(_tabs.SelectedTab, out docInfo);
                return docInfo;
            }
        }
        public void Register(TabPage tab, DocInfo docInfo)
        {
            _documents[tab] = docInfo;
            _tabs.TabPages.Add(tab);
            _tabs.SelectedTab = tab;
        }
        
        public IEnumerable<DocInfo> AllDocuments
        {
            get {return _documents.Values;}
        }

        public DocInfo GetDocument(TabPage tab)
        {
            DocInfo docInfo;
            _documents.TryGetValue(tab, out docInfo);
            return docInfo;
        }

        
    }
}
