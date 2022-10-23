namespace PolygonManipulator
{
    public abstract class Constraint
    {
        public MyPoint MePoint;
        public MyPoint CorespondingPoint;
        public bool _paintThisConstraints;
        protected int _id;

        abstract public void Execute();
        abstract public void DeleteConstraint();
        abstract public bool AssertIfDelete(MyPoint point);
        abstract public bool PaintConstraint(Graphics g, PointF point);
    }

    public class ConstraintParallel : Constraint
    {
        public Polygon MePolygon;
        public Polygon CorespondingPolygon;
        public static int MaxId;

        public ConstraintParallel(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon, int id, bool paintThisConstraint)
        {
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            MePolygon = firstPolygon;
            CorespondingPolygon = secondPolygon;
            _id = id;
            _paintThisConstraints = paintThisConstraint;
        }
        override public void Execute()
        {
            CorespondingPoint.RotatePoint(MePoint, CorespondingPoint);
        }
        override public void DeleteConstraint()
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
        override public bool AssertIfDelete(MyPoint point)
        {
            return MePoint == point || CorespondingPoint == point;
        }
        override public bool PaintConstraint(Graphics g, PointF point)
        {
            if (_paintThisConstraints)
            {
                g.DrawString("||" + _id.ToString(), new Font("Arial", 8), Brushes.Magenta, new PointF((MePoint.X + MePoint.Next.X) / 2 + point.X, (MePoint.Y + MePoint.Next.Y) / 2 + point.Y)); ;
            }
            return _paintThisConstraints;
        }
    }
    public class ConstraintLength : Constraint
    {
        private MyPoint _anchorPoint;
        private float _length;

        public ConstraintLength(MyPoint firstPoint, MyPoint secondPoint, MyPoint anchor, float length, int id, bool paintThisConstraint)
        {
            _length = length;
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            _anchorPoint = anchor;
            _id = id;
            _paintThisConstraints = paintThisConstraint;
        }
        override public void Execute()
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
        override public void DeleteConstraint()
        {
            if (_anchorPoint != MePoint)
            {
                return;
            }
            MePoint.DeleteConstraintsAssociatedWithPoint(MePoint);
            CorespondingPoint.DeleteConstraintsAssociatedWithPoint(MePoint);
        }
        override public bool AssertIfDelete(MyPoint point)
        {
            return _anchorPoint == point;
        }
        override public bool PaintConstraint(Graphics g, PointF offset)
        {
            if (_paintThisConstraints)
            {
                g.DrawString("L" + _id.ToString(), new Font("Arial", 8), Brushes.DarkRed, new PointF((MePoint.X + CorespondingPoint.X) / 2 + offset.X, (MePoint.Y + CorespondingPoint.Y) / 2 + offset.Y));
            }
            return _paintThisConstraints;

        }
    }
}
