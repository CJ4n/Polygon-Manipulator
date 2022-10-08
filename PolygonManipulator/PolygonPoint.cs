namespace PolygonManipulator
{

    public class MyPoint
    {
        public int Id;
        public int X, Y;
        public MyPoint Prev, Next;
        public MyPoint(int x, int y, int id, MyPoint prev, MyPoint next)
        {
            Id = id;
            X = x;
            Y = y;
            Prev = prev;
            Next = next;
        }
    }

    public class Polygon
    {
        public List<MyPoint> Points { get; set; }
        private bool _isPolygonCycle = false;
        const int SearchRadiusPoint = 4;
        const int SearchRadiusLine = 8;
        private int _pointRadius;
        private Brush _pointColor;
        private Pen _lineColor;
        private Font _font;
        private class BoundingBox
        {
            public int MaxX = int.MinValue;
            public int MaxY = int.MinValue;
            public int MinX = int.MaxValue;
            public int MinY = int.MaxValue;
        }
        private BoundingBox _boundingBox;
        private int _maxId = 0;

        public Polygon(Pen line, Brush point, int pointRadius)
        {
            _lineColor = line;
            _pointColor = point;
            Points = new List<MyPoint>();

            _boundingBox = new BoundingBox();
            _pointRadius = pointRadius;
            _font = new Font("Arial", 10);
        }
        private MyPoint GetPointFromId(int id)
        {
            foreach (var p in Points)
            {
                if (p.Id == id)
                {
                    return p;
                }
            }
            return null;
        }

        public int AddPointAtEnd(int x, int y)
        {
            if (_isPolygonCycle)
            {
                return -1; // Can't add more points to polygon, polygon is already a cycle
            }
            if (Points.Count != 0)
            {
                MyPoint lastPoint = Points.Last();
                foreach (MyPoint p in Points) // If new point closes cycle 
                {
                    MyPoint firstPoint = Points.First();

                    if (AreTwoPointsNear(x, y, firstPoint.X, firstPoint.Y))
                    {
                        lastPoint.Next = firstPoint;
                        firstPoint.Prev = lastPoint;
                        _isPolygonCycle = true;
                        MessageBox.Show("end");

                        return 0; // Creation of polygon has just been finished
                    }
                }
                MyPoint newPoint = new MyPoint(x, y, _maxId++, lastPoint, null);
                lastPoint.Next = newPoint;
                Points.Add(newPoint);

                lastPoint = Points.Last();

            }
            else
            {
                Points.Add(new MyPoint(x, y, _maxId++, null, null));
            }

            UpdateBoundingBoxAfterTranslation();
            return 1; // Added new point to polygon
        }
        public bool AreTwoPointsNear(int x1, int y1, int x2, int y2)
        {
            //return Math.Abs(x1 - x2) < radius && Math.Abs(y1 - y2) < radius;
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) <= SearchRadiusPoint * SearchRadiusPoint;
        }
        public bool IsPointInBoundingBox(Point point)
        {
            if (IsClickNearSomeLineOrPoint(point) == true) // there is some line of point near click, so we want to move them
            {
                return false;
            }
            //// todo: fix bounding box sth wrong when polygon goes outside of canvas
            //return _boundingBox.MinX < point.X && _boundingBox.MaxX > point.X && _boundingBox.MinY < point.Y && _boundingBox.MaxY > point.Y;

            var c = new ConvexHullAlgorithms();
            (double, double)[] points = new (double, double)[Points.Count()];
            int i = 0;
            foreach (var p in Points)
            {
                points[i++] = (p.X, p.Y);
            }
            var res = c.ConvexHull(points);
            if (res == null)
            {
                return false;
            }
            var ret = c.IsPointInPolygon(res, (point.X, point.Y));
            return ret;
            //public static bool IsPointInPolygon4(PointF[] Points, PointF point)
            //{
            //    bool result = false;
            //    int j = Points.Count() - 1;
            //    for (int i = 0; i < Points.Count(); i++)
            //    {
            //        if (Points[i].Y < point.Y && Points[j].Y >= point.Y || Points[j].Y < point.Y && Points[i].Y >= point.Y)
            //        {
            //            if (Points[i].X + (point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X) < point.X)
            //            {
            //                result = !result;
            //            }
            //        }
            //        j = i;
            //    }
            //    return result;
            //}
        }
        public void UpdateBoundingBoxAfterTranslation()
        {
            int MaxX = int.MinValue;
            int MaxY = int.MinValue;
            int MinX = int.MaxValue;
            int MinY = int.MaxValue;
            foreach (MyPoint p in Points)
            {
                MaxX = Math.Max(MaxX, p.X);
                MinX = Math.Min(MinX, p.X);
                MaxY = Math.Max(MaxY, p.Y);
                MinY = Math.Min(MinY, p.Y);
            }
            _boundingBox.MaxX = MaxX;
            _boundingBox.MinX = MinX;
            _boundingBox.MaxY = MaxY;
            _boundingBox.MinY = MinY;
        }
        // Return points id, if click is within Radius from some point, otherwise -1
        public int IsClickNearSomePoint(Point p, int id = -1)
        {
            if (id != -1)
            {
                MyPoint pt = GetPointFromId(id);
                if (pt != null)
                {
                    if (AreTwoPointsNear(pt.X, pt.Y, p.X, p.Y))
                    {
                        return pt.Id;
                    }
                }
            }
            foreach (MyPoint point in Points)
            {
                if (AreTwoPointsNear(point.X, point.Y, p.X, p.Y))
                {
                    return point.Id;
                }
            }
            return -1;
        }
        public int IsClickNearSomeLine(Point p, int id = -1)
        {
            if (id != -1)
            {
                MyPoint point = GetPointFromId(id);
                if (point != null)
                {
                    if (point.Next != null)
                    {

                        int minX = Math.Min(point.X, point.Next.X);
                        int maxX = Math.Max(point.X, point.Next.X);
                        int minY = Math.Min(point.Y, point.Next.Y);
                        int maxY = Math.Max(point.Y, point.Next.Y);
                        if (!(minX > p.X || maxX < p.X || minY > p.Y || maxY < p.Y))
                        {
                            int x1 = point.X, y1 = point.Y, x2 = point.Next.X, y2 = point.Next.Y;
                            double A = y1 - y2;
                            double B = -(x1 - x2);
                            double C = y1 * (x1 - x2) - x1 * (y1 - y2);
                            if (Math.Abs(A * p.X + B * p.Y + C) / Math.Sqrt(A * A + B * B) < SearchRadiusLine)
                            {
                                return point.Id;
                            }
                        }
                    }
                }
            }
            foreach (var mp in Points)
            {
                if (mp.Next == null)
                {
                    continue;
                }
                int minX = Math.Min(mp.X, mp.Next.X);
                int maxX = Math.Max(mp.X, mp.Next.X);
                int minY = Math.Min(mp.Y, mp.Next.Y);
                int maxY = Math.Max(mp.Y, mp.Next.Y);
                if (minX > p.X || maxX < p.X || minY > p.Y || maxY < p.Y)
                {
                    continue;
                }

                int x1 = mp.X, y1 = mp.Y, x2 = mp.Next.X, y2 = mp.Next.Y;
                double A = y1 - y2;
                double B = -(x1 - x2);
                double C = y1 * (x1 - x2) - x1 * (y1 - y2);
                if (Math.Abs(A * p.X + B * p.Y + C) / Math.Sqrt(A * A + B * B) < SearchRadiusLine)
                {
                    return mp.Id;
                }
            }
            return -1;
        }
        public bool IsClickNearSomeLineOrPoint(Point p)
        {
            if (IsClickNearSomeLine(p) != -1 || IsClickNearSomePoint(p) != -1)
            {
                return true;
            }
            return false;
        }
        public void RepaintPolygon(Graphics g, Pen lineColor, Brush pointColor, Pen selectedLineColor, Brush selectedPointColor,
            int pointId = -1, LastSelectedElement lastSelectedElement = LastSelectedElement.POLYGON)
        {
            var selectedPoint = GetPointFromId(pointId);
            if (selectedPoint == null)
            {
                selectedPoint = new MyPoint(0, 0, -1, null, null);
            }
            if (lastSelectedElement == LastSelectedElement.POINT)
            {
                foreach (MyPoint point in Points)
                {
                    if (point.Id == selectedPoint.Id)
                    {
                        g.FillEllipse(selectedPointColor, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                    }
                    else
                    {
                        g.FillEllipse(pointColor, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                    }
                    if (point.Next != null)
                    {
                        g.DrawLine(lineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Blue, new Point(point.X, point.Y));
                }
            }
            else if (lastSelectedElement == LastSelectedElement.LINE)
            {

                foreach (MyPoint point in Points)
                {
                    if (point.Id == selectedPoint.Id || (selectedPoint.Next != null && point.Id == selectedPoint.Next.Id))
                    {
                        g.FillEllipse(selectedPointColor, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                    }
                    else
                    {
                        g.FillEllipse(pointColor, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                    }
                    if (point.Next != null)
                    {
                        if (point.Id == selectedPoint.Id)
                        {
                            g.DrawLine(selectedLineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                        }
                        else
                        {
                            g.DrawLine(lineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                        }
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Blue, new Point(point.X, point.Y));
                }
            }
            else if (lastSelectedElement == LastSelectedElement.POLYGON)
            {
                foreach (MyPoint point in Points)
                {
                    g.FillEllipse(pointColor, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                    if (point.Next != null)
                    {
                        g.DrawLine(lineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Brown, new Point(point.X, point.Y));
                }
            }
        }
        public void TranslatePoint(int dx, int dy, int id)
        {
            var point = GetPointFromId(id);
            if (point == null)
            {
                return;
            }
            point.X -= dx;
            point.Y -= dy;
            UpdateBoundingBoxAfterTranslation();
        }
        public void TranslateLine(int dx, int dy, int id)
        {

            TranslatePoint(dx, dy, id);
            var point = GetPointFromId(id);
            id = point.Next.Id;
            TranslatePoint(dx, dy, id);
        }
        public void TranslatePolygon(int dx, int dy)
        {
            foreach (var point in Points)
            {
                TranslatePoint(dx, dy, point.Id);
            }
        }
        public void DeletePoint(int id)
        {
            var p = GetPointFromId(id);
            if (p == null)
            {
                return;
            }
            if (Points.Count() <= 3)
            {
                Points.Clear();
            }
            else
            {
                if (p.Prev != null)
                {
                    p.Prev.Next = p.Next;
                }
                if (p.Next != null)
                {
                    p.Next.Prev = p.Prev;
                }
                Points.Remove(p);
            }
        }
        public void DeleteLine(int id)
        {
            int nextId = GetPointFromId(id).Next.Id;
            DeletePoint(id);

            DeletePoint(nextId);
        }
        public void AddPointInTheMiddleOfLine(int id)
        {
            var p1 = GetPointFromId(id);
            var p2 = GetPointFromId(p1.Next.Id);
            int x = (p1.X + p2.X) / 2;
            int y = (p1.Y + p2.Y) / 2;
            MyPoint newPoint = new MyPoint(x, y, _maxId++, p1, p2);
            p1.Next = newPoint;
            p2.Prev = newPoint;
            Points.Add(newPoint);

        }
    }

}
