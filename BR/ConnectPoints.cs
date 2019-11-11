using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR
{
    class ConnectPoints
    {
        private List<Point> points;
        private List<Point> figurePoints;


        public ConnectPoints(List<Point> points)
        {
            this.points = points;
            figurePoints = null; //will be set by GetFigureLines
        }


        public List<Line> GetFigureLines()
        {
            List<Line> lines = new List<Line>();
            List <Point> infasuratoare = new List<Point>();
            Line l;
            Point start = new Point();


            start = getStartingPoint();

            infasuratoare = PuncteInfasuratoare(points, start);
            figurePoints = new List<Point>();
            figurePoints = infasuratoare;

            for (int i = 0; i <infasuratoare.Count()-1; i++)
            {
                l = new Line(infasuratoare[i], infasuratoare[i + 1]);
                lines.Add(l);
            }

            l = new Line(infasuratoare[0], infasuratoare[infasuratoare.Count() - 1]);
            lines.Add(l);

            return lines;
        }


        private int getSmallestX()
        {
            int x = 4000;

            foreach (Point p in points)
            {
                if (p.x < x)
                {
                    x = p.x;
                }
            }
            return x;
        }

        private Point getStartingPoint()
        {
            //porneste de la pct cu cel mai mic x
            //daca sunt mai multi ia-l pe cel cu cel mai mic y
            List<Point> candidates = new List<Point>() ;
            Point start = new Point();
            int minX = getSmallestX();

            foreach (Point p in points)
            {
                if (p.x == minX)
                {
                    candidates.Add(p);
                }
            }


            int nrPct = candidates.Count();
            if (nrPct == 0)
            {
                //nu ar trebui sa se ajunga aici
                throw new InvalidOperationException();
            }
            else if (nrPct == 1)
            {
                return candidates.ElementAt(0);
            }
            else
            {
                //mai mult de un candidat cu acelasi x = xMin
                start.y = 4000;
                foreach (Point p in candidates)
                {
                    if (p.y <= start.y)
                    {
                        start = p;
                    }
                }
                return start;
            }

            

        }

        public List<Point> PuncteInfasuratoare(List<Point> points, Point start)
        {
            //codul luat de pe net https://stackoverflow.com/questions/10020949/gift-wrapping-algorithm
            //foloseste algoritmul inpachetarii cadurilor (marsul lui Jarvis)
            //the gift wrapping algorithm, also known as Jarvis march
            //returneaza doar pct infasuratoare
            List<Point> convexe = new List<Point>();

            Point varfStart = start;
            Point vEndpoint;

            do
            {
                convexe.Add(varfStart);
                vEndpoint = points[0];

                for (int i = 1; i < points.Count; i++)
                {
                    if ((varfStart == vEndpoint) || (Orientare(varfStart, vEndpoint, points[i]) == -1))
                    {
                        vEndpoint = points[i];
                    }
                }

                varfStart = vEndpoint;

            }
            while (vEndpoint != convexe[0]);

            return convexe;
        }

        private static int Orientare(Point p1, Point p2, Point p)
        {
            //de pe net
            //Ecuatia dreptei determinata de doua puncte
            //determinant
            int Orin = (p2.x - p1.x) * (p.y - p1.y) - (p.x - p1.x) * (p2.y - p1.y);
            if (Orin > 0)
                return -1; //          (* Orientation is to the left-hand side  *)
            if (Orin < 0)
                return 1; // (* Orientation is to the right-hand side *)

            return 0; //  (* Orientation is neutral aka collinear  *)
        }

        public List<Point> getFigurePoints()
        {
            return figurePoints;
        }

    }
}
