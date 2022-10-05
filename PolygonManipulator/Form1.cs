using System.Drawing.Drawing2D;
using System.Numerics;
namespace PolygonManipulator
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        List<Polygon> Polygons;
        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(Canvas.Size.Width, Canvas.Size.Height);
            Canvas.Image = DrawArea;
            using (Graphics g = Graphics.FromImage(DrawArea))
            {
                g.Clear(Color.LightBlue);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void canvas_Paint(object sender, PaintEventArgs e)
        {


            // Create graphics path object and add ellipse.
            //GraphicsPath graphPath = new GraphicsPath();
            //graphPath.AddEllipse(0, 0, 200, 100);

            //// Create pen.
            //Pen blackPen = new Pen(Color.Black, 3);

            //// Draw graphics path to screen.
            //e.Graphics.DrawPath(blackPen, graphPath);

        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int RADIUS = 10;
            using (Graphics g = Graphics.FromImage(DrawArea))
            {
                g.FillEllipse(Brushes.White, e.X - RADIUS, e.Y - RADIUS, RADIUS * 2, RADIUS * 2);
                g.DrawLine(Pens.Red, new Point(e.X, e.Y), new Point(e.X - 10, e.Y - 10));
                DrawArea.SetPixel(100, 100, Color.Red);
            }
            var a = Polygons.Last();

            Canvas.Refresh();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Polygons.Add(new Polygon());
        }
    }
}