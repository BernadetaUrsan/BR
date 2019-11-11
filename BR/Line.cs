using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR
{
    class Line
    {
        public Point p1;
        public Point p2;
        public double a; //parametrii ecuatiei liniei (dreptei respective)
        public double b;
        public double c;

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            SetEquationParameters(p1, p2);
        }

        private void SetEquationParameters(Point p1, Point p2)
        {

            int kx;
            int ky;
            kx = p2.x - p1.x;
            ky = p2.y - p1.y;
            a = ky;
            b = -kx;
            c = ky * p1.x - kx * p1.y;

        }
    }
}
