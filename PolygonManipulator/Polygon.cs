namespace PolygonManipulator
{
    public class MyPoint
    {
        public int Id;
        public int X, Y;
        public MyPoint Prev, Next;
        public List<Constraint> Constraints;
        public bool HasConstraintBeenExecuted = false;
        public MyPoint(int x, int y, int id, MyPoint prev, MyPoint next)
        {
            Id = id;
            X = x;
            Y = y;
            Prev = prev;
            Next = next;
            Constraints = new List<Constraint>();
        }

        public void TranslatePoint(int dx, int dy)
        {
            X -= dx;
            Y -= dy;
            ExecuteConstrains();
            HasConstraintBeenExecuted = false;
        }
        public void JustTranslatePoint(int dx, int dy)
        {
            X -= dx;
            Y -= dy;
        }
        public void ExecuteConstrains()
        {
            foreach (Constraint c in Constraints)
            {
                c.Execute();
            }
        }

        public void TranslateAsLine(int dx, int dy)
        {
            X -= dx;
            Y -= dy;
            Next.X -= dx;
            Next.Y -= dy;

            ExecuteConstrains();
            this.HasConstraintBeenExecuted = true;
            Next.ExecuteConstrains();
            this.HasConstraintBeenExecuted = false;
        }
        public void AddConstraintParallel(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon)
        {
            firstPoint.Constraints.Add(new ConstraintParallel(firstPoint, secondPoint, firstPolygon, secondPolygon));
            firstPoint.Next.Constraints.Add(new ConstraintParallel(firstPoint, secondPoint, firstPolygon, secondPolygon));
            secondPoint.Constraints.Add(new ConstraintParallel(secondPoint, firstPoint, secondPolygon, firstPolygon));
            secondPoint.Next.Constraints.Add(new ConstraintParallel(secondPoint, firstPoint, secondPolygon, firstPolygon));

            RotatePoint(firstPoint, secondPoint);


            //MyPoint tmp1 = new MyPoint(firstPoint.X, firstPoint.Y, firstPoint.Id, null, null);
            //MyPoint tmp2 = new MyPoint(secondPoint.X, secondPoint.Y, secondPoint.Id, null, null);
            //tmp1.X += 100;
            //tmp2.X += 100;
            //double slopeTmp1 = ((double)(tmp1.Next.Y - tmp1.Y)) / ((double)(tmp1.Next.X - tmp1.X));
            //double slopeTmp2 = ((double)(tmp2.Next.Y - tmp2.Y)) / ((double)(tmp2.Next.X - tmp2.X));
            //if (Math.Abs(slopeTmp2 - slopeTmp1) < 0.1)
            //{
            //    firstPoint.Constraints.Add(new Constraint(firstPoint, secondPoint, firstPolygon, secondPolygon));
            //    secondPoint.Constraints.Add(new Constraint(secondPoint, firstPoint, secondPolygon, firstPolygon));

            //    firstPoint.Next.Constraints.Add(new Constraint(firstPoint.Next, secondPoint.Next, firstPolygon, secondPolygon));
            //    secondPoint.Next.Constraints.Add(new Constraint(secondPoint.Next, firstPoint.Next, secondPolygon, firstPolygon));

            //}
            //else
            //{

            //}

        }

        public void AddConstraintLength(MyPoint firstPoint, MyPoint secondPoint)
        {
            firstPoint.Constraints.Add(new ConstraintLength(firstPoint, secondPoint));
            secondPoint.Constraints.Add(new ConstraintLength(secondPoint, firstPoint));

            ExecuteConstrains();
        }
        public void RotatePoint(MyPoint p1, MyPoint p2)
        {
            if (HasConstraintBeenExecuted)
            {
                return;
            }
            //double theta1 = Math.Atan2(p1.Y - p1.Next.Y, p1.X - p1.Next.X);
            //double theta2 = Math.Atan2(p2.Y - p2.Next.Y, p2.X - p2.Next.X);
            //double diff = Math.Abs(theta1 - theta2);
            //double angle = Math.Min(diff, Math.Abs(Math.PI - diff));

            //angle = -theta2;
            //double v = (Math.Cos(angle) * p2.Next.X - Math.Sin(angle) * p2.Next.Y);
            //double u = (Math.Sin(angle) * p2.Next.X + Math.Cos(angle) * p2.Next.Y);

            //double check = Math.Atan2(p2.Y - u, p2.X - v);
            //angle = theta1;
            //v = (Math.Cos(angle) * v - Math.Sin(angle) * u);
            //u = (Math.Sin(angle) * v + Math.Cos(angle) * u);

            //check = Math.Atan2(p2.Y - u, p2.X - v);
            //p2.Next.X = ((int)Math.Round(v));
            ////Math.Round(v);
            //p2.Next.Y = ((int)Math.Round(u));
            if (p1.Next != p2 && p2.Next != p1)
            //if (false)
            {
                double x1 = p2.X;
                double x2 = p2.Next.X;
                double y1 = p2.Y;
                double y2 = p2.Next.Y;

                double dist2 = (y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1);
                if (p1.Next.X - p1.X == 0) // case when line line (p1,p1.next) is vertical
                {
                    return;
                    //double v = ((double)(x1 + x2)) / 2.0;
                    //double y1_y2_sum = y1 + y2;
                    //double A = 2;
                    //double B = -2 * y1_y2_sum;
                    //double C = y1_y2_sum * y1_y2_sum - dist2;
                    //if (B * B - 4 * A * C < 0)
                    //{
                    //    //MessageBox.Show("delta<0");
                    //    return;
                    //}
                    //double u1_p = (-B + Math.Sqrt(B * B - 4 * A * C)) / 2 / A;
                    //double u1_m = (-B - Math.Sqrt(B * B - 4 * A * C)) / 2 / A;
                    //double u2_p = y1_y2_sum - u1_p;
                    //double u2_m = y1_y2_sum - u1_m;

                    //double mse_m = +Math.Pow(y1 - u1_m, 2) + Math.Pow(y2 - u2_m, 2);
                    //double mse_p = Math.Pow(y1 - u1_p, 2) + Math.Pow(y2 - u2_p, 2);

                    //if (mse_m < mse_p)
                    //{
                    //    p2.X = (int)Math.Round(v);
                    //    p2.Next.X = (int)Math.Round(v);
                    //    p2.Y = (int)Math.Round(u1_m);
                    //    p2.Next.Y = (int)Math.Round(u2_m);
                    //}
                    //else
                    //{

                    //    p2.X = (int)Math.Round(v);
                    //    p2.Next.X = (int)Math.Round(v);
                    //    p2.Y = (int)Math.Round(u1_p);
                    //    p2.Next.Y = (int)Math.Round(u2_p);

                    //}
                    //return;
                }
                double slope = ((double)(p1.Next.Y - p1.Y)) / ((double)(p1.Next.X - p1.X));

                double v1_plus = 0.5 * (x1 + x2 + Math.Sqrt(dist2 / (1.0 + slope * slope)));
                double v2_plus = x1 + x2 - v1_plus;

                double u1_plus = 0.5 * (y1 + y2 - slope * (x1 + x2 - 2.0 * v1_plus));
                double u2_plus = y1 + y2 - u1_plus;

                double v1_minus = 0.5 * (x1 + x2 - Math.Sqrt(dist2 / (1.0 + slope * slope)));
                double v2_minus = x1 + x2 - v1_minus;

                double u1_minus = 0.5 * (y1 + y2 - slope * (x1 + x2 - 2.0 * v1_minus));
                double u2_minus = y1 + y2 - u1_minus;

                //double x1_x2 = x1 + x2;
                //double v1_v2_plus = v1_plus + v2_plus;
                //double v1_v2_minus = v1_minus + v2_minus;

                //double y1_y2 = y1 + y2;
                //double u1_u2_plus = u1_plus + u2_plus;
                //double u1_u2_minuns = u1_minus + u2_minus;
                double mse_minus = Math.Pow(x1 - v1_minus, 2) + Math.Pow(x2 - v2_minus, 2) + Math.Pow(y1 - u1_minus, 2) + Math.Pow(y2 - u2_minus, 2);
                double mse_plus = Math.Pow(x1 - v1_plus, 2) + Math.Pow(x2 - v2_plus, 2) + Math.Pow(y1 - u1_plus, 2) + Math.Pow(y2 - u2_plus, 2);
                //double mse_minus= (Math.Abs(x1 - v1_minus) + Math.Abs(x2 - v2_minus) + Math.Abs(y1 - u1_minus) + Math.Abs(y2 - u2_minus)
                //double slope_minus = (u2_minus - u1_minus) / (v2_minus - v1_minus);
                //double slope_plus = (u2_plus - u1_plus) / (v2_plus - v1_plus);
                if (mse_minus < mse_plus)
                {
                    p2.X = (int)Math.Round(v1_minus);
                    p2.Next.X = (int)Math.Round(v2_minus);
                    p2.Y = (int)Math.Round(u1_minus);
                    p2.Next.Y = (int)Math.Round(u2_minus);
                }
                else
                {

                    p2.X = (int)Math.Round(v1_plus);
                    p2.Next.X = (int)Math.Round(v2_plus);
                    p2.Y = (int)Math.Round(u1_plus);
                    p2.Next.Y = (int)Math.Round(u2_plus);

                }
            }

            //double dist2_out_exact_plus = (v1_plus - v2_plus) * (v1_plus - v2_plus) + (u1_plus - u2_plus) * (u1_plus - u2_plus);
            //double dist2_out_exact_minus = (v1_minus - v2_minus) * (v1_minus - v2_minus) + (u1_minus - u2_minus) * (u1_minus - u2_minus);
            //double slope_out_exact_plus = (u2_plus - u1_plus) / (v2_plus - v1_plus);
            //double slope_out_exact_minus = (u2_minus - u1_minus) / (v2_minus - v1_minus);

            //double dist2_out = (p2.X - p2.Next.X) * (p2.X - p2.Next.X) + (p2.Y - p2.Next.Y) * (p2.Y - p2.Next.Y);
            //double slope_out_original_line_based_off = ((double)(p1.Next.Y - p1.Y)) / ((double)(p1.Next.X - p1.X));
            //double slope_out_2_old_line_after_change = ((double)(p2.Next.Y - p2.Y)) / ((double)(p2.Next.X - p2.X));
            //if (Math.Abs(slope_out_2_old_line_after_change - slope) > 0.1)
            //{
            //    return;
            //}
            //return;
            else
            {
                double x1 = 0;
                double x2 = 0;
                double y1 = 0;
                double y2 = 0;
                double slope = 0;

                if (p1.Next == p2)
                {
                    x1 = p2.X;
                    x2 = p2.Next.X;
                    y1 = p2.Y;
                    y2 = p2.Next.Y;
                    slope = ((double)(p1.Next.Y - p1.Y)) / ((double)(p1.Next.X - p1.X));
                }
                else if (p2.Next == p1)
                {
                    x1 = p2.Next.X;
                    x2 = p2.X;
                    y1 = p2.Next.Y;
                    y2 = p2.Y;
                    slope = ((double)(p1.Next.Y - p1.Y)) / ((double)(p1.Next.X - p1.X));//why isint it before if??
                }
                else
                {
                    MessageBox.Show("rotate line some error ");
                }
                double dist2 = (y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1);

                if (p1.Next.X - p1.X == 0) // case when line line (p1,p1.next) is vertical
                {
                    return;
                    //double v = ((double)(x1 + x2)) / 2.0;
                    //double y1_y2_sum = y1 + y2;
                    //double A = 2;
                    //double B = -2 * y1_y2_sum;
                    //double C = y1_y2_sum * y1_y2_sum - dist2;
                    //if (B * B - 4 * A * C < 0)
                    //{
                    //    //MessageBox.Show("delta<0");
                    //    return;
                    //}
                    //double u1_p = (-B + Math.Sqrt(B * B - 4 * A * C)) / 2 / A;
                    //double u1_m = (-B - Math.Sqrt(B * B - 4 * A * C)) / 2 / A;
                    //double u2_p = y1_y2_sum - u1_p;
                    //double u2_m = y1_y2_sum - u1_m;

                    //double u1_u =


                    //double mse_m = +Math.Pow(y1 - u1_m, 2) + Math.Pow(y2 - u2_m, 2);
                    //double mse_p = Math.Pow(y1 - u1_p, 2) + Math.Pow(y2 - u2_p, 2);

                    //if (mse_m < mse_p)
                    //{
                    //    p2.X = (int)Math.Round(v);
                    //    p2.Next.X = (int)Math.Round(v);
                    //    p2.Y = (int)Math.Round(u1_m);
                    //    p2.Next.Y = (int)Math.Round(u2_m);
                    //}
                    //else
                    //{

                    //    p2.X = (int)Math.Round(v);
                    //    p2.Next.X = (int)Math.Round(v);
                    //    p2.Y = (int)Math.Round(u1_p);
                    //    p2.Next.Y = (int)Math.Round(u2_p);

                    //}
                    //return;
                }

                double D = -slope * x1;
                double A = 1.0 + slope * slope;
                double B = -2 * x1 + 2 * D * slope;
                double C = x1 * x1 + D * D - dist2;
                if (B * B - 4 * A * C < 0)
                {
                    return;
                }
                double v1 = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2.0 * A);
                double v2 = (-B - Math.Sqrt(B * B - 4 * A * C)) / (2.0 * A);
                double u1 = v1 * slope + D + y1;
                double u2 = v2 * slope + D + y1;

                //double sv1 = v1;
                //double su1 = u1;
                //double sv2 = v2;
                //double su2 = u2;
                double diff11 = Math.Pow(v1 - x2, 2) + Math.Pow(u1 - y2, 2);
                double diff12 = Math.Pow(v2 - x2, 2) + Math.Pow(u2 - y2, 2);

                if (diff11 < diff12)
                {
                    if (p1.Next == p2)
                    {
                        p2.Next.X = (int)Math.Round(v1);
                        p2.Next.Y = (int)Math.Round(u1);
                    }
                    else
                    {
                        p2.X = (int)Math.Round(v1);
                        p2.Y = (int)Math.Round(u1);
                    }
                }
                else
                {
                    if (p1.Next == p2)
                    {
                        p2.Next.X = (int)Math.Round(v2);
                        p2.Next.Y = (int)Math.Round(u2);
                    }
                    else
                    {
                        p2.X = (int)Math.Round(v2);
                        p2.Y = (int)Math.Round(u2);
                    }
                }
            }
            HasConstraintBeenExecuted = true;
            ExecuteConstrains();
            HasConstraintBeenExecuted = false;
        }
        public void DeleteAssociatedConstraints()
        {
            //foreach (var c in Constraints)
            int len = Constraints.Count();
            for (int i = 0; i < len; ++i)
            {
                //Constraints[i].MePoint.DeleteConstraintsAssociatedWithPoint(this);
                //if (i >= Constraints.Count())
                //    continue;
                //Constraints[i].CorespondingPoint.DeleteConstraintsAssociatedWithPoint(this);
                //Constraints[i].CorespondingPoint.Next.DeleteConstraintsAssociatedWithPoint(this);
                Constraints[i].DeleteConstraint(this);
                len = Constraints.Count();
            }
            Next.DeleteConstraintsAssociatedWithPoint(this);
            Prev.DeleteConstraintsAssociatedWithPoint(this);


        }
        public void DeleteConstraintsAssociatedWithPoint(MyPoint point)
        {
            List<Constraint> constraintsToDelete = new List<Constraint>();
            foreach (var c in Constraints)
            {
                //if (c.MePoint == point || c.CorespondingPoint == point)

                if (c.AssertIfDelete(this))
                {
                    //pointsToDelete.Add(c)
                    constraintsToDelete.Add(c);
                    //Constraints.Remove(c);
                }
            }
            foreach (var c in constraintsToDelete)
            {
                Constraints.Remove(c);
            }
        }
        public void PaintConstraints(Graphics g)
        {
            foreach (var c in Constraints)
            {
                c.PaintConstraint(g);
            }
        }
    }

    public class Polygon
    {
        public Dictionary<int, MyPoint> Points { get; set; }
        public bool IsPolygonCycle = false;
        const int SearchRadiusPoint = 8;
        const int SearchRadiusLine = 8;
        private int _pointRadius;
        private Brush _pointColor;
        private Pen _lineColor;
        private Font _font;
        private int _maxId = 0;
        private MyPoint _lastPoint;
        private MyPoint _firstPoint;

        public void MyDrawLine(int x, int y, int x2, int y2, Bitmap bitmap)
        {
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
        public void OwnPaint(Bitmap bitmap, Graphics g)
        {
            foreach (var p in Points)
            {
                MyDrawLine(p.Value.X, p.Value.Y, p.Value.Next.X, p.Value.Next.Y, bitmap);
                MyDrawLine(p.Value.X + 2, p.Value.Y, p.Value.Next.X + 2, p.Value.Next.Y, bitmap);
                MyDrawLine(p.Value.X + 1, p.Value.Y, p.Value.Next.X + 1, p.Value.Next.Y, bitmap);
                MyDrawLine(p.Value.X, p.Value.Y + 1, p.Value.Next.X, p.Value.Next.Y + 1, bitmap);
                MyDrawLine(p.Value.X, p.Value.Y + 2, p.Value.Next.X, p.Value.Next.Y + 2, bitmap);

                g.FillEllipse(Brushes.Black, p.Value.X - _pointRadius, p.Value.Y - _pointRadius, _pointRadius * 2, _pointRadius * 2);
                g.DrawString(p.Value.Id.ToString(), _font, Brushes.Blue, new Point(p.Value.X, p.Value.Y));
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
            //}
            var res = Points.TryGetValue(id, out MyPoint point);
            if (res == false)
            {
                return null;

            }
            else return point;
        }
        public int AddPointAtEnd(int x, int y)
        {
            if (IsPolygonCycle)
            {
                return -1; // Can't add more points to polygon, polygon is already a cycle
            }
            if (Points.Count != 0)
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
        public bool AreTwoPointsNear(int x1, int y1, int x2, int y2)
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
            foreach (var id_point in Points)
            {
                MyPoint mp = id_point.Value;
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
                        g.DrawLine(lineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Blue, new Point(point.X, point.Y));
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
                            g.DrawLine(selectedLineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                        }
                        else
                        {
                            g.DrawLine(lineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                        }
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Blue, new Point(point.X, point.Y));
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
                        g.DrawLine(lineColor, new Point(point.X, point.Y), new Point(point.Next.X, point.Next.Y));
                    }
                    g.DrawString(point.Id.ToString(), _font, Brushes.Brown, new Point(point.X, point.Y));
                    point.PaintConstraints(g);
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
            point.TranslatePoint(dx, dy);
        }
        public void TranslateLine(int dx, int dy, int id)
        {

            //TranslatePoint(dx, dy, id);
            var point = GetPointFromId(id);
            if (point == null)
            {
                return;
            }
            point.TranslateAsLine(dx, dy);
            //id = point.Next.Id;
            //TranslatePoint(dx, dy, id);
        }
        public void TranslatePolygon(int dx, int dy)
        {
            foreach (var id_point in Points)
            {
                MyPoint point = id_point.Value;
                //TranslatePoint(dx, dy, point.Id);
                point.JustTranslatePoint(dx, dy);
            }
        }
        public void DeletePoint(int id)
        {
            var p = GetPointFromId(id);

            //p.Prev.Constraints = new List<Constraint>();
            if (p == null) // new such id
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
            var p2 = GetPointFromId(p1.Next.Id);
            int x = (p1.X + p2.X) / 2;
            int y = (p1.Y + p2.Y) / 2;
            MyPoint newPoint = new MyPoint(x, y, _maxId++, p1, p2);

            p1.Next = newPoint;
            p2.Prev = newPoint;

            Points.Add(newPoint.Id, newPoint);
        }
    }
}
