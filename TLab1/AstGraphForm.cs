using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TLab1.AST;

namespace TLab1
{
    public partial class AstGraphForm : Form
    {
        private readonly AstNode _root;

        private const int NodeWidth = 180;
        private const int NodeHeight = 50;
        private const int HorizontalGap = 50;
        private const int VerticalGap = 90;

        private readonly Dictionary<AstNode, Rectangle> _nodeBounds =
            new Dictionary<AstNode, Rectangle>();

        public AstGraphForm(AstNode root)
        {
            InitializeComponent();

            _root = root;

            Text = "Графическая визуализация AST";
            Width = 1100;
            Height = 700;
            BackColor = Color.White;
            AutoScroll = true;
            DoubleBuffered = true;

            Paint += AstGraphForm_Paint;
        }

        private void AstGraphForm_Load(object sender, EventArgs e)
        {
        }

        private void AstGraphForm_Paint(object sender, PaintEventArgs e)
        {
            if (_root == null)
                return;

            e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
            e.Graphics.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            _nodeBounds.Clear();

            int startX = 50;
            int startY = 40;

            int totalWidth = CalculateLayout(_root, startX, startY);

            AutoScrollMinSize = new Size(
                Math.Max(totalWidth + 100, ClientSize.Width),
                GetTreeDepth(_root) * (NodeHeight + VerticalGap) + 100
            );

            using (Pen edgePen = new Pen(Color.Black, 2))
            using (Pen nodePen = new Pen(Color.DarkBlue, 2))
            using (Brush nodeBrush = new SolidBrush(Color.LightBlue))
            using (Brush textBrush = new SolidBrush(Color.Black))
            using (Font font = new Font("Arial", 9))
            {
                DrawEdges(e.Graphics, _root, edgePen);
                DrawNodes(e.Graphics, _root, nodePen, nodeBrush, textBrush, font);
            }
        }

        private int CalculateLayout(AstNode node, int x, int y)
        {
            List<AstNode> children = node.GetChildren();

            if (children == null || children.Count == 0)
            {
                _nodeBounds[node] = new Rectangle(x, y, NodeWidth, NodeHeight);
                return NodeWidth + HorizontalGap;
            }

            int currentX = x;
            int totalWidth = 0;

            foreach (AstNode child in children)
            {
                int childWidth = CalculateLayout(child, currentX, y + VerticalGap);
                currentX += childWidth;
                totalWidth += childWidth;
            }

            int nodeX = x + totalWidth / 2 - NodeWidth / 2;

            _nodeBounds[node] = new Rectangle(
                nodeX,
                y,
                NodeWidth,
                NodeHeight
            );

            return Math.Max(totalWidth, NodeWidth + HorizontalGap);
        }

        private void DrawEdges(Graphics g, AstNode node, Pen pen)
        {
            if (!_nodeBounds.ContainsKey(node))
                return;

            Rectangle parentRect = _nodeBounds[node];

            foreach (AstNode child in node.GetChildren())
            {
                if (!_nodeBounds.ContainsKey(child))
                    continue;

                Rectangle childRect = _nodeBounds[child];

                Point parentPoint = new Point(
                    parentRect.Left + parentRect.Width / 2,
                    parentRect.Bottom
                );

                Point childPoint = new Point(
                    childRect.Left + childRect.Width / 2,
                    childRect.Top
                );

                g.DrawLine(pen, parentPoint, childPoint);

                DrawEdges(g, child, pen);
            }
        }

        private void DrawNodes(
            Graphics g,
            AstNode node,
            Pen nodePen,
            Brush nodeBrush,
            Brush textBrush,
            Font font)
        {
            if (!_nodeBounds.ContainsKey(node))
                return;

            Rectangle rect = _nodeBounds[node];

            g.FillRectangle(nodeBrush, rect);
            g.DrawRectangle(nodePen, rect);

            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(
                node.NodeName,
                font,
                textBrush,
                rect,
                format
            );

            foreach (AstNode child in node.GetChildren())
            {
                DrawNodes(g, child, nodePen, nodeBrush, textBrush, font);
            }
        }

        private int GetTreeDepth(AstNode node)
        {
            if (node == null)
                return 0;

            List<AstNode> children = node.GetChildren();

            if (children == null || children.Count == 0)
                return 1;

            int maxDepth = 0;

            foreach (AstNode child in children)
            {
                int depth = GetTreeDepth(child);

                if (depth > maxDepth)
                    maxDepth = depth;
            }

            return maxDepth + 1;
        }
    }
}