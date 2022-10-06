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
        private bool IsPolygonCycle = false;
        public Polygon()
        {
            Points = new List<MyPoint>();
        }
        public int AddPoint(int x, int y)
        {
            if (IsPolygonCycle)
            {
                return -1; // Can't add more points to polygon, polygon is already a cycle
            }
            if (Points.Count != 0)
            {
                MyPoint LastPoint = Points.Last();
                foreach (MyPoint p in Points)
                {
                    MyPoint FirstPoint = Points.First();

                    if (IsCycleClosed(x, y, FirstPoint.X, FirstPoint.Y))
                    {
                        LastPoint.Next = FirstPoint;
                        FirstPoint.Prev = LastPoint;
                        IsPolygonCycle = true;
                        MessageBox.Show("end");

                        return 0; // Creation of polygon has just been finished
                    }
                }

                Points.Add(new MyPoint(x, y, LastPoint.Id + 1, LastPoint, null));
                LastPoint = Points.Last();
            }
            else
            {
                Points.Add(new MyPoint(x, y, 0, null, null));
            }
            return 1; // Added new point to polygon
        }

        private bool IsCycleClosed(int x1, int y1, int x2, int y2)
        {
            const int Radius = 20;
            return Math.Abs(x1 - x2) < Radius && Math.Abs(y1 - y2) < Radius;
        }
    }

}
