namespace PolygonManipulator
{
    public class Constraint
    {
        public MyPoint MePoint;
        public MyPoint CorespondingPoint;
        public Polygon MePolygon;
        public Polygon CorespondingPolygon;
        public Constraint(MyPoint firstPoint, MyPoint secondPoint, Polygon firstPolygon, Polygon secondPolygon)
        {
            MePoint = firstPoint;
            CorespondingPoint = secondPoint;
            MePolygon = firstPolygon;
            CorespondingPolygon = secondPolygon;
        }
    }
}
