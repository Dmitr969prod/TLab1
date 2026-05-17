using System.Windows.Forms;
using TLab1.AST;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TLab1
{
    public partial class AstTreeForm : Form
    {
        private AstNode _root;

        public AstTreeForm(AstNode root)
        {
            InitializeComponent();
            _root = root;
        }

        private void AstTreeForm_Load(object sender, System.EventArgs e)
        {
            treeView1.Nodes.Clear();

            if (_root != null)
            {
                BuildTree(_root, null);
                treeView1.ExpandAll();
            }
        }

        private void BuildTree(AstNode node, TreeNode parent)
        {
            if (node == null)
                return;

            TreeNode treeNode = new TreeNode(node.NodeName);

            if (parent == null)
            {
                treeView1.Nodes.Add(treeNode);
            }
            else
            {
                parent.Nodes.Add(treeNode);
            }

            foreach (AstNode child in node.GetChildren())
            {
                BuildTree(child, treeNode);
            }
        }
    }
}