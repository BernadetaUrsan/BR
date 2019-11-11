//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BR
//{
//    class ConnectPoints
//    {
//        private List<Point> points;


//        public ConnectPoints(List<Point> points)
//        {
//            this.points = points;
//        }


//        public List<Line> GetFigureLines()
//        {
//            List<Line> lines = new List<Line>();
//            int minX = 0;
//            int minY = 0;
//            int maxX = 0;
//            int maxY = 0;
//            //Point start = new Point();
//            Point currentPoint = new Point();
//            Point pointUp = new Point();
//            Point pointDown = new Point();

//            //minX = getSmallestX();
//            minY = getSmallestY();
//            // maxX = getBiggestX();
//            maxY = getBiggestY();
//            //start = getStartingPoint();

//            //sorteaza lista de puncte crescator dupa x si crescator dupa y
//            points.Sort(
//                delegate(Point p1, Point p2)
//                {
//                    //return p1.x.CompareTo(p2.x);  //daca era p2(p1) era sortare descending
//                    return p1.x.CompareTo(p2.x);
//                });

//            for (int i = 0; i < points.Count(); i++)
//            {
//                currentPoint = points.ElementAt(i);
//                pointDown = new Point();
//                pointUp = new Point();
//                int deltaX;
//                int deltaY;
//                int indexUp = -1;
//                int indexDown = -1;

//                //pt fiecare pct pt care cauti pct cu care sa se conecteze
//                //initializeaza la inceput val max posibile cu o val la care
//                //nu se poate ajunge
//                int deltaXmin = 4000;
//                int deltaYmin = 4000;
//                int deltaYminUP = 4000;
//                int deltaYminDOWN = 4000;

//                //initializeaza punctele in care se vor stoca pct gasite cu -1
//                //ca sa stii daca ia gasit sau nu ptc in sus/jos
//                pointDown.x = -1;
//                pointDown.y = -1;
//                pointUp.x = -1;
//                pointUp.y = -1;

//                if (currentPoint.connectDown == false || currentPoint.connectUp == false)
//                {
//                    //doar pt pct care mai au cel putin un capat de conectat are rost sa
//                    //faci cautarea pct de conectare

//                    for (int j = i + 1; j <= points.Count() - 1; j++)
//                    {
//                        //gaseste cel mai apropiat punct de pe axa y in sus si in jos
//                        //care e si cel mai apropiat pe axa x
//                        deltaX = points.ElementAt(j).x - currentPoint.x;
//                        if (points.ElementAt(j).y < currentPoint.y)
//                        {
//                            //punctul care se compara cu pct curent este sub acesta
//                            deltaY = currentPoint.y - points.ElementAt(j).y;
//                            if ((deltaX <= deltaXmin) && (deltaY <= deltaYminUP) // dc e pct care se compara cu pct curent e cel mai apropiat de acesta
//                               && (points.ElementAt(j).connectUp == false))//si dc care se compara cu pct curent nu mai e deja conectat in sus
//                            {
//                                //pointDown = points.ElementAt(j);
//                                pointDown = new Point(points.ElementAt(j).x, points.ElementAt(j).y);
//                                indexDown = j;
//                                deltaXmin = deltaX;
//                                deltaYminUP = deltaY;
//                            }
//                        }
//                        else
//                        {
//                            //punctul care se compara cu pct curent este deasupra de acesta
//                            deltaY = points.ElementAt(j).y - currentPoint.y;
//                            if ((deltaX <= deltaXmin) && (deltaY <= deltaYminDOWN) // dc e pct care se compara cu pct curent e cel mai apropiat de acesta
//                               && (points.ElementAt(j).connectDown == false)) //si dc care se compara cu pct curent nu mai e deja conectat in jos
//                            {
//                                //pointUp = points.ElementAt(j);
//                                pointUp = new Point(points.ElementAt(j).x, points.ElementAt(j).y);
//                                indexUp = j;
//                                deltaXmin = deltaX;
//                                deltaYminDOWN = deltaY;
//                            }
//                        }

