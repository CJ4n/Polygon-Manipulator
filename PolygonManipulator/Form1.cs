namespace PolygonManipulator
{
    public enum LastSelectedElement { POINT, LINE, POLYGON, NONE };
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
        // private enum LastSelectedElement { POINT, LINE, POLYGON };
        private LastSelectedElement _lastSelectedElement;
        private int _currentPointId;
        private Point _contextMenuOpenLocation;

        public Form1()
        {
            InitializeComponent();
            _drawArea = new Bitmap(Canvas.Size.Width, Canvas.Size.Height);
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
            using (Graphics g = Graphics.FromImage(_drawArea))
            {
                g.Clear(_canvasColor);
            }
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
            using (Graphics g = Graphics.FromImage(_drawArea))
            {
                g.Clear(_canvasColor);
            }
            Canvas.Refresh();
            this.groupBox1.Enabled = false;
            this.AddPointsRadioButton.Checked = false;
        }
        private void AddNewPolygonToCanvasButton_MouseClick(object sender, MouseEventArgs e)
        {
            _polygons.Add(new Polygon(_lineColor, _pointColor, Radius));
            _currentPolygon = _polygons.Last();
            this.groupBox1.Enabled = true;
            this.AddPointsRadioButton.Checked = true;
            _state = State.ADDING;
        }
        private void AddPointsRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            _state = State.ADDING;
        }
        private void MovePointsRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            _state = State.MOVEING;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_prevMouseLocation == e.Location)
            {
                return;
            }
            this.label1.Text = "posigion: (" + (e.X - _prevMouseLocation.X).ToString() + ", " + (e.Y - _prevMouseLocation.Y).ToString() + ")";
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
                    //using (Graphics g = Graphics.FromImage(_drawArea))
                    //{
                    //    g.Clear(_canvasColor);

                    //    foreach (var poly in _polygons)
                    //    {
                    //        poly.RepaintPolygon(g);
                    //    }
                    //}
                    //Canvas.Refresh();

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
                if (polygon.IsClickNearSomeLineOrPoint(e.Location) == 1)
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
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if (_polygons.Count == 0)
            {
                return;
            }
            _prevMouseLocation = e.Location;
            switch (_state)
            {
                case State.ADDING:
                    DrawAndAddPoint(sender, e);
                    break;
                case State.MOVEING:
                    this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);

                    break;
            }
        }
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_state)
            {
                case State.MOVEING:
                    this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);

                    //MovePointOrPolygon(sender, e);
                    break;
            }
        }
        private void RepaintCanvas()
        {
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
                    toolStripMenuItemDeleteLine.Click += new EventHandler(DeleteLine);
                    toolStripMenuItemAddPointMiddle.Click += new EventHandler(AddPointOnTheLine);
                    c.Items.Add(toolStripMenuItemDeleteLine);
                    c.Items.Add(toolStripMenuItemAddPointMiddle);
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
        }
    }
}