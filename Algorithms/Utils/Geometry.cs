using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDSA.Utils
{
    
    public static class Geometry
    {
        public static double Distance(Point first, Point second)
        {
            var result= Math.Abs(Math.Sqrt(Math.Pow((first.X - second.X),2) + Math.Pow((first.Y - second.Y), 2)));

            return result;
        }

        public static Point Middle(Point p1, Point p2)
        {
            
            return new Point((p1.X + p2.X)/2, (p1.Y + p2.Y) / 2);
        }

        public static Point MoveTowords(Point p1,Point p2,double distance)
        {
            //int gcd = GCD(Math.Abs(p2.Y - p1.Y), Math.Abs(p2.X - p1.X));

            //int dx = (p2.X - p1.X) / gcd;
            //int dy = (p2.Y - p1.Y) / gcd;


            //double abs = Geometry.Distance(p1, p2);
            double theta = Math.Atan2(Math.Abs(p2.Y - p1.Y), Math.Abs(p2.X - p1.X));


            return new Point((int)(p1.X+ distance * Math.Cos(theta)), (int)(p1.Y + distance * Math.Sin(theta)));
        }


        private static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

    }
}