//                    }
//                }

//                if (currentPoint.connectDown == false)
//                {
//                    //dc pct curent trebuie conectat in jos
//                    if (pointDown.x != -1)
//                    {
//                        Line newLine = new Line(currentPoint, pointDown);
//                        int indexInLista;
//                        lines.Add(newLine);
//                        currentPoint.connectDown = true;
//                        //pointDown.connectUp = true;

//                        //marcheaza conectarea
//                        //fa modif direct in lista, altfel nu vor fi vazute la urm cautari


//                        points.ElementAt(indexDown).connectUp = true;

//                        //gresit
//                        //indexInLista = points.IndexOf(currentPoint);
//                        //points.ElementAt(indexInLista).connectDown = true;
//                        //indexInLista = points.IndexOf(pointDown);
//                        //points.ElementAt(indexInLista).connectUp = true;
//                    }
//                    else
//                    {
//                        //nu mai am cu ce sa-l conectez
//                        currentPoint.connectDown = true;
//                    }
//                }

//                if (currentPoint.connectUp == false)
//                {
//                    //dc pct curent trebuie conectat in sus
//                    if (pointUp.x != -1)
//                    {
//                        Line newLine = new Line(currentPoint, pointUp);
//                        //int indexInLista;
//                        lines.Add(newLine);

//                        //marcheaza conectarea
//                        //fa modif direct in lista, altfel nu vor fi vazute la urm cautari

//                        points.ElementAt(indexUp).connectDown = true;

//                        //de modificat a.i sa mearga!!!
//                        //indexInLista = points.IndexOf(currentPoint);
//                        //points.ElementAt(indexInLista).connectUp = true;
//                        //indexInLista = points.IndexOf(pointUp);
//                        //points.ElementAt(indexInLista).connectDown = true;
//                    }
//                    else
//                    {
//                        //nu mai am cu ce sa-l conectez
//                        currentPoint.connectUp = true;
//                    }
//                }
//            }

//            return lines;
//        }

//        private int getSmallestX()
//        {
//            int x = 4000;

//            foreach (Point p in points)
//            {
//                if (p.x < x)
//                {
//                    x = p.x;
//                }
//            }
//            return x;
//        }

//        private int getBiggestX()
//        {
//            int x = 0;

//            foreach (Point p in points)
//            {
//                if (p.x > x)
//                {
//                    x = p.x;
//                }
//            }
//            return x;
//        }

//        private int getSmallestY()
//        {
//            int y = 4000;

//            foreach (Point p in points)
//            {
//                if (p.y < y)
//                {
//                    y = p.y;
//                }
//            }
//            return y;
//        }

//        private int getBiggestY()
//        {
//            int y = 0;

//            foreach (Point p in points)
//            {
//                if (p.y > y)
//                {
//                    y = p.y;
//                }
//            }
//            return y;
//        }

//        private Point getStartingPoint()
//        {
//            //porneste de la pct cu cel mai mic x
//            //daca sunt mai multi ia-l pe cel cu cel mai mic y
//            List<Point> candidates = new List<Point>();
//            Point start = new Point();
//            int minX = getSmallestX();

//            foreach (Point p in points)
//            {
//                if (p.x == minX)
//                {
//                    candidates.Add(p);
//                }
//            }


//            int nrPct = candidates.Count();
//            if (nrPct == 0)
//            {
//                //nu ar trebui sa se ajunga aici
//                throw new InvalidOperationException();
//            }
//            else if (nrPct == 1)
//            {
//                return candidates.ElementAt(0);
//            }
//            else
//            {
//                //mai mult de un candidat cu acelasi x = xMin
//                start.y = 4000;
//                foreach (Point p in candidates)
//                {
//                    if (p.y <= start.y)
//                    {
//                        start = p;
//                    }
//                }
//                return start;
//            }



//        }

//    }
//}
