namespace PolygonManipulator
{
    public class Polygon
    {
        public Dictionary<int, MyPoint> Points { get; set; }
        public bool IsPolygonCycle = false;
        const int SearchRadiusPoint = 12;
        const int SearchRadiusLine = 9;
        private int _pointRadius;
        private Brush _pointColor;
        private Pen _lineColor;
        private Font _font;
        private int _maxId = 0;
        private MyPoint _lastPoint;
        private MyPoint _firstPoint;

        // https://rosettacode.org/wiki/Xiaolin_Wu%27s_line_algorithm#C#
        private void plot(Bitmap bitmap, double x, double y, double c)
        {
            int alpha = (int)(c * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Color color = Color.FromArgb(alpha, Color.Black);
            if (x < 0 || y < 0)
            {
                return;
            }
            bitmap.SetPixel((int)x, (int)y, color);
        }
        int ipart(double x) { return (int)x; }
        int round(double x) { return ipart(x + 0.5); }
        double fpart(double x)
        {
            if (x < 0) return (1 - (x - Math.Floor(x)));
            return (x - Math.Floor(x));
        }
        double rfpart(double x)
        {
            return 1 - fpart(x);
        }
        public void WuLine(float x0, float y0, float x1, float y1, Bitmap bitmap, int lineWidth)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            float temp;
            if (steep)
            {
                temp = x0; x0 = y0; y0 = temp;
                temp = x1; x1 = y1; y1 = temp;
            }
            if (x0 > x1)
            {
                temp = x0; x0 = x1; x1 = temp;
                temp = y0; y0 = y1; y1 = temp;
            }

            double dx = x1 - x0;
            double dy = y1 - y0;
            double gradient = dy / dx;

            double xEnd = round(x0);
            double yEnd = y0 + gradient * (xEnd - x0);
            double xGap = rfpart(x0 + 0.5);
            double xPixel1 = xEnd;
            double yPixel1 = ipart(yEnd);

            if (steep)
            {
                plot(bitmap, yPixel1, xPixel1, rfpart(yEnd) * xGap);
                plot(bitmap, yPixel1 + 1, xPixel1, fpart(yEnd) * xGap);
            }
            else
            {
                plot(bitmap, xPixel1, yPixel1, rfpart(yEnd) * xGap);
                plot(bitmap, xPixel1, yPixel1 + 1, fpart(yEnd) * xGap);
            }
            double intery = yEnd + gradient;

            xEnd = round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = fpart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = ipart(yEnd);
            if (steep)
            {
                plot(bitmap, yPixel2, xPixel2, rfpart(yEnd) * xGap);
                plot(bitmap, yPixel2 + 1, xPixel2, fpart(yEnd) * xGap);
            }
            else
            {
                plot(bitmap, xPixel2, yPixel2, rfpart(yEnd) * xGap);
                plot(bitmap, xPixel2, yPixel2 + 1, fpart(yEnd) * xGap);
            }

            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(bitmap, ipart(intery) - lineWidth / 2 - 1, x, rfpart(intery));
                    for (int i = -lineWidth / 2; i < (int)Math.Ceiling(lineWidth / 2.0); i++)
                    {
                        plot(bitmap, ipart(intery) + i, x, 255);
                    }
                    plot(bitmap, ipart(intery) + (int)Math.Ceiling(lineWidth / 2.0), x, fpart(intery));
                    intery += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    plot(bitmap, x, ipart(intery) - lineWidth / 2 - 1, rfpart(intery));
                    for (int i = -lineWidth / 2; i < (int)Math.Ceiling(lineWidth / 2.0); i++)
                    {
                        plot(bitmap, x, ipart(intery) + i, 255);
                    }
                    plot(bitmap, x, ipart(intery) + (int)Math.Ceiling(lineWidth / 2.0), fpart(intery));
                    intery += gradient;
                }
            }
        }
        public void WuPaint(Bitmap bitmap, Graphics g, int lineWidth = 1)
        {
            foreach (var p in Points)
            {
                WuLine(p.Value.X, p.Value.Y, p.Value.Next.X, p.Value.Next.Y, bitmap, lineWidth);

                g.FillEllipse(Brushes.Black, p.Value.X - _pointRadius, p.Value.Y - _pointRadius, _pointRadius * 3, _pointRadius * 3);
                g.DrawString(p.Value.Id.ToString(), _font, Brushes.Blue, new PointF(p.Value.X, p.Value.Y));
                p.Value.PaintConstraints(g);
            }
        }
        // https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public void BresenhamLine(float xD, float yD, float x2D, float y2D, Bitmap bitmap)
        {
            int x = (int)Math.Round(xD);
            int y = (int)Math.Round(yD);
            int x2 = (int)Math.Round(x2D);
            int y2 = (int)Math.Round(y2D);
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                if (!(x < 0 || y < 0)) { bitmap.SetPixel(x, y, Color.Black); }

                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
        public void BresenhamPaint(Bitmap bitmap, Graphics g, int lineWidth)
        {
            foreach (var p in Points)
            {
                float a = (p.Value.Y - p.Value.Next.Y) / (p.Value.X - p.Value.Next.X);
                if ((1 > a && a >= 0) || (a < 0 && a > -1))
                {
                    for (int i = -lineWidth / 2; i < (int)Math.Ceiling(lineWidth / 2.0); i++)
                    {
                        BresenhamLine(p.Value.X, p.Value.Y + i, p.Value.Next.X, p.Value.Next.Y + i, bitmap);
                    }
                }
                else
                {
                    for (int i = -lineWidth / 2; i < (int)Math.Ceiling(lineWidth / 2.0); i++)
                    {
                        BresenhamLine(p.Value.X + i, p.Value.Y, p.Value.Next.X + i, p.Value.Next.Y, bitmap);
                    }
                }

                g.FillEllipse(Brushes.Black, p.Value.X - _pointRadius, p.Value.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                g.DrawString(p.Value.Id.ToString(), _font, Brushes.Blue, new PointF(p.Value.X, p.Value.Y));
                p.Value.PaintConstraints(g);
            }
        }
        public Polygon(Pen line, Brush point, int pointRadius)
        {
            _lineColor = line;
            _pointColor = point;
            Points = new Dictionary<int, MyPoint>();
            _pointRadius = pointRadius;
            _font = new Font("Arial", 10);
        }
        public MyPoint GetPointFromId(int id)
        {
            var res = Points.TryGetValue(id, out MyPoint point);
            if (res == false)
            {
                return null;
            }
            else return point;
        }
        public int AddPointAtEnd(float x, float y)
        {
            if (IsPolygonCycle)
            {
                return -1; // Can't add more points to polygon, polygon is already a cycle
            }
            else if (Points.Count != 0)
            {
                MyPoint lastPoint = _lastPoint;
                foreach (var id_p in Points) // If new point closes cycle 
                {
                    MyPoint p = id_p.Value;
                    MyPoint firstPoint = _firstPoint;
                    if (AreTwoPointsNear(x, y, firstPoint.X, firstPoint.Y))
                    {
                        if (Points.Count < 3) return 1;
                        lastPoint.Next = firstPoint;
                        firstPoint.Prev = lastPoint;
                        IsPolygonCycle = true;

                        return 0; // Creation of polygon has just been finished
                    }
                }
                MyPoint newPoint = new MyPoint(x, y, _maxId++, lastPoint, null);
                lastPoint.Next = newPoint;
                Points.Add(newPoint.Id, newPoint);
                _lastPoint = newPoint;
            }
            else
            {
                var newPoint = new MyPoint(x, y, _maxId++, null, null);
                _firstPoint = newPoint;
                _lastPoint = newPoint;
                Points.Add(newPoint.Id, newPoint);
            }

            return 1; // Added new point to polygon
        }
        public bool AreTwoPointsNear(float x1, float y1, float x2, float y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) <= SearchRadiusPoint * SearchRadiusPoint;
        }
        public bool IsPointInBoundingBox(Point point)
        {
            if (IsClickNearSomeLineOrPoint(point) == true) // there is some line of point near click, so we want to move them
            {
                return false;
            }

            var c = new ConvexHullAlgorithms();
            (double, double)[] points = new (double, double)[Points.Count()];
            int i = 0;
            foreach (var id_point in Points)
            {
                MyPoint p = id_point.Value;
                points[i++] = (p.X, p.Y);
            }
            var res = c.ConvexHull(points);
            if (res == null)
            {
                return false;
            }
            var ret = c.IsPointInPolygon(res, (point.X, point.Y));
            return ret;

        }
        // Return points id, if click is within Radius from some point, otherwise -1
        public int IsClickNearSomePoint(PointF p, int id = -1)
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
            foreach (var id_point in Points)
            {
                MyPoint point = id_point.Value;
                if (AreTwoPointsNear(point.X, point.Y, p.X, p.Y))
                {
                    return point.Id;
                }
            }
            return -1;
        }
        public int IsClickNearSomeLine(PointF p, int id = -1)
        {
            if (id != -1)
            {
                MyPoint point = GetPointFromId(id);
                if (point != null && point.Next != null)
                {
                    float minX = Math.Min(point.X, point.Next.X);
                    float maxX = Math.Max(point.X, point.Next.X);
                    float minY = Math.Min(point.Y, point.Next.Y);
                    float maxY = Math.Max(point.Y, point.Next.Y);
                    if (!(minX > p.X || maxX < p.X || minY > p.Y || maxY < p.Y))
                    {
                        float x1 = point.X, y1 = point.Y, x2 = point.Next.X, y2 = point.Next.Y;
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
            foreach (var id_point in Points)
            {
                MyPoint mp = id_point.Value;
                if (mp.Next == null)
                {
                    continue;
                }
                float minX = Math.Min(mp.X, mp.Next.X);
                float maxX = Math.Max(mp.X, mp.Next.X);
                float minY = Math.Min(mp.Y, mp.Next.Y);
                float maxY = Math.Max(mp.Y, mp.Next.Y);
                if (minX > p.X || maxX < p.X || minY > p.Y || maxY < p.Y)
                {
                    continue;
                }

                float x1 = mp.X, y1 = mp.Y, x2 = mp.Next.X, y2 = mp.Next.Y;
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
        public void BasicPaint(Graphics g, int lineWidth)
        {
            foreach (var id_point in Points)
            {
                MyPoint point = id_point.Value;
                g.FillEllipse(Brushes.Black, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                if (point.Next != null)
                {
                    g.DrawLine(new Pen(Brushes.Black, lineWidth), new PointF(point.X, point.Y), new PointF(point.Next.X, point.Next.Y));
                }
                g.DrawString(point.Id.ToString(), _font, Brushes.Brown, new PointF(point.X, point.Y));
                point.PaintConstraints(g);
            }
        }
        public void PaintPolygon(Graphics g, Pen lineColor, Brush pointColor, Pen selectedLineColor, Brush selectedPointColor,
            int pointId = -1, LastSelectedElement lastSelectedElement = LastSelectedElement.POLYGON)
        {
            var selectedPoint = GetPointFromId(pointId);
            if (selectedPoint == null)
            {
                selectedPoint = new MyPoint(0, 0, -1, null, null);
            }
            if (lastSelectedElement == LastSelectedElement.POINT)
            {
                foreach (var id_point in Points)
                {
                    MyPoint point = id_point.Value;
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
                        g.DrawLine(lineColor, new PointF(point.X, point.Y), new PointF(point.Next.X, point.Next.Y));
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Blue, new PointF(point.X, point.Y));
                    point.PaintConstraints(g);
                }
            }
            else if (lastSelectedElement == LastSelectedElement.LINE)
            {

                foreach (var id_point in Points)
                {
                    MyPoint point = id_point.Value;
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
                            g.DrawLine(selectedLineColor, new PointF(point.X, point.Y), new PointF(point.Next.X, point.Next.Y));
                        }
                        else
                        {
                            g.DrawLine(lineColor, new PointF(point.X, point.Y), new PointF(point.Next.X, point.Next.Y));
                        }
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Blue, new PointF(point.X, point.Y));
                    point.PaintConstraints(g);
                }
            }
            else if (lastSelectedElement == LastSelectedElement.POLYGON)
            {
                foreach (var id_point in Points)
                {
                    MyPoint point = id_point.Value;
                    g.FillEllipse(pointColor, point.X - _pointRadius, point.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                    if (point.Next != null)
                    {
                        g.DrawLine(lineColor, new PointF(point.X, point.Y), new PointF(point.Next.X, point.Next.Y));
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Brown, new PointF(point.X, point.Y));
                    point.PaintConstraints(g);
                }
            }
        }
        public void TranslatePoint(float dx, float dy, int id)
        {
            var point = GetPointFromId(id);
            if (point == null)
            {
                return;
            }
            point.TranslatePoint(dx, dy);
        }
        public void TranslateLine(float dx, float dy, int id)
        {
            var point = GetPointFromId(id);
            if (point == null)
            {
                return;
            }
            point.TranslateAsLine(dx, dy);
        }
        public void TranslatePolygon(float dx, float dy)
        {
            foreach (var id_point in Points)
            {
                MyPoint point = id_point.Value;
                point.TranslatePointWithoutExecutingConstraints(dx, dy);
            }
        }
        public void DeletePoint(int id)
        {
            var p = GetPointFromId(id);
            if (p == null) // no such id
            {
                return;
            }
            p.DeleteAssociatedConstraints();
            p.Constraints = new List<Constraint>();
            if (Points.Count() <= 3) // polygon would degenerate
            {
                Points.Clear();
            }
            else // normal delete
            {

                if (p == _firstPoint)
                {
                    _firstPoint = p.Next;
                }
                if (p == _lastPoint)
                {
                    _lastPoint = p.Prev;
                }
                if (p.Prev != null)
                {
                    p.Prev.Next = p.Next;
                }
                if (p.Next != null)
                {
                    p.Next.Prev = p.Prev;
                }
                Points.Remove(p.Id);
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
            p1.DeleteAssociatedConstraints();
            var p2 = GetPointFromId(p1.Next.Id);
            p2.DeleteAssociatedConstraints();
            float x = (p1.X + p2.X) / 2;
            float y = (p1.Y + p2.Y) / 2;
            MyPoint newPoint = new MyPoint(x, y, _maxId++, p1, p2);
            p1.Next = newPoint;
            p2.Prev = newPoint;
            Points.Add(newPoint.Id, newPoint);
        }
    }
}
