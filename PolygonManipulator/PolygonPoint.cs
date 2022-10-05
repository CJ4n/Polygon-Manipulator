using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonManipulator
{

    class MyPoint
    {
        int Id;
        int X, Y;
        Point Prev, Next;

    }
    public class Polygon
    {
        Dictionary<int, MyPoint> Points;
        public Polygon()
        {
            Points = new Dictionary<int, MyPoint>();
        }
    }

}
