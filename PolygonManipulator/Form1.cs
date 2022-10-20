namespace PolygonManipulator
{
    public enum LastSelectedElement { POINT, LINE, POLYGON, NONE, ADDING_CONSTRAINT_LENGTH, ADDING_CONSTRAINT_PARALLEL };
    public partial class Form1 : Form
    {
        private Bitmap _drawArea;
        private List<Polygon> _polygons;
        private Polygon _currentPolygon;
        private PointF _prevMouseLocation;
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
        private PointF _contextMenuOpenLocation;
        private bool _myPaint = false;
        private bool _mouseDown = false;
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
            _currentPolygon = null;
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            RepaintCanvas();
            Scene1Button_Click(null, null);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void DrawAndAddPoint(object sender, MouseEventArgs e)
        {


            if (_currentPolygon.Points.Count() == 0) // adding first point to canvas
            {
                _currentPolygon.AddPointAtEnd(e.X, e.Y);
                _currentPointId = _currentPolygon.Points.Count() - 1;
            }
            else
            {
                var LastPoint = _currentPolygon.Points.Last();
                int res = _currentPolygon.AddPointAtEnd(e.X, e.Y);

                if (res == 0) // polygon just became a cycle
                {
                }

                else if (res == 1) // added new point to polygon
                {
                    _currentPointId = _currentPolygon.Points.Count() - 1;
                }
                else
                {
                    MessageBox.Show("Polygon is already a cycle, no more points can be added to it!");
                }
            }
            RepaintCanvas();
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
            MyPoint.MaxConstraitnId = 0;
            Canvas.Refresh();
        }
        private void AddNewPolygonToCanvasButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (_currentPolygon != null && _currentPolygon.IsPolygonCycle == false) { return; }
            _polygons.Add(new Polygon(_lineColor, _pointColor, Radius));
            _currentPolygon = _polygons.Last();
        }
        private bool MovePoint(MouseEventArgs e, PointF prevMouseLocation)
        {
            if (_mouseDown == true)
            {
                _currentPolygon.TranslatePoint(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, _currentPointId);
                RepaintCanvas();
                return true;
            }

            if (_currentPolygon != null)
            {
                int res = _currentPolygon.IsClickNearSomePoint(prevMouseLocation, _currentPointId);
                if (res != -1)
                {
                    _currentPolygon.TranslatePoint(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, res);
                    RepaintCanvas();
                    _currentPointId = res;
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
                    _currentPolygon = polygon;
                    _currentPointId = res;
                    RepaintCanvas();
                    return true;
                }
            }
            return false;
        }
        private bool MoveLine(MouseEventArgs e, PointF prevMouseLocation)
        {

            if (_mouseDown == true)
            {
                _currentPolygon.TranslateLine(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y, _currentPointId);
                RepaintCanvas();
                return true;
            }
            if (_currentPolygon != null)
            {
                int resLine = _currentPolygon.IsClickNearSomeLine(prevMouseLocation, _currentPointId);
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
        private bool MovePolygon(MouseEventArgs e, PointF prevMouseLocation)
        {
            if (_mouseDown == true)
            {
                _currentPolygon.TranslatePolygon(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y);
                RepaintCanvas();
                return true;
            }

            if (_currentPolygon.IsPointInBoundingBox(e.Location))
            {
                _currentPolygon.TranslatePolygon(prevMouseLocation.X - e.X, prevMouseLocation.Y - e.Y);
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
                    _currentPolygon = polygon;
                    _currentPointId = -1;
                    RepaintCanvas();
                    return true;
                }
            }
            return false;
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_prevMouseLocation == e.Location)
            {
                return;
            }
            HandleMoveRequest(sender, e, _prevMouseLocation);
            _prevMouseLocation = e.Location;
        }
        private void HandleMoveRequest(object sender, MouseEventArgs e, PointF prevMouseLocation)
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
        }
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            _prevMouseLocation = e.Location;
            _mouseDown = true;
            if (e.Button == MouseButtons.Right)
            {
                CreateContextMenu(sender as PictureBox, e);
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
                else
                {
                    this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);

                    var resLine = GetLineFromLocation(e.Location);
                    if (resLine.Item1 != null)
                    {
                        _lastSelectedElement = LastSelectedElement.LINE;
                        _currentPointId = resLine.Item2;
                        _currentPolygon = resLine.Item1;
                        return;
                    }

                    var resPoint = GetPointFromLocation(e.Location);
                    if (resPoint.Item1 != null)
                    {
                        _lastSelectedElement = LastSelectedElement.POINT;
                        _currentPointId = resPoint.Item2;
                        _currentPolygon = resPoint.Item1;
                        return;
                    }

                    foreach (Polygon polygon in _polygons)
                    {
                        if (polygon.IsPointInBoundingBox(e.Location))
                        {
                            _currentPolygon = polygon;
                            _currentPointId = -1;
                            _lastSelectedElement = LastSelectedElement.POLYGON;
                            return;
                        }
                    }
                }
            }
        }
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            if (e.Button == MouseButtons.Left)
            {
                this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            }
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
        (Polygon?, int) GetPointFromLocation(PointF p)
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
        (Polygon?, int) GetLineFromLocation(PointF p)
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
        void AddConstraintParallel(int idP1, PointF p2)
        {
            MyPoint p11 = _currentPolygon.GetPointFromId(idP1);
            if (p11 == null)
            {
                return;
            }
            var res = GetLineFromLocation(p2);
            if (res.Item1 == null)
            {
                return;
            }
            MyPoint p22 = res.Item1.GetPointFromId(res.Item2);
            p11.AddConstraintParallel(p11, p22, _currentPolygon, res.Item1);
            RepaintCanvas();
        }
        void InitiateAddingConstraintParallel(object sender, EventArgs e)
        {
            _lastSelectedElement = LastSelectedElement.ADDING_CONSTRAINT_PARALLEL;
        }
        void InitiateAddingConstraintLength(object sender, EventArgs e)
        {
            var res = GetLineFromLocation(_prevMouseLocation);
            if (res.Item1 == null)
            {
                return;
            }
            MyPoint point = res.Item1.GetPointFromId(res.Item2);

            string ret = Microsoft.VisualBasic.Interaction.InputBox("Enter new length for line", "Set length"
                , Math.Round(Math.Sqrt(Math.Pow(point.X - point.Next.X, 2) + Math.Pow(point.Y - point.Next.Y, 2)), 2).ToString());
            if (ret == "") { return; }

            try
            {
                var newlength = float.Parse(ret);
                if (newlength <= 0)
                {
                    MessageBox.Show("Negative distance!");
                    return;
                }

                point.AddConstraintLength(point, point.Next, newlength);
                RepaintCanvas();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


        }
        bool HasLineAnyConstraint(PointF p)
        {
            var res = GetLineFromLocation(p);
            if (res.Item1 == null)
            {
                return false;
            }
            return res.Item1.GetPointFromId(res.Item2).HowManyMeaningfulConstraints() > 0;
        }
        void DeleteConstraints(object sender, EventArgs e)
        {
            var res = GetLineFromLocation(_prevMouseLocation);
            if (res.Item1 == null)
            {
                return;
            }
            MyPoint point = res.Item1.GetPointFromId(res.Item2);
            point.DeleteAssociatedConstraints();
            RepaintCanvas();
        }
        void CreateContextMenu(PictureBox sender, MouseEventArgs e)
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
                _currentPolygon = resPoint.Item1;
            }
            else
            {
                var resLine = GetLineFromLocation(e.Location);

                RepaintCanvas();
                if (resLine.Item1 != null && resPoint.Item1 == null)
                {
                    _currentPointId = resLine.Item2;
                    _currentPolygon = resLine.Item1;
                    _lastSelectedElement = LastSelectedElement.LINE;

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

                    if (HasLineAnyConstraint(e.Location))
                    {
                        ToolStripMenuItem toolStripMenuItemDeleteConstraints = new ToolStripMenuItem("Delete constaints");
                        toolStripMenuItemDeleteConstraints.Click += new EventHandler(DeleteConstraints);
                        c.Items.Add(toolStripMenuItemDeleteConstraints);

                    }
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
        private void label1_Click(object sender, EventArgs e)
        {
            this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            _mouseDown = false;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _myPaint = !_myPaint;

            RepaintCanvas();
        }
        private void Scene1Button_Click(object sender, EventArgs e)
        {
            AddNewPolygonToCanvasButton_MouseClick(null, null);
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
            _currentPolygon.Points[9].AddConstraintParallel(_currentPolygon.Points[9], _currentPolygon.Points[5], _currentPolygon, _currentPolygon);
            _currentPolygon.Points[0].AddConstraintParallel(_currentPolygon.Points[0], _currentPolygon.Points[7], _currentPolygon, _currentPolygon);
            _currentPolygon.Points[8].AddConstraintLength(_currentPolygon.Points[8], _currentPolygon.Points[8].Next, 100);


            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(200 + 66, 200 + 79);
            _currentPolygon.AddPointAtEnd(200 + 75, 200 + 140);
            _currentPolygon.AddPointAtEnd(200 + 435, 200 + 165);
            _currentPolygon.AddPointAtEnd(200 + 506, 200 + 84);
            _currentPolygon.AddPointAtEnd(200 + 359, 200 + 32);
            _currentPolygon.AddPointAtEnd(200 + 254, 200 + 26);
            _currentPolygon.AddPointAtEnd(200 + 246, 200 + 114);
            _currentPolygon.AddPointAtEnd(200 + 281, 200 + 206);
            _currentPolygon.AddPointAtEnd(200 + 188, 200 + 202);
            _currentPolygon.AddPointAtEnd(200 + 211, 200 + 77);
            _currentPolygon.AddPointAtEnd(200 + 116, 200 + 28);
            _currentPolygon.AddPointAtEnd(200 + 129, 200 + 84);
            _currentPolygon.AddPointAtEnd(200 + 66, 200 + 79);

            _currentPolygon.Points[2].AddConstraintParallel(_currentPolygon.Points[2], _currentPolygon.Points[0], _currentPolygon, _currentPolygon);
            _currentPolygon.Points[2].AddConstraintParallel(_currentPolygon.Points[2], _polygons[0].Points[9], _currentPolygon, _currentPolygon);
            _currentPolygon.Points[7].AddConstraintLength(_currentPolygon.Points[7], _currentPolygon.Points[7].Next, 100);
            _currentPolygon.Points[8].AddConstraintLength(_currentPolygon.Points[8], _currentPolygon.Points[8].Next, 100);


            RepaintCanvas();
        }
        private void Scene2Button_Click(object sender, EventArgs e)
        {
            int baseYShift = 200;
            //wheel 0
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 230);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 200);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 230);
            var p0_wheel0 = _currentPolygon.GetPointFromId(0);
            var p2_wheel0 = _currentPolygon.GetPointFromId(2);
            var wheel0 = _currentPolygon;
            p0_wheel0.AddConstraintParallel(p0_wheel0, p2_wheel0, wheel0, wheel0);

            //wheel 1
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(330, baseYShift + 230);
            _currentPolygon.AddPointAtEnd(330, baseYShift + 200);
            _currentPolygon.AddPointAtEnd(330, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(330, baseYShift + 230);
            var p0_wheel1 = _currentPolygon.GetPointFromId(0);
            var p2_wheel1 = _currentPolygon.GetPointFromId(2);
            var wheel1 = _currentPolygon;
            p0_wheel1.AddConstraintParallel(p0_wheel1, p2_wheel1, wheel1, wheel1);

            //wheel 2
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(430, baseYShift + 230);
            _currentPolygon.AddPointAtEnd(430, baseYShift + 200);
            _currentPolygon.AddPointAtEnd(430, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(430, baseYShift + 230);
            var p0_wheel2 = _currentPolygon.GetPointFromId(0);
            var p2_wheel2 = _currentPolygon.GetPointFromId(2);
            var wheel2 = _currentPolygon;
            p0_wheel2.AddConstraintParallel(p0_wheel2, p2_wheel2, wheel2, wheel2);

            //wheel 3
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(530, baseYShift + 230);
            _currentPolygon.AddPointAtEnd(530, baseYShift + 200);
            _currentPolygon.AddPointAtEnd(530, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(530, baseYShift + 230);
            var p0_wheel3 = _currentPolygon.GetPointFromId(0);
            var p2_wheel3 = _currentPolygon.GetPointFromId(2);
            var wheel3 = _currentPolygon;
            p0_wheel3.AddConstraintParallel(p0_wheel3, p2_wheel3, wheel3, wheel3);

            //wheel 4
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(630, baseYShift + 230);
            _currentPolygon.AddPointAtEnd(630, baseYShift + 200);
            _currentPolygon.AddPointAtEnd(630, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(630, baseYShift + 230);
            var p0_wheel4 = _currentPolygon.GetPointFromId(0);
            var p2_wheel4 = _currentPolygon.GetPointFromId(2);
            var wheel4 = _currentPolygon;
            p0_wheel4.AddConstraintParallel(p0_wheel4, p2_wheel4, wheel4, wheel4);

            //connecting rod
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 228);
            _currentPolygon.AddPointAtEnd(630, baseYShift + 228);
            _currentPolygon.AddPointAtEnd(630, baseYShift + 172);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 172);
            _currentPolygon.AddPointAtEnd(230, baseYShift + 228);
            var p3_connectingRod = _currentPolygon.GetPointFromId(3);
            var p1_connectingRod = _currentPolygon.GetPointFromId(1);
            var ConnectingRod = _currentPolygon;
            p1_connectingRod.AddConstraintParallel(p1_connectingRod, p3_connectingRod, ConnectingRod, ConnectingRod);

            //some box
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(215, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(645, baseYShift + 170);
            _currentPolygon.AddPointAtEnd(645, baseYShift + 140);
            _currentPolygon.AddPointAtEnd(215, baseYShift + 140);
            _currentPolygon.AddPointAtEnd(215, baseYShift + 170);

            //main hulk
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(645, baseYShift + 140);
            _currentPolygon.AddPointAtEnd(645, baseYShift + 0);
            _currentPolygon.AddPointAtEnd(572.5f, baseYShift + -15);
            _currentPolygon.AddPointAtEnd(500, baseYShift + 0);
            _currentPolygon.AddPointAtEnd(500, baseYShift + 70);
            //chimny
            _currentPolygon.AddPointAtEnd(320, baseYShift + 70);
            _currentPolygon.AddPointAtEnd(320, baseYShift + 20);
            _currentPolygon.AddPointAtEnd(330, baseYShift + 10);
            _currentPolygon.AddPointAtEnd(325, baseYShift + 0);
            _currentPolygon.AddPointAtEnd(295, baseYShift + 0);
            _currentPolygon.AddPointAtEnd(290, baseYShift + 10);
            _currentPolygon.AddPointAtEnd(300, baseYShift + 20);
            _currentPolygon.AddPointAtEnd(300, baseYShift + 70);
            //chimny
            _currentPolygon.AddPointAtEnd(215, baseYShift + 70);
            _currentPolygon.AddPointAtEnd(200, baseYShift + 105);
            _currentPolygon.AddPointAtEnd(215, baseYShift + 140);
            _currentPolygon.AddPointAtEnd(500, baseYShift + 140);
            _currentPolygon.AddPointAtEnd(645, baseYShift + 140);

            //window
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(610, baseYShift + 60);
            _currentPolygon.AddPointAtEnd(610, baseYShift + 20);
            _currentPolygon.AddPointAtEnd(530, baseYShift + 20);
            _currentPolygon.AddPointAtEnd(530, baseYShift + 60);
            _currentPolygon.AddPointAtEnd(610, baseYShift + 60);

            // steer
            AddNewPolygonToCanvasButton_MouseClick(null, null);
            _currentPolygon.AddPointAtEnd(500, baseYShift + 300);
            _currentPolygon.AddPointAtEnd(525, baseYShift + 250);
            _currentPolygon.AddPointAtEnd(550, baseYShift + 300);
            _currentPolygon.AddPointAtEnd(500, baseYShift + 300);
            var p0_steer = _currentPolygon.GetPointFromId(0);
            var steer = _currentPolygon;
            p0_steer.AddConstraintParallel(p2_wheel0, p0_steer, wheel0, steer);
            p0_steer.AddConstraintParallel(p2_wheel1, p0_steer, wheel0, steer);
            p0_steer.AddConstraintParallel(p2_wheel2, p0_steer, wheel0, steer);
            p0_steer.AddConstraintParallel(p2_wheel3, p0_steer, wheel0, steer);
            p0_steer.AddConstraintParallel(p2_wheel4, p0_steer, wheel0, steer);
            p0_steer.AddConstraintParallel(p1_connectingRod, p0_steer, ConnectingRod, steer);


            RepaintCanvas();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Canvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
        }

    }
}