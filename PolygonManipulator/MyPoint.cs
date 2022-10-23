namespace PolygonManipulator
{
    public class MyPoint
    {
        public int Id;
        public float X, Y;
        public MyPoint Prev, Next;
        public List<Constraint> Constraints;
        public bool HasConstraintBeenExecuted = false;
        public static int MaxConstraitnId = 0;
        public MyPoint(float x, float y, int id, MyPoint prev, MyPoint next)
        {
            Id = id;
            X = x;
            Y = y;
            Prev = prev;
            Next = next;
            Constraints = new List<Constraint>();
        }
        public void TranslatePoint(float dx, float dy)
        {
            X -= dx;
            Y -= dy;
            ExecuteConstrains();
        }
        public void TranslatePointWithoutExecutingConstraints(float dx, float dy)
        {
            X -= dx;
            Y -= dy;
        }
        public void ExecuteConstrains()
        {
            HasConstraintBeenExecuted = true;
            foreach (Constraint c in Constraints)
            {
                c.Execute();
            }
            HasConstraintBeenExecuted = false;
        }
        public void TranslateAsLine(float dx, float dy)
        {
            X -= dx;
            Y -= dy;
            Next.X -= dx;
            Next.Y -= dy;

            ExecuteConstrains();
            Next.ExecuteConstrains();
        }
        private bool _constraintsDeleting = false;
        public void AddConstraintParallel(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon)
        {
            if (firstPoint.Next == secondPoint || firstPoint == secondPoint.Next)
            {
                MessageBox.Show("Can't make adjacent lines parallel");
                return;
            }
            firstPoint.Constraints.Add(new ConstraintParallel(firstPoint, secondPoint, firstPolygon, secondPolygon, MaxConstraitnId, true));
            firstPoint.Next.Constraints.Add(new ConstraintParallel(firstPoint, secondPoint, firstPolygon, secondPolygon, MaxConstraitnId, false));
            secondPoint.Constraints.Add(new ConstraintParallel(secondPoint, firstPoint, secondPolygon, firstPolygon, MaxConstraitnId, true));
            secondPoint.Next.Constraints.Add(new ConstraintParallel(secondPoint, firstPoint, secondPolygon, firstPolygon, MaxConstraitnId, false));
            MaxConstraitnId++;
            RotatePoint(firstPoint, secondPoint);
        }
        public void AddConstraintLength(MyPoint firstPoint, MyPoint secondPoint, float length)
        {
            firstPoint.Constraints.Add(new ConstraintLength(firstPoint, secondPoint, firstPoint, length, MaxConstraitnId, true));
            secondPoint.Constraints.Add(new ConstraintLength(secondPoint, firstPoint, firstPoint, length, MaxConstraitnId, false));
            MaxConstraitnId++;
            ExecuteConstrains();
        }
        // Rotates point around ((p1.x+p2.x)/2,(p1.y+p2.y)/2)
        public void RotatePoint(MyPoint p1, MyPoint p2)
        {
            if (HasConstraintBeenExecuted)
            {
                return;
            }
            if (p1.Next != p2 && p2.Next != p1)
            {
                float x1 = p2.X;
                float x2 = p2.Next.X;
                float y1 = p2.Y;
                float y2 = p2.Next.Y;

                double dist2 = (y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1);
                if (Math.Abs(p1.Next.X - p1.X) < 10e-6) // case when line (p1,p1.next) is very close to vertical 
                {
                    return;
                }
                double slope = (p1.Next.Y - p1.Y) / (p1.Next.X - p1.X);

                double v1_plus = 0.5 * (x1 + x2 + Math.Sqrt(dist2 / (1.0 + slope * slope)));
                double v2_plus = x1 + x2 - v1_plus;

                double u1_plus = 0.5 * (y1 + y2 - slope * (x1 + x2 - 2.0 * v1_plus));
                double u2_plus = y1 + y2 - u1_plus;

                double v1_minus = 0.5 * (x1 + x2 - Math.Sqrt(dist2 / (1.0 + slope * slope)));
                double v2_minus = x1 + x2 - v1_minus;

                double u1_minus = 0.5 * (y1 + y2 - slope * (x1 + x2 - 2.0 * v1_minus));
                double u2_minus = y1 + y2 - u1_minus;

                double mse_minus = Math.Pow(x1 - v1_minus, 2) + Math.Pow(x2 - v2_minus, 2) + Math.Pow(y1 - u1_minus, 2) + Math.Pow(y2 - u2_minus, 2);
                double mse_plus = Math.Pow(x1 - v1_plus, 2) + Math.Pow(x2 - v2_plus, 2) + Math.Pow(y1 - u1_plus, 2) + Math.Pow(y2 - u2_plus, 2);
                if (mse_minus < mse_plus)
                {
                    p2.X = (float)v1_minus;
                    p2.Next.X = (float)v2_minus;
                    p2.Y = (float)u1_minus;
                    p2.Next.Y = (float)u2_minus;
                }
                else
                {
                    p2.X = (float)v1_plus;
                    p2.Next.X = (float)v2_plus;
                    p2.Y = (float)u1_plus;
                    p2.Next.Y = (float)u2_plus;
                }
            }
            else // case when we want to make two adjecant lines parallel,
                 // though in current version of program such operation is prohiibted, I leave the code, 
                 // so basically it will never run
            {
                float x1 = 0;
                float x2 = 0;
                float y1 = 0;
                float y2 = 0;
                double slope = 0;

                if (p1.Next == p2)
                {
                    x1 = p2.X;
                    x2 = p2.Next.X;
                    y1 = p2.Y;
                    y2 = p2.Next.Y;
                    slope = (p1.Next.Y - p1.Y) / (p1.Next.X - p1.X);
                }
                else if (p2.Next == p1)
                {
                    x1 = p2.Next.X;
                    x2 = p2.X;
                    y1 = p2.Next.Y;
                    y2 = p2.Y;
                    slope = (p1.Next.Y - p1.Y) / (p1.Next.X - p1.X); //why isint it before if??
                }
                else
                {
                    MessageBox.Show("rotate line some error ");
                }
                double dist2 = (y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1);

                if (Math.Abs(p1.Next.X - p1.X) < 10e-6) // case when line (p1,p1.next) is very close to vertical 
                {
                    return;
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

                double diff11 = Math.Pow(v1 - x2, 2) + Math.Pow(u1 - y2, 2);
                double diff12 = Math.Pow(v2 - x2, 2) + Math.Pow(u2 - y2, 2);

                if (diff11 < diff12)
                {
                    if (p1.Next == p2)
                    {
                        p2.Next.X = (float)v1;
                        p2.Next.Y = (float)u1;
                    }
                    else
                    {
                        p2.X = (float)v1;
                        p2.Y = (float)u1;
                    }
                }
                else
                {
                    if (p1.Next == p2)
                    {
                        p2.Next.X = (float)v2;
                        p2.Next.Y = (float)u2;
                    }
                    else
                    {
                        p2.X = (float)v2;
                        p2.Y = (float)u2;
                    }
                }
            }
            ExecuteConstrains();
        }
        public void DeleteAssociatedConstraints()
        {

            Dictionary<MyPoint, MyPoint> points = new Dictionary<MyPoint, MyPoint>();
            foreach (var c in Constraints)
            {
                points.TryAdd(c.MePoint, c.MePoint);
                points.TryAdd(c.CorespondingPoint, c.CorespondingPoint);
            }
            foreach (var p in points)
            {
                p.Value.DeleteConstraintsAssociatedWithPoint(this);
                p.Value.Next.DeleteConstraintsAssociatedWithPoint(this);
                p.Value.Prev.DeleteConstraintsAssociatedWithPoint(this);
            }
        }
        public void DeleteConstraintsAssociatedWithPoint(MyPoint point)
        {
            if (_constraintsDeleting)
            {
                return;
            }
            List<Constraint> constraintsToDelete = new List<Constraint>();
            foreach (var c in Constraints)
            {
                if (c.AssertIfDelete(point))
                {
                    constraintsToDelete.Add(c);
                }
            }
            foreach (var c in constraintsToDelete)
            {
                Constraints.Remove(c);
            }
        }
        public void PaintConstraints(Graphics g)
        {
            int constraintCount = 0;
            foreach (var c in Constraints)
            {

                var offset = new PointF(20 * constraintCount, constraintCount);

                if (c.PaintConstraint(g, offset) == false)
                {
                    continue;
                }
                constraintCount++;
            }
        }
        public int HowManyMeaningfulConstraints()
        {
            int sum = 0;
            foreach (var c in Constraints)
            {
                if (c.AssertIfDelete(this))
                {
                    sum++;
                }
            }
            return sum;
        }
    }
}
