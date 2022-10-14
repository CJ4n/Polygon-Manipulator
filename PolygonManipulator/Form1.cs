namespace PolygonManipulator
{
    public enum LastSelectedElement { POINT, LINE, POLYGON, NONE, ADDING_CONSTRAINT_LENGTH, ADDING_CONSTRAINT_PARALLEL };
    public partial class Form1 : Form
    {
        private Bitmap _drawArea;
        private List<Polygon> _polygons;
        private Polygon _currentPolygon;
        private enum State { ADDING, MOVEING };
        private State _state;
        private Point _prevMouseLocation;
        private const int Radius = 4;
        private Color _canvasColor;
        private Brush _pointColor;
        private Pen _lineColor;
        private Brush _selectedPointColor;
        private Pen _selectedLineColor;
        private Brush _selectedPolygonPointColor;
        private Pen _selectedPolygonLineColor;
        private LastSelectedElement _lastSelectedElement;
        private int _currentPointId;
        private Point _contextMenuOpenLocation;
        private bool _myPaint = false;
        public Form1()
        {
            InitializeComponent();
            _drawArea = new Bitmap(Canvas.Size.Width * 10, Canvas.Size.Height * 10);
            Canvas.Image = _drawArea;
            _polygons = new List<Polygon>();
            _canvasColor = Color.LightBlue;
            _lineColor = new Pen(Brushes.Black, 1);
            _selectedLineColor = new Pen(Brushes.Yellow, 3);
            _selectedPolygonLineColor = new Pen(Brushes.DarkCyan, 2);
            _pointColor = Brushes.Black;
            _selectedPointColor = Brushes.Pink;
            _selectedPolygonPointColor = Brushes.DarkBlue;
            _lastSelectedElement = LastSelectedElement.POINT;
            _currentPointId = -1;
            AddNewPolygonToCanvasButton_MouseClick(null, null);

            using (Graphics g = Graphics.FromImage(_drawArea))
            {
                g.Clear(_canvasColor);
            }
            //GenerateScene1();{
            Scene1Button_Click(null, null);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void DrawAndAddPoint(object sender, MouseEventArgs e)
        {

            //Graphics g = Graphics.FromImage(_drawArea);

            if (_currentPolygon.Points.Count() == 0) // adding first point to canvas
            {
                _currentPolygon.AddPointAtEnd(e.X, e.Y);
                _currentPointId = _currentPolygon.Points.Count() - 1;
                //g.FillEllipse(_pointColor, e.X - Radius, e.Y - Radius, Radius * 2, Radius * 2);
            }
            else
            {
                var LastPoint = _currentPolygon.Points.Last();
                int res = _currentPolygon.AddPointAtEnd(e.X, e.Y);

                if (res == 0) // polygon just became a cycle
                {
                    //g.DrawLine(Pens.Black, new Point(LastPoint.X, LastPoint.Y), new Point(_currentPolygon.Points.First().X, _currentPolygon.Points.First().Y));
                }

                else if (res == 1) // added new point to polygon
                {
                    _currentPointId = _currentPolygon.Points.Count() - 1;
                    //g.FillEllipse(_pointColor, e.X - Radius, e.Y - Radius, Radius * 2, Radius * 2);
                    //g.DrawLine(_lineColor, new Point(e.X, e.Y), new Point(LastPoint.X, LastPoint.Y));
                }
                else
                {
                    MessageBox.Show("Polygon is already a cycle, no more points can be added to it!");
                }
            }
            //Canvas.Refresh();
            RepaintCanvas();
            //g.Dispose();
        }
        private void ClearCanvasButton_Click(object sender, EventArgs e)
        {
            _currentPolygon = null;
            _polygons = new List<Polygon>();
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            using (Graphics g = Graphics.FromImage(_drawArea))
            {
                g.Clear(_canvasColor);
            }
            Canvas.Refresh();

        }
        private void AddNewPolygonToCanvasButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (_currentPolygon != null && _currentPolygon.IsPolygonCycle == false) { return; }
            _polygons.Add(new Polygon(_lineColor, _pointColor, Radius));
            _currentPolygon = _polygons.Last();

            _state = State.ADDING;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_prevMouseLocation == e.Location)
            {
                return;
            }
            this.label1.Text = "posigion: (" + (e.X).ToString() + ", " + (e.Y).ToString() + ")";
            HandleMoveRequest(sender, e, _prevMouseLocation);
            _prevMouseLocation = e.Location;

        }
        private bool MovePoint(MouseEventArgs e, Point prevMouseLocation)
        {
            if (_currentPolygon != null)
            {
                int res = _currentPolygon.IsClickNearSomePoint(prevMouseLocation, _currentPointId);
                if (res != -1)
                {
                    _currentPolygon.TranslatePoint(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, res);
                    //using (Graphics g = Graphics.FromImage(_drawArea))
                    //{
                    //    g.Clear(_canvasColor);

                    //    foreach (var poly in _polygons)
                    //    {
                    //        poly.RepaintPolygon(g);
                    //    }
                    //}
                    //Canvas.Refresh();


                    RepaintCanvas();
                    _currentPointId = res;
                    this.label1.Text += " id: " + res.ToString();
                    return true;
                }
            }

            foreach (var polygon in _polygons)
            {
                if (polygon == _currentPolygon)
                {
                    continue;
                }
                int res = polygon.IsClickNearSomePoint(prevMouseLocation);
                if (res != -1)
                {

                    polygon.TranslatePoint(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, res);
                    //using (Graphics g = Graphics.FromImage(_drawArea))
                    //{
                    //    g.Clear(_canvasColor);

                    //    foreach (var poly in _polygons)
                    //    {
                    //        poly.RepaintPolygon(g);
                    //    }
                    //}
                    //Canvas.Refresh();

                    _currentPolygon = polygon;
                    _currentPointId = res;
                    RepaintCanvas();
                    return true;
                }
            }
            return false;
        }
        private bool MoveLine(MouseEventArgs e, Point prevMouseLocation)
        {
            if (_currentPolygon != null)
            {
                int resLine = _currentPolygon.IsClickNearSomeLine(prevMouseLocation, _currentPointId);
                this.label1.Text += " Id: " + resLine.ToString();
                if (resLine != -1)
                {
                    _currentPolygon.TranslateLine(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, resLine);
                    _currentPointId = resLine;
                    RepaintCanvas();
                    return true;
                }
            }

            foreach (var polygon in _polygons)
            {
                if (polygon == _currentPolygon)
                {
                    continue;
                }
                int res = polygon.IsClickNearSomeLine(prevMouseLocation);
                if (res != -1)
                {
                    polygon.TranslateLine(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, res);
                    _currentPolygon = polygon;
                    _currentPointId = res;
                    RepaintCanvas();
                    return true;
                }
            }
            return false;
        }
        private bool MovePolygon(MouseEventArgs e, Point prevMouseLocation)
        {
            if (_currentPolygon.IsPointInBoundingBox(e.Location))
            {
                _currentPolygon.TranslatePolygon(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y);
                //using (Graphics g = Graphics.FromImage(_drawArea))
                //{
                //    g.Clear(_canvasColor);

                //    foreach (var poly in _polygons)
                //    {
                //        poly.RepaintPolygon(g);
                //    }
                //}
                //Canvas.Refresh();
                RepaintCanvas();

                return true;
            }
            foreach (Polygon polygon in _polygons)
            {
                if (polygon.IsClickNearSomeLineOrPoint(e.Location) == true)
                {
                    return false;
                }
            }
            foreach (Polygon polygon in _polygons)
            {
                if (polygon.IsPointInBoundingBox(e.Location))
                {
                    polygon.TranslatePolygon(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y);
                    //using (Graphics g = Graphics.FromImage(_drawArea))
                    //{
                    //    g.Clear(_canvasColor);

                    //    foreach (var poly in _polygons)
                    //    {
                    //        poly.RepaintPolygon(g);
                    //    }
                    //}
                    //Canvas.Refresh();
                    _currentPolygon = polygon;
                    _currentPointId = -1;
                    RepaintCanvas();
                    return true;
                }
            }
            return false;
        }
        private void HandleMoveRequest(object sender, MouseEventArgs e, Point prevMouseLocation)
        {
            if (_currentPolygon.IsPolygonCycle == false)
            {
                return;
            }

            if (_lastSelectedElement == LastSelectedElement.POINT && MovePoint(e, prevMouseLocation))
            {
            }
            else if (_lastSelectedElement == LastSelectedElement.LINE && MoveLine(e, prevMouseLocation))
            {

            }
            else if (_lastSelectedElement == LastSelectedElement.POLYGON && MovePolygon(e, prevMouseLocation))
            {
            }
            else if (MovePoint(e, prevMouseLocation))
            {
                _lastSelectedElement = LastSelectedElement.POINT;
            }
            else if (MoveLine(e, prevMouseLocation))
            {
                _lastSelectedElement = LastSelectedElement.LINE;
            }
            else if (MovePolygon(e, prevMouseLocation))
            {
                _lastSelectedElement = LastSelectedElement.POLYGON;
            }
            if (_lastSelectedElement == LastSelectedElement.LINE)
                this.label1.Text += ": LINE";
            else if (_lastSelectedElement == LastSelectedElement.POINT)
                this.label1.Text += ": POINT";
            else
                this.label1.Text += ": POLYGON";
        }
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            _prevMouseLocation = e.Location;
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            if (e.Button == MouseButtons.Middle)
            {
                DrawAndAddPoint(sender, e);
            }
            if (e.Button == MouseButtons.Left)
            {
                if (_lastSelectedElement == LastSelectedElement.ADDING_CONSTRAINT_PARALLEL)
                {
                    _lastSelectedElement = LastSelectedElement.LINE;
                    AddConstraintParallel(_currentPointId, e.Location);
                }
                else if (_lastSelectedElement == LastSelectedElement.ADDING_CONSTRAINT_LENGTH)
                {
                    _lastSelectedElement = LastSelectedElement.LINE;
                    AddConstraintLength(e.Location);
                }
                else
                {
                    this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
                }
            }
            //switch (_state)
            //{
            //    case State.ADDING:
            //        DrawAndAddPoint(sender, e);
            //        break;
            //    case State.MOVEING:
            //        this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);

            //        break;
            //}
        }
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);

            }
            //switch (_state)
            //{
            //    case State.MOVEING:
            //        this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            //        break;
            //}
        }
        private void RepaintCanvas()
        {
            if (_myPaint)
            {
                using (Graphics g = Graphics.FromImage(_drawArea))
                {
                    g.Clear(_canvasColor);
                    foreach (var poly in _polygons)
                    {
                        poly.OwnPaint(_drawArea, g);
                    }
                }
                Canvas.Refresh();
                return;
            }
            using (Graphics g = Graphics.FromImage(_drawArea))
            {
                g.Clear(_canvasColor);
                foreach (var poly in _polygons)
                {
                    if (poly == _currentPolygon)
                    {
                        poly.RepaintPolygon(g, _selectedPolygonLineColor, _selectedPolygonPointColor, _selectedLineColor, _selectedPointColor, _currentPointId, _lastSelectedElement);

                    }
                    else
                    {
                        poly.RepaintPolygon(g, _lineColor, _pointColor, _lineColor, _pointColor);
                    }
                }
            }
            Canvas.Refresh();
        }
        (Polygon?, int) GetPointFromLocation(Point p)
        {
            int res = _currentPolygon.IsClickNearSomePoint(p, _currentPointId);
            if (res != -1)
            {
                return (_currentPolygon, res);
            }
            foreach (Polygon polygon in _polygons)
            {
                res = polygon.IsClickNearSomePoint(p, _currentPointId);
                if (res != -1)
                {
                    return (polygon, res);
                }
            }
            return (null, -1);
        }
        (Polygon?, int) GetLineFromLocation(Point p)
        {
            int res = _currentPolygon.IsClickNearSomeLine(p, _currentPointId);
            if (res != -1)
            {
                return (_currentPolygon, res);
            }
            foreach (Polygon polygon in _polygons)
            {
                res = polygon.IsClickNearSomeLine(p, _currentPointId);
                if (res != -1)
                {
                    return (polygon, res);
                }
            }
            return (null, -1);
        }
        void DeletePoint(object sender, EventArgs e)
        {
            var res = GetPointFromLocation(_contextMenuOpenLocation);
            if (res.Item1 != null)
            {
                res.Item1.DeletePoint(res.Item2);
            }
            RepaintCanvas();
        }
        void DeleteLine(object sender, EventArgs e)
        {
            var res = GetLineFromLocation(_contextMenuOpenLocation);
            if (res.Item1 != null)
            {
                res.Item1.DeleteLine(res.Item2);
            }
            RepaintCanvas();
        }
        void AddPointOnTheLine(object sender, EventArgs e)
        {
            var res = GetLineFromLocation(_contextMenuOpenLocation);
            if (res.Item1 != null)
            {
                res.Item1.AddPointInTheMiddleOfLine(res.Item2);
            }
            RepaintCanvas();
        }
        void AddConstraintParallel(int idP1, Point p2)
        {
            MyPoint p11 = _currentPolygon.GetPointFromId(idP1);
            var res = GetLineFromLocation(p2);
            if (res.Item1 == null)
            {
                return;
            }
            MyPoint p22 = res.Item1.GetPointFromId(res.Item2);
            p11.AddConstraintParallel(p11, p22, _currentPolygon, res.Item1);
            //p11.Constraints.Add(new Constraint(p11, p22, _currentPolygon, res.Item1));
            //p22.Constraints.Add(new Constraint(p22, p11, res.Item1, _currentPolygon));
            //MessageBox.Show("added constrain");
            RepaintCanvas();
        }
        void AddConstraintLength(Point p)
        {
            var res = GetLineFromLocation(p);

            if (res.Item1 == null)
            {
                return;
            }
            MyPoint point = res.Item1.GetPointFromId(res.Item2);
            point.AddConstraintLength(point, point.Next);
            //point.Next.AddConstraintLength(point.Next, point);
            RepaintCanvas();
        }
        void InitiateAddingConstraintParallel(object sender, EventArgs e)
        {
            _lastSelectedElement = LastSelectedElement.ADDING_CONSTRAINT_PARALLEL;
        }
        void InitiateAddingConstraintLength(object sender, EventArgs e)
        {
            _lastSelectedElement = LastSelectedElement.ADDING_CONSTRAINT_LENGTH;
        }
        private void CreateContextMenu(PictureBox sender, MouseEventArgs e)
        {
            _contextMenuOpenLocation = e.Location;
            var c = new ContextMenuStrip();

            var resPoint = GetPointFromLocation(e.Location);
            if (resPoint.Item1 != null)
            {
                ToolStripMenuItem toolStripMenuItemDeletePoint = new ToolStripMenuItem("Delete point");
                toolStripMenuItemDeletePoint.Click += new EventHandler(DeletePoint);
                c.Items.Add(toolStripMenuItemDeletePoint);
                _lastSelectedElement = LastSelectedElement.POINT;
                _currentPointId = resPoint.Item2;
            }
            else
            {
                var resLine = GetLineFromLocation(e.Location);
                if (resLine.Item1 != null && resPoint.Item1 == null)
                {
                    ToolStripMenuItem toolStripMenuItemDeleteLine = new ToolStripMenuItem("Delete line");
                    ToolStripMenuItem toolStripMenuItemAddPointMiddle = new ToolStripMenuItem("Add point in the middle");
                    ToolStripMenuItem toolStripMenuConstraintParalell = new ToolStripMenuItem("Add constraint parallel");
                    ToolStripMenuItem toolStripMenuConstraintLength = new ToolStripMenuItem("Add constraint on length");
                    toolStripMenuItemDeleteLine.Click += new EventHandler(DeleteLine);
                    toolStripMenuItemAddPointMiddle.Click += new EventHandler(AddPointOnTheLine);
                    toolStripMenuConstraintParalell.Click += new EventHandler(InitiateAddingConstraintParallel);
                    toolStripMenuConstraintLength.Click += new EventHandler(InitiateAddingConstraintLength);
                    c.Items.Add(toolStripMenuItemDeleteLine);
                    c.Items.Add(toolStripMenuItemAddPointMiddle);
                    c.Items.Add(toolStripMenuConstraintParalell);
                    c.Items.Add(toolStripMenuConstraintLength);
                    _lastSelectedElement = LastSelectedElement.LINE;
                    _currentPointId = resLine.Item2;
                }
                else
                {
                    _currentPointId = -1;
                }
            }
            RepaintCanvas();
            c.Show(sender, e.Location);
            return;
        }
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CreateContextMenu(sender as PictureBox, e);
            }
            //if(e.Button )
        }
        private void GenerateScene1()
        {
            _currentPolygon.AddPointAtEnd(66, 79);
            _currentPolygon.AddPointAtEnd(75, 140);
            _currentPolygon.AddPointAtEnd(435, 165);
            _currentPolygon.AddPointAtEnd(506, 84);
            _currentPolygon.AddPointAtEnd(359, 32);
            _currentPolygon.AddPointAtEnd(254, 26);
            _currentPolygon.AddPointAtEnd(246, 114);
            _currentPolygon.AddPointAtEnd(281, 206);
            _currentPolygon.AddPointAtEnd(188, 202);
            _currentPolygon.AddPointAtEnd(211, 77);
            _currentPolygon.AddPointAtEnd(116, 28);
            _currentPolygon.AddPointAtEnd(129, 84);
            _currentPolygon.AddPointAtEnd(66, 79);
            RepaintCanvas();
            //_currentPolygon.AddPointAtEnd(, );
            //_currentPolygon.AddPointAtEnd(, );

        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _myPaint = !_myPaint;
            RepaintCanvas();
        }

        private void Scene1Button_Click(object sender, EventArgs e)
        {
            _currentPolygon.AddPointAtEnd(66, 79);
            _currentPolygon.AddPointAtEnd(75, 140);
            _currentPolygon.AddPointAtEnd(435, 165);
            _currentPolygon.AddPointAtEnd(506, 84);
            _currentPolygon.AddPointAtEnd(359, 32);
            _currentPolygon.AddPointAtEnd(254, 26);
            _currentPolygon.AddPointAtEnd(246, 114);
            _currentPolygon.AddPointAtEnd(281, 206);
            _currentPolygon.AddPointAtEnd(188, 202);
            _currentPolygon.AddPointAtEnd(211, 77);
            _currentPolygon.AddPointAtEnd(116, 28);
            _currentPolygon.AddPointAtEnd(129, 84);
            _currentPolygon.AddPointAtEnd(66, 79);
            RepaintCanvas();
        }

        private void Scene2Button_Click(object sender, EventArgs e)
        {

        }
    }
}