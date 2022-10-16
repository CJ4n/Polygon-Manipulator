namespace PolygonManipulator
{
    public interface Constraint
    {
        public void Execute();
        public void DeleteConstraint();
        public bool AssertIfDelete(MyPoint point);
        public void PaintConstraint(Graphics g);
    }

    public class ConstraintParallel : Constraint
    {
        public MyPoint MePoint;
        public MyPoint CorespondingPoint;
        public Polygon MePolygon;
        public Polygon CorespondingPolygon;
        public ConstraintParallel(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon)
        {
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            MePolygon = firstPolygon;
            CorespondingPolygon = secondPolygon;
        }
        public void Execute()
        {
            CorespondingPoint.RotatePoint(MePoint, CorespondingPoint);
        }
        public void DeleteConstraint()
        {
            foreach (var p in CorespondingPolygon.Points)
            {
                p.Value.DeleteConstraintsAssociatedWithPoint(MePoint);
            }
            foreach (var p in MePolygon.Points)
            {
                p.Value.DeleteConstraintsAssociatedWithPoint(MePoint);
            }
        }
        public bool AssertIfDelete(MyPoint point)
        {
            return MePoint == point || CorespondingPoint == point;
        }
        public void PaintConstraint(Graphics g)
        {
            g.DrawString("||", new Font("Arial", 10), Brushes.Magenta, new PointF((MePoint.X + MePoint.Next.X) / 2, (MePoint.Y + MePoint.Next.Y) / 2));
        }

    }
    public class ConstraintLength : Constraint
    {
        public MyPoint MePoint;
        public MyPoint CorespondingPoint;
        private float _length;
        public ConstraintLength(MyPoint firstPoint, MyPoint secondPoint, float length)
        {
            _length = length;
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
        }
        public void Execute()
        {
            if (CorespondingPoint.HasConstraintBeenExecuted)
            {
                return;
            }
            float curLength = (float)Math.Sqrt(Math.Pow(MePoint.X - CorespondingPoint.X, 2) + Math.Pow(MePoint.Y - CorespondingPoint.Y, 2));
            if (Math.Abs(_length - curLength) < 0.001)
            {
                return;
            }
            float ratio = _length / curLength;
            float newX = (CorespondingPoint.X - MePoint.X) * ratio + MePoint.X;
            float newY = (CorespondingPoint.Y - MePoint.Y) * ratio + MePoint.Y;
            CorespondingPoint.X = newX;
            CorespondingPoint.Y = newY;
            CorespondingPoint.ExecuteConstrains();

        }
        public void DeleteConstraint()
        {
            var p = MePoint;
            p.DeleteConstraintsAssociatedWithPoint(MePoint);
            p = p.Next;
            while (p != MePoint)
            {
                p.DeleteConstraintsAssociatedWithPoint(MePoint);
                p = p.Next;
            }
        }
        public bool AssertIfDelete(MyPoint point)
        {
            return MePoint == point || CorespondingPoint == point;
        }
        public void PaintConstraint(Graphics g)
        {
            g.DrawString("L", new Font("Arial", 10), Brushes.Black, new PointF((MePoint.X + CorespondingPoint.X) / 2, (MePoint.Y + CorespondingPoint.Y) / 2));
        }

    }
}
