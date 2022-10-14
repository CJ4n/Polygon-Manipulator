namespace PolygonManipulator
{
    public interface Constraint
    {
        //public MyPoint MePoint;
        //public MyPoint CorespondingPoint;
        //public Polygon MePolygon;
        //public Polygon CorespondingPolygon;
        //public Constraint(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon)
        //{
        //    MePoint = firstPoint;
        //    CorespondingPoint = secondPoint;
        //    MePolygon = firstPolygon;
        //    CorespondingPolygon = secondPolygon;
        //}
        public void Execute();
        public void DeleteConstraint(MyPoint point);
        public bool AssertIfDelete(MyPoint point);
        public void PaintConstraint(Graphics g);
        //{
        //CorespondingPoint.RotatePoint(MePoint, CorespondingPoint);
        //}
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
        public void DeleteConstraint(MyPoint point)
        {
            CorespondingPoint.DeleteConstraintsAssociatedWithPoint(point);
            CorespondingPoint.Next.DeleteConstraintsAssociatedWithPoint(point);
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
        private double _length;
        private int _x;
        private int _y;
        public ConstraintLength(MyPoint firstPoint, MyPoint secondPoint)
        {
            _length = Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            _x = firstPoint.X;
            _y = firstPoint.Y;
        }
        public void Execute()
        {
            if (Math.Abs(_length - Math.Sqrt(Math.Pow(MePoint.X - CorespondingPoint.X, 2) + Math.Pow(MePoint.Y - CorespondingPoint.Y, 2))) < 10e-4)
            {
                return;
            }
            //CorespondingPoint.TranslatePoint(MePoint.X - _x, MePoint.Y - _y);
            CorespondingPoint.X += MePoint.X - _x;
            CorespondingPoint.Y += MePoint.Y - _y;
            _x = MePoint.X;
            _y = MePoint.Y;
            _length = Math.Sqrt(Math.Pow(MePoint.X - CorespondingPoint.X, 2) + Math.Pow(MePoint.Y - CorespondingPoint.Y, 2));

        }
        public void DeleteConstraint(MyPoint point)
        {
            CorespondingPoint.DeleteConstraintsAssociatedWithPoint(point);
            CorespondingPoint.Next.DeleteConstraintsAssociatedWithPoint(point);
        }
        public bool AssertIfDelete(MyPoint point)
        {
            return MePoint == point || CorespondingPoint == point;
        }
        public void PaintConstraint(Graphics g)
        {
            g.DrawString("L", new Font("Arial", 10), Brushes.Magenta, new PointF((MePoint.X + CorespondingPoint.X) / 2, (MePoint.Y + CorespondingPoint.Y) / 2));

        }

    }
}
