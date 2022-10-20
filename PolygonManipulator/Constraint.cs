namespace PolygonManipulator
{
    public interface Constraint
    {

        public void Execute();
        public void DeleteConstraint();
        public bool AssertIfDelete(MyPoint point);
        public bool PaintConstraint(Graphics g, PointF point);
        public (MyPoint, MyPoint) GetPoints();
    }

    public class ConstraintParallel : Constraint
    {
        public MyPoint MePoint;
        public MyPoint CorespondingPoint;
        public Polygon MePolygon;
        public Polygon CorespondingPolygon;
        public static int MaxId;
        private int _id;
        public bool _paintThisConstraints;
        public ConstraintParallel(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon, int id, bool paintThisConstraint)
        {
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            MePolygon = firstPolygon;
            CorespondingPolygon = secondPolygon;
            _id = id;
            _paintThisConstraints = paintThisConstraint;

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
        public bool PaintConstraint(Graphics g, PointF point)
        {
            if (_paintThisConstraints)
            {
                g.DrawString("||" + _id.ToString(), new Font("Arial", 8), Brushes.Magenta, new PointF((MePoint.X + MePoint.Next.X) / 2 + point.X, (MePoint.Y + MePoint.Next.Y) / 2 + point.Y)); ;
            }
            return _paintThisConstraints;
        }
        public (MyPoint, MyPoint) GetPoints()
        {
            return (MePoint, CorespondingPoint);
        }


    }
    public class ConstraintLength : Constraint
    {
        public MyPoint MePoint;
        public MyPoint CorespondingPoint;
        private MyPoint _anchorPoint;
        private float _length;
        private int _id;
        public bool _paintThisConstraints;
        public ConstraintLength(MyPoint firstPoint, MyPoint secondPoint, MyPoint anchor, float length, int id, bool paintThisConstraint)
        {
            _length = length;
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            _anchorPoint = anchor;
            _id = id;
            _paintThisConstraints = paintThisConstraint;
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
            if (_anchorPoint != MePoint)
            {
                return;
            }
            MePoint.DeleteConstraintsAssociatedWithPoint(MePoint);
            CorespondingPoint.DeleteConstraintsAssociatedWithPoint(MePoint);
        }
        public bool AssertIfDelete(MyPoint point)
        {
            return _anchorPoint == point;
        }
        public bool PaintConstraint(Graphics g, PointF offset)
        {
            if (_paintThisConstraints)
            {
                g.DrawString("L" + _id.ToString(), new Font("Arial", 6), Brushes.Black, new PointF((MePoint.X + CorespondingPoint.X) / 2 + offset.X, (MePoint.Y + CorespondingPoint.Y) / 2 + offset.Y));
            }
            return _paintThisConstraints;

        }
        public (MyPoint, MyPoint) GetPoints()
        {
            return (MePoint, CorespondingPoint);
        }
    }
}
