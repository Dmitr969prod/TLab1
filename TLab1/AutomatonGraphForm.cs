using System.Drawing;
using System.Windows.Forms;

namespace TLab1
{
    public class AutomatonGraphForm : Form
    {
        public AutomatonGraphForm()
        {
            Text = "Граф автомата для автомобильного номера";
            Width = 1100;
            Height = 420;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 2);
            Pen finalPen = new Pen(Color.Black, 4);
            Font font = new Font("Arial", 10);
            Font stateFont = new Font("Arial", 11, FontStyle.Bold);
            Brush brush = Brushes.White;
            Brush textBrush = Brushes.Black;

            int y = 170;
            int r = 28;
            int startX = 70;
            int step = 100;

            Point[] states = new Point[10];

            for (int i = 0; i < states.Length; i++)
            {
                states[i] = new Point(startX + i * step, y);
            }

            DrawStartArrow(g, pen, states[0], r);

            for (int i = 0; i < states.Length; i++)
            {
                bool isFinal = i == 8 || i == 9;
                DrawState(g, pen, finalPen, brush, textBrush, stateFont, states[i], r, "q" + i, isFinal);
            }

            DrawTransition(g, pen, font, states[0], states[1], r, "буква");
            DrawTransition(g, pen, font, states[1], states[2], r, "цифра");
            DrawTransition(g, pen, font, states[2], states[3], r, "цифра");
            DrawTransition(g, pen, font, states[3], states[4], r, "цифра");
            DrawTransition(g, pen, font, states[4], states[5], r, "буква");
            DrawTransition(g, pen, font, states[5], states[6], r, "буква");
            DrawTransition(g, pen, font, states[6], states[7], r, "цифра");
            DrawTransition(g, pen, font, states[7], states[8], r, "цифра");
            DrawTransition(g, pen, font, states[8], states[9], r, "цифра");

            g.DrawString("Допускающие состояния: q8, q9", font, textBrush, 70, 270);
            g.DrawString("Формат: [АВЕКМНОРСТУХ] + 3 цифры + 2 буквы + 2 или 3 цифры региона", font, textBrush, 70, 300);
            g.DrawString("Примеры: А123ВС77, М456ОР199", font, textBrush, 70, 330);
        }

        private void DrawState(
            Graphics g,
            Pen pen,
            Pen finalPen,
            Brush fillBrush,
            Brush textBrush,
            Font font,
            Point center,
            int r,
            string name,
            bool isFinal)
        {
            Rectangle rect = new Rectangle(center.X - r, center.Y - r, r * 2, r * 2);

            g.FillEllipse(fillBrush, rect);
            g.DrawEllipse(pen, rect);

            if (isFinal)
            {
                Rectangle inner = new Rectangle(center.X - r + 5, center.Y - r + 5, r * 2 - 10, r * 2 - 10);
                g.DrawEllipse(finalPen, inner);
            }

            SizeF size = g.MeasureString(name, font);
            g.DrawString(name, font, textBrush, center.X - size.Width / 2, center.Y - size.Height / 2);
        }

        private void DrawTransition(
            Graphics g,
            Pen pen,
            Font font,
            Point from,
            Point to,
            int r,
            string label)
        {
            int x1 = from.X + r;
            int y1 = from.Y;
            int x2 = to.X - r;
            int y2 = to.Y;

            g.DrawLine(pen, x1, y1, x2, y2);
            DrawArrowHead(g, pen, x2, y2);

            int labelX = (x1 + x2) / 2 - 18;
            int labelY = y1 - 28;
            g.DrawString(label, font, Brushes.Black, labelX, labelY);
        }

        private void DrawStartArrow(Graphics g, Pen pen, Point q0, int r)
        {
            int x1 = q0.X - 55;
            int y1 = q0.Y;
            int x2 = q0.X - r;
            int y2 = q0.Y;

            g.DrawLine(pen, x1, y1, x2, y2);
            DrawArrowHead(g, pen, x2, y2);
        }

        private void DrawArrowHead(Graphics g, Pen pen, int x, int y)
        {
            Point p1 = new Point(x, y);
            Point p2 = new Point(x - 10, y - 5);
            Point p3 = new Point(x - 10, y + 5);

            g.FillPolygon(Brushes.Black, new Point[] { p1, p2, p3 });
        }
    }
}