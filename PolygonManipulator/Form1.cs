namespace PolygonManipulator
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        List<Polygon> Polygons;
        Polygon CurrentPolygon;
        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(Canvas.Size.Width, Canvas.Size.Height);
            Canvas.Image = DrawArea;
            Polygons = new List<Polygon>();
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
            int RADIUS = 5;
            //using (Graphics g = Graphics.FromImage(DrawArea))
            //{
            //    g.FillEllipse(Brushes.White, e.X - RADIUS, e.Y - RADIUS, RADIUS * 2, RADIUS * 2);
            //    //g.DrawLine(Pens.Red, new Point(e.X, e.Y), new Point(e.X - 10, e.Y - 10));
            //    //DrawArea.SetPixel(100, 100, Color.Red);
            //}
            Graphics g = Graphics.FromImage(DrawArea);


            if (CurrentPolygon.Points.Count() == 0)
            {
                CurrentPolygon.AddPoint(e.X, e.Y);
                g.FillEllipse(Brushes.Black, e.X - RADIUS, e.Y - RADIUS, RADIUS * 2, RADIUS * 2);
            }
            else
            {
                var LastPoint = CurrentPolygon.Points.Last();
                int res = CurrentPolygon.AddPoint(e.X, e.Y);

                if (res == 0)
                {
                    g.DrawLine(Pens.Red, new Point(LastPoint.X, LastPoint.Y), new Point(CurrentPolygon.Points.First().X, CurrentPolygon.Points.First().Y));
                }

                else if (res == 1)
                {
                    g.FillEllipse(Brushes.White, e.X - RADIUS, e.Y - RADIUS, RADIUS * 2, RADIUS * 2);
                    g.DrawLine(Pens.Red, new Point(e.X, e.Y), new Point(LastPoint.X, LastPoint.Y));
                }
                else
                {
                    MessageBox.Show("Polygon is already a cycle, no more points can be added to it!");
                }
            }
            Canvas.Refresh();
            g.Dispose();
        }



        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Polygons.Add(new Polygon());
            CurrentPolygon = Polygons.Last();
        }

        private void ClearCanvasButton_Click(object sender, EventArgs e)
        {
            CurrentPolygon = null;
            Polygons = new List<Polygon>();
            using (Graphics g = Graphics.FromImage(DrawArea))
            {

                g.Clear(Color.Green);

            }
            Canvas.Refresh();
        }
    }
}