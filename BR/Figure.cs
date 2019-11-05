using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR
{
    class Figure
    {
        public List <Point> points;
        public List<Line> lines;
        private Point XMax;
        private Point XMin;
        private Point YMax;
        private Point YMin;

        public Figure(List<Point> points, List<Line> lines)
        {
            this.points = points;
            this.lines = lines;
            XMax = GetXMax();
            XMin = GetXMin();
            YMax = GetYMax();
            YMin = GetYMin();
        }

        public bool CheckIfPointIsInsideFigure(Point pt)
        {
            //returneaza true daca un punct dat se afla in interiorul figurii
            if (pt.x > XMax.x || pt.x < XMin.x || pt.y > YMax.y || pt.y < YMin.y)
            {
                return false; //sigur e inafara figurii
            }
            else
            {
                //deseneaza o linie care sa treaca prin punct, paralela cu Ox (acelasi y)
                // (ar putea fi orice linie dar e mai usor asa)
                //calculeaza pct de intersectie ale dreptei respective cu toate celalalte linii
                //daca se gasesc puncte de intersectie (ar trebui sa fie 2 ptc e convex)
                // verifica unde sunt acestea in raport cu punctul tau
                //daca ambele sunt inainte sau dupa punct at punctul e inafara
                //dc unul e de o parte si celalat de cealalta at e inauntrul figurii


                int yDr = pt.y; //dreapta paralela
                List <Point> intersectionPoints = new List<Point>();
                long x; //necunoscuta, x-ul pct de intersecti

                foreach (Line l in lines)
                {
                    //ne intereseaza punctul in care dreapta curenta si dreapt paralela
                    //cu Ox se intersecteaza pe spatiul de lucru (daca s-ar intersecta
                    //inafara at nu ne intereseaza)
                    //limite spatiu de lucru:
                    //X min = 0
                    //X max = 650
                    //Y min = 0
                    //Y max = 1365

                    x = -(long)(l.b * yDr) / (long)l.a;
                    if (x > 0 && x < 650)
                    {
                        intersectionPoints.Add(new Point((int)x, yDr));
                    }

                }

                if (intersectionPoints.Count != 2)
                {
                    //ar trebui sa fie 2 pct
                    //throw new MissingMemberException(); //am aruncat o exceptie oarecare
                    //de vazut de ce se ajunge aici cu count = 0
                    return true;
                }
                else
                {
                    if (intersectionPoints[0].x <= pt.x &&
                       intersectionPoints[1].x >= pt.x)
                    {
                        return true; //se afla in figura
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private Point GetXMax()
        {
            int max = 0;
            Point found = null;
            foreach (Point p in points)
            {
                if (p.x > max)
                {
                    max = p.x;
                    found = p;
                }
            }
            return found;
        }

        private Point GetYMax()
        {
            int max = 0;
            Point found = null;
            foreach (Point p in points)
            {
                if (p.y > max)
                {
                    max = p.y;
                    found = p;
                }
            }
            return found;
        }

        private Point GetXMin()
        {
            int min = 100000;
            Point found = null;
            foreach (Point p in points)
            {
                if (p.x < min)
                {
                    min = p.x;
                    found = p;
                }
            }
            return found;
        }

        private Point GetYMin()
        {
            int min = 100000;
            Point found = null;
            foreach (Point p in points)
            {
                if (p.y < min)
                {
                    min = p.y;
                    found = p;
                }
            }
            return found;
        }
    }
}
