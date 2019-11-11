using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BR
{

    public partial class Form1 : Form
    {

        Bitmap btmDesen; //pentru desenarea cu pixeli
        Graphics graphics;
        Pen pen;
        List<Point> points;
        List<Line> lines;
        List<Figure> figures;
        Point StartPoint;
        Point StopPoint;
        bool done; //previne desenarea de puncte dupa ce am dat pe add figure
        bool ClickDrawPoints;
        bool ClickStartPoint;
        bool ClickStopPoint;
        bool StartPointExists;
        bool StopPointExists;
        int StartStopPointDiam;
        int startPx;
        int startPy;
        int stopPx;
        int stopPy;
        int v = 0;
        int numOfCells = 0;
        float cellSizeHorizontal = 0f;
        float cellSizeVertical = 0f;
        int[,] matrix = new int[100, 100];
        //Pentru grid
        Dreptunghi[,] matrixDreptunghi;
        List<Dreptunghi> dreptunghis;
        //pentru metoda neomogena
        DreptunghiNeomogen mainDreptunghiNeomogen;
        int deepnessLevel;
        int newNumOfCells;

        public Form1()
        {
            InitializeComponent();

            lines = new List<Line>();
            points = new List<Point>();
            figures = new List<Figure>();                               
            pen = new Pen(Color.SkyBlue, 2);
            StartStopPointDiam = 20;
            done = false;
            ClickDrawPoints = true;
            ClickStartPoint = false;
            ClickStopPoint = false;
            StartPoint = null;
            StopPoint = null;
            StartPointExists = false;
            StopPointExists = false;
        }

        private void startPoint_Click(object sender, EventArgs e)
        {
            if (StartPointExists == false)
            {
                ClickDrawPoints = false;
                ClickStartPoint = true;
                ClickStopPoint = false;
            }
            else
            {
                DialogResult d = MessageBox.Show("A StartPoint already exists. Do you wish to delete the old one and add a new startPoint?", "You cannot have more startPoints", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    ClickDrawPoints = false;
                    ClickStartPoint = true;
                    ClickStopPoint = false;
                    DeleteStartStopPoint(StartPoint);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btmDesen = new Bitmap(pctDesen.Width, pctDesen.Height);
            graphics = Graphics.FromImage(btmDesen);

            //secventa obligatorie la orice modificare a imaginii ca sa se vada in form
            //(grafica persistenta)
            graphics.Clear(Color.White);  //modificare
            pctDesen.Image = btmDesen; //vezi modificarea in form (reload)   
        }

        void colorDreptunghi(Dreptunghi dreptunghi, Color color) //coloreaza cu culoarea selectara = negru suprafata delimitata de parametrii dati
        {
            graphics.FillRectangle(new SolidBrush(color), dreptunghi.SusStangaX, dreptunghi.SusStangaY, dreptunghi.width, dreptunghi.height);
            pctDesen.Image = btmDesen; //vezi modificarea in form (reload)
        }

        private void drawStartPoint() //seteaza punctul de start printr-un cerc folosind brush
        {
            pen.Color = Color.Green;
            graphics.FillEllipse(pen.Brush, StartPoint.x - StartStopPointDiam / 2, StartPoint.y - StartStopPointDiam / 2, StartStopPointDiam, StartStopPointDiam);
            //cu w = 20, h =20, e da poz mouse-ului
            pctDesen.Image = btmDesen;
        }

        private void drawStopPoint() //seteaza punctul de stop printr-un cerc folosind brush
        {
            pen.Color = Color.Crimson;
            graphics.FillEllipse(pen.Brush, StopPoint.x - StartStopPointDiam / 2, StopPoint.y - StartStopPointDiam / 2, StartStopPointDiam, StartStopPointDiam);
            //cu w = 20, h =20, e da poz mouse-ului
            pctDesen.Image = btmDesen;
        }
        
        private void pctDesenare_MouseDown(object sender, MouseEventArgs e)
        {
            if (ClickDrawPoints == true)
            {
                //draw points
                if (done == false)
                {
                    pen.Color = Color.SkyBlue;
                    graphics.DrawEllipse(pen, e.X, e.Y, 2, 2); //pt pct reprezentam o elipsa
                    //cu w = 2, h =2, e da poz mouse-ului
                    pctDesen.Image = btmDesen;
                    points.Add(new Point(e.X, e.Y));
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            else if (ClickStartPoint == true)
            {
              
                Point startPointCandidate = new Point(e.X, e.Y);
                bool retry = true;
                
                retry = false;
                if (retry == true)
                {

                    DialogResult d = MessageBox.Show("Point overlaps the figure. Try again?", "Your choice is not ok", MessageBoxButtons.YesNo);
                    if (d == DialogResult.No)
                    {
                        //get back to waiting for a usercommand
                        ClickDrawPoints = false;
                        ClickStartPoint = false;
                        ClickStopPoint = false;
                    }
                }
                else
                {
                    //draw startpoint
                    pen.Color = Color.ForestGreen;
                    graphics.FillEllipse(pen.Brush, e.X - StartStopPointDiam / 2, e.Y - StartStopPointDiam / 2, StartStopPointDiam, StartStopPointDiam);
                    //cu w = 20, h =20, e da poz mouse-ului
                    pctDesen.Image = btmDesen;

                    //get back to waiting for a usercommand
                    ClickDrawPoints = false;
                    ClickStartPoint = false;
                    ClickStopPoint = false;
                    StartPointExists = true;
                    //save point only when you are sure it is ok
                    StartPoint = new Point(startPointCandidate.x, startPointCandidate.y);
                    startPx = startPointCandidate.x;
                    startPy = startPointCandidate.y;
                }
            }
            else if (ClickStopPoint == true)
            {
                bool retry = true;
                Point stopPointCandidate = new Point(e.X, e.Y);
                retry = false;

                if (retry == true)
                {
                    DialogResult d = MessageBox.Show("Your point overlaps the figure. Try again?", "Your choice is not correct", MessageBoxButtons.YesNo);
                    if (d == DialogResult.No)
                    {
                        //get back to waiting for a usercommand
                        ClickDrawPoints = false;
                        ClickStartPoint = false;
                        ClickStopPoint = false;
                    }
                }
                else
                {
                    //draw stoppoint
                    pen.Color = Color.Crimson;
                    graphics.FillEllipse(pen.Brush, e.X - StartStopPointDiam / 2, e.Y - StartStopPointDiam / 2, StartStopPointDiam, StartStopPointDiam);
                    //cu w = 20, h =20, e da poz mouse-ului
                    pctDesen.Image = btmDesen;

                    //get back to waiting for a user command
                    ClickDrawPoints = false;
                    ClickStartPoint = false;
                    ClickStopPoint = false;
                    StopPointExists = true;
                    StopPoint = new Point(stopPointCandidate.x, stopPointCandidate.y);
                    //save point only whenyou are sure it is ok
                    stopPx = stopPointCandidate.x;
                    stopPy = stopPointCandidate.y;
                }

            }
            else if (ClickDrawPoints == false && ClickStartPoint == false && ClickStopPoint == false)
            {
                MessageBox.Show("Please select what you wish to do first: Add new figure\n\t\t\t\t\t\t\t\t\t\t            Add Start point\n\t\t\t\t\t\t\t\t\t\t            Add Stop point!\nClick on the corresponding button!",
                                 "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void btnNew_Click(object sender, EventArgs e) // se vrea creerea unui nou obiect
        {// asa ca se pune done/add pe 0 si 
            done = false;
            ClickDrawPoints = true;
            ClickStartPoint = false;
            ClickStopPoint = false;
        }

        private void btnOK_Click(object sender, EventArgs e) // se vrea adaugarea/salvarea obiectului pe harta
        {
            ConnectPoints connector = new ConnectPoints(points); // se incearca conectarea punctelor pentru a forma obiectul

            if (points.Count < 3)
            {
                MessageBox.Show("First add  at least 3 points by clicking on the screen at the location where you wish to place them",
                                "Waning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                done = true; // s-a finalizat construirea obiectului
                lines = connector.GetFigureLines(); // de cautat 

                foreach (Line l in lines)
                {
                    graphics.DrawLine(pen, l.p1.x, l.p1.y, l.p2.x, l.p2.y);
                }
                pctDesen.Image = btmDesen; //vezi modificarea in form (reload)

                //salveaza figura si pregateste spatiul pt urm figura
                List<Point> varfuri = connector.getFigurePoints();
                if (varfuri == null)
                {
                    //should.t get here
                    MessageBox.Show("Tried to get the figure points before defining the figure!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Figure fig = new Figure(varfuri, lines);
                    figures.Add(fig);
                }

                FillFigure(varfuri); //fill figure before clearing the points

                points.Clear();
                //lines.Clear();


            }

        }

        private void btnClear_Click(object sender, EventArgs e) // se apasa butonul de delete map
        {

            graphics.Clear(Color.White);  //modificare
            pctDesen.Image = btmDesen; //vezi modificarea in form (reload)
            points.Clear();
            lines.Clear();
            figures.Clear();
            StartPointExists = false;
            StopPointExists = false;
            done = false;
            // se sterge tot, nu mai exista puncte de start/stop sau figuri si se face ecranul alb
        }

        private void stopPoint_Click(object sender, EventArgs e)
        {
            if (StopPointExists == false)
            {
                ClickDrawPoints = false;
                ClickStartPoint = false;
                ClickStopPoint = true;
            }
            else
            {
                DialogResult d = MessageBox.Show("A stopPoint already exists. Do you wish to delete the old one and add a new stopPoint?", "You cannot have more stopPoints", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    ClickDrawPoints = false;
                    ClickStartPoint = false;
                    ClickStopPoint = true;
                    DeleteStartStopPoint(StopPoint);
                }
            }
        }

        private void FillFigure(List<Point> points)
        {
            System.Drawing.Point[] pct = new System.Drawing.Point[points.Count()];
            int i = 0;
            SolidBrush b = new SolidBrush(Color.SkyBlue);
            foreach (Point p in points)
            {
                pct[i] = new System.Drawing.Point(p.x, p.y);
                i++;
            }
            graphics.FillPolygon((Brush)b, pct); // metoda
            pctDesen.Image = btmDesen; //vezi modificarea in form (reload)
        }

        private void DeleteStartStopPoint(Point p)
        {
            pen.Color = Color.White;
            graphics.FillEllipse(pen.Brush, p.x - StartStopPointDiam / 2, p.y - StartStopPointDiam / 2, StartStopPointDiam, StartStopPointDiam);
            //cu w = 20, h =20, e da poz mouse-ului
            pctDesen.Image = btmDesen;
        }

        //desenam gridul
        private void drawLines()
        {
            Pen p = new Pen(Color.Gray);
            for (int i = 0; i < numOfCells; i++)
            {
                //orizontal
                graphics.DrawLine(p, 0, (int)(i * cellSizeVertical), (int)(numOfCells * cellSizeHorizontal), (int)(i * cellSizeVertical));
                //vertical
                graphics.DrawLine(p, (int)(i * cellSizeHorizontal), 0, (int)(i * cellSizeHorizontal), (int)(numOfCells * cellSizeVertical));
            }
        }

        private bool doesLineEnterDreptunghi(Line line, Dreptunghi dreptunghi)
        {
            List<Line> dreptunghiLines = new List<Line>();
            dreptunghiLines.Add(new Line(new Point(dreptunghi.SusStangaX, dreptunghi.SusStangaY), new Point(dreptunghi.SusStangaX + dreptunghi.width, dreptunghi.SusStangaY)));
            dreptunghiLines.Add(new Line(new Point(dreptunghi.SusStangaX, dreptunghi.SusStangaY), new Point(dreptunghi.SusStangaX, dreptunghi.SusStangaY + dreptunghi.height)));
            dreptunghiLines.Add(new Line(new Point(dreptunghi.SusStangaX, dreptunghi.SusStangaY + dreptunghi.height), new Point(dreptunghi.SusStangaX + dreptunghi.width, dreptunghi.SusStangaY + dreptunghi.height)));
            dreptunghiLines.Add(new Line(new Point(dreptunghi.SusStangaX + dreptunghi.width, dreptunghi.SusStangaY), new Point(dreptunghi.SusStangaX + dreptunghi.width, dreptunghi.SusStangaY + dreptunghi.height)));
            bool entered = false;

            if (line.p1.x >= dreptunghi.SusStangaX && line.p1.x <= (dreptunghi.SusStangaX + dreptunghi.width) && line.p2.x >= dreptunghi.SusStangaX && line.p2.x <= (dreptunghi.SusStangaX + dreptunghi.width))
            {
                if (line.p1.y >= dreptunghi.SusStangaY && line.p1.y <= (dreptunghi.SusStangaY + dreptunghi.height) && line.p2.y >= dreptunghi.SusStangaY && line.p2.y <= (dreptunghi.SusStangaY + dreptunghi.height))
                {
                    entered = true;
                }
            }

            dreptunghiLines.ForEach(currentLine =>
            {
                double delta = currentLine.a * line.b - line.a * currentLine.b;

                if (delta == 0)
                { }

                else
                {
                    double x = (line.b * currentLine.c - currentLine.b * line.c) / delta;

                    double y = (currentLine.a * line.c - line.a * currentLine.c) / delta;
                    int xMin, xMax;
                    int yMin, yMax;
                    if (line.p1.x < line.p2.x)
                    {
                        xMin = line.p1.x;
                        xMax = line.p2.x;
                    }
                    else
                    {
                        xMin = line.p2.x;
                        xMax = line.p1.x;
                    }
                    if (line.p1.y < line.p2.y)
                    {
                        yMin = line.p1.y;
                        yMax = line.p2.y;
                    }
                    else
                    {
                        yMin = line.p2.y;
                        yMax = line.p1.y;
                    }
                    if (x >= dreptunghi.SusStangaX && x <= (dreptunghi.SusStangaX + dreptunghi.width) && x >= xMin && x <= xMax)
                    {
                        if (y >= dreptunghi.SusStangaY && y <= (dreptunghi.SusStangaY + dreptunghi.height) && y >= yMin && y <= yMax)
                        {
                            entered = true;
                        }
                    }
                }
            });

            return entered;
        }

        private void LogMatrice()
        {
            for (int i = 0; i < numOfCells; i++)
            {
                for (int j = 0; j < numOfCells; j++)
                {
                    Console.Write(matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        //metoda omogena
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (v != 0) // in cazul in care a mai fost facut odata descompunerea, stergem grid-ul 
            {
                graphics.Clear(Color.White);
                pctDesen.Image = btmDesen;
            }

            if (patrat.Text != "")
            {
                numOfCells = Int32.Parse(patrat.Text);
                matrix = new int[numOfCells, numOfCells];
                matrixDreptunghi = new Dreptunghi[numOfCells, numOfCells];
                dreptunghis = new List<Dreptunghi>();
                cellSizeHorizontal = (float)pctDesen.Width / numOfCells;
                cellSizeVertical = (float)pctDesen.Height / numOfCells;
                Pen p = new Pen(Color.Gray);

                for (int i = 0; i < numOfCells; i++)
                {
                    //orizontal
                    for (int j = 0; j < numOfCells; j++)
                    {
                        Dreptunghi dreptunghi = new Dreptunghi();
                        dreptunghi.SusStanga = new System.Drawing.Point((int)(j * cellSizeHorizontal), (int)(i * cellSizeVertical));
                        dreptunghi.SusDreapta = new System.Drawing.Point((int)(j * 2 * cellSizeHorizontal), (int)(i * cellSizeVertical));
                        dreptunghi.JosStanga = new System.Drawing.Point((int)(j * cellSizeHorizontal), (int)(i * 2 * cellSizeVertical));
                        dreptunghi.JosDreapta = new System.Drawing.Point((int)(j * 2 * cellSizeHorizontal), (int)(i * 2 * cellSizeVertical));
                        dreptunghi.SusStangaX = (int)(j * cellSizeHorizontal);
                        dreptunghi.SusStangaY = (int)(i * cellSizeVertical);
                        dreptunghi.width = (int)cellSizeHorizontal;
                        dreptunghi.height = (int)cellSizeVertical;
                        dreptunghi.indexIMatrice = i;
                        dreptunghi.indexJMatrice = j;
                        dreptunghis.Add(dreptunghi);
                        matrixDreptunghi[i, j] = dreptunghi;
                        matrix[i, j] = 1;
                    }
                }
                //MATRIX INITIAL
                Console.WriteLine("MATRIX INITIAL");
                LogMatrice();

                pctDesen.Image = btmDesen;
                v++;

                figures.ForEach(figure =>
                {
                    figure.lines.ForEach(line =>
                    {
                        dreptunghis.ForEach(
                            dreptunghi =>
                            {
                                if (doesLineEnterDreptunghi(line, dreptunghi))
                                {
                                    graphics.FillRectangle(new SolidBrush(Color.CadetBlue), dreptunghi.SusStangaX, dreptunghi.SusStangaY, dreptunghi.width, dreptunghi.height);
                                    pctDesen.Image = btmDesen; //vezi modificarea in form (reload)
                                    matrix[dreptunghi.indexIMatrice, dreptunghi.indexJMatrice] = -1;
                                }
                            });
                    });
                });

                for (int i = 0; i < numOfCells; i++)
                {
                    //orizontal
                    graphics.DrawLine(p, 0, (int)(i * cellSizeVertical), (int)(numOfCells * cellSizeHorizontal), (int)(i * cellSizeVertical));
                    //vertical
                    graphics.DrawLine(p, (int)(i * cellSizeHorizontal), 0, (int)(i * cellSizeHorizontal), (int)(numOfCells * cellSizeVertical));
                }

                figures.ForEach(figure =>
                {
                    FillFigure(figure.points);
                });

                //MATRIX DUPA METODA
                Console.WriteLine("MATRIX DUPA OMOGEN");
                LogMatrice();
            }

            else
            {
                MessageBox.Show("Please insert a valid number of square.");
            }

        }

        //neomogen
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (v != 0)         // in cazul in care a mai fost facut odata descompunerea, stergem grid-ul 
            {
                graphics.Clear(Color.White);
                pctDesen.Image = btmDesen;
            }

            if (patrat.Text != "")
            {
                deepnessLevel = Int32.Parse(deepnessLevelText.Text);
                numOfCells = Int32.Parse(patrat.Text);
                mainDreptunghiNeomogen = new DreptunghiNeomogen(null, 0, 0, pctDesen.Width, pctDesen.Height);

                mainDreptunghiNeomogen.buildChildDreptunghis(numOfCells);

                figures.ForEach(figure =>
                {
                    figure.lines.ForEach(line =>
                    {
                        completeDreptunghiNeomogenMatricesRecursively(line, mainDreptunghiNeomogen, Int32.Parse(deepnessLevelText.Text));
                    });
                });

                LogMatrice();

                fillRectanglesRecursively(mainDreptunghiNeomogen);

                drawLinesForDreptunghiAndChildrenRecursively(mainDreptunghiNeomogen);
                figures.ForEach(figure =>
                {
                    FillFigure(figure.points);
                });
                v = 1;
            }

            else
            {
                MessageBox.Show("Please insert a valid number of square.");
            }

        }

        private void completeDreptunghiNeomogenMatricesRecursively(Line line, DreptunghiNeomogen dreptunghiNeomogen, int deepnessLimit)
        {
            dreptunghiNeomogen.childDreptunghisList.ForEach(childDreptunghi =>
            {
                if (doesLineEnterDreptunghi(line, childDreptunghi))
                {
                    dreptunghiNeomogen.childDreptunghisPresenceMatrix[childDreptunghi.indexIMatrice, childDreptunghi.indexJMatrice] = -1; //Line crosses there
                    if (deepnessLimit > dreptunghiNeomogen.deepnessLevel)
                    {
                        childDreptunghi.buildChildDreptunghis(dreptunghiNeomogen.nrOfCells);
                        completeDreptunghiNeomogenMatricesRecursively(line, childDreptunghi, deepnessLimit);
                    }

                }
            });
        }

        private void fillRectanglesRecursively(DreptunghiNeomogen dreptunghiNeomogen)
        {
            for (int i = 0; i < numOfCells; i++)
                for (int j = 0; j < numOfCells; j++)
                {
                    if (dreptunghiNeomogen.childDreptunghisPresenceMatrix[i, j] == -1)
                    {
                        if (dreptunghiNeomogen.childDreptunghis[i, j].childDreptunghisList == null)
                        {
                            graphics.FillRectangle(new SolidBrush(Color.Turquoise), dreptunghiNeomogen.childDreptunghis[i, j].SusStangaX, dreptunghiNeomogen.childDreptunghis[i, j].SusStangaY, dreptunghiNeomogen.childDreptunghis[i, j].width, dreptunghiNeomogen.childDreptunghis[i, j].height);
                            pctDesen.Image = btmDesen; //vezi modificarea in form (reload)
                        }
                        else
                        {
                            fillRectanglesRecursively(dreptunghiNeomogen.childDreptunghis[i, j]);
                        }
                    }
                }
        }

        private void drawLinesForDreptunghiAndChildrenRecursively(DreptunghiNeomogen dreptunghiNeomogen)
        {
            Pen p = new Pen(Color.Red);
            graphics.DrawLine(p, dreptunghiNeomogen.SusStangaX, dreptunghiNeomogen.SusStangaY, dreptunghiNeomogen.SusStangaX + dreptunghiNeomogen.width, dreptunghiNeomogen.SusStangaY);
            pctDesen.Image = btmDesen;
            graphics.DrawLine(p, dreptunghiNeomogen.SusStangaX, dreptunghiNeomogen.SusStangaY, dreptunghiNeomogen.SusStangaX, dreptunghiNeomogen.SusStangaY + dreptunghiNeomogen.height);
            pctDesen.Image = btmDesen;
            graphics.DrawLine(p, dreptunghiNeomogen.SusStangaX, dreptunghiNeomogen.SusStangaY + dreptunghiNeomogen.height, dreptunghiNeomogen.SusStangaX + dreptunghiNeomogen.width, dreptunghiNeomogen.SusStangaY + dreptunghiNeomogen.height);
            pctDesen.Image = btmDesen;
            graphics.DrawLine(p, dreptunghiNeomogen.SusStangaX + dreptunghiNeomogen.width, dreptunghiNeomogen.SusStangaY, dreptunghiNeomogen.SusStangaX + dreptunghiNeomogen.width, dreptunghiNeomogen.SusStangaY + dreptunghiNeomogen.height);
            pctDesen.Image = btmDesen;
            if (dreptunghiNeomogen.childDreptunghisList != null)
            {
                dreptunghiNeomogen.childDreptunghisList.ForEach(childDreptunghi =>
                {
                    drawLinesForDreptunghiAndChildrenRecursively(childDreptunghi);
                });
            }

        }

        //start omogen
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
                Dreptunghi startDreptunghi = whichDreptunghiSeAfla(StartPoint);
                Dreptunghi endDreptunghi = whichDreptunghiSeAfla(StopPoint);
                List<Point> list = BR.MainClass.GO(null, numOfCells, matrix, new Point(startDreptunghi.indexIMatrice, startDreptunghi.indexJMatrice), new Point(endDreptunghi.indexIMatrice, endDreptunghi.indexJMatrice));

                foreach (Point point in list)
                {
                    colorDreptunghi(matrixDreptunghi[point.x, point.y], Color.Coral);

                }
                drawLines();
                figures.ForEach(figure =>
                {
                    FillFigure(figure.points);
                }
                    );
                drawStartPoint();
                drawStopPoint();
            
        }

        Dreptunghi whichDreptunghiSeAfla(Point point)
        {
            foreach (Dreptunghi dreptunghi in dreptunghis)
                if (dreptunghi.SusStangaX <= point.x)
                    if ((dreptunghi.SusStangaX + dreptunghi.width) >= point.x)
                        if (dreptunghi.SusStangaY <= point.y)
                            if ((dreptunghi.SusStangaY + dreptunghi.height) >= point.y)
                                return dreptunghi;
            return null;
        }

        //start neomogen
        private void descOmogena_Click(object sender, EventArgs e)
        {
            desc_OmogenaTest();

            Dreptunghi startDreptunghi = whichDreptunghiSeAfla(StartPoint);
            Dreptunghi endDreptunghi = whichDreptunghiSeAfla(StopPoint);

            List<Point> list = BR.MainClass.GO(null, newNumOfCells, matrix, new Point(startDreptunghi.indexIMatrice, startDreptunghi.indexJMatrice), new Point(endDreptunghi.indexIMatrice, endDreptunghi.indexJMatrice));

            foreach (Point point in list)
            {
                Dreptunghi dreptungiTemp = matrixDreptunghi[point.x, point.y];
                Point mijlocCoordinate = new Point((dreptungiTemp.SusStangaX + 10), (dreptungiTemp.SusStangaY + 10));
                DreptunghiNeomogen drne = whichDreptunghiSeAflaNeomogenRecursiv(mijlocCoordinate, mainDreptunghiNeomogen);
                colorDreptunghi(drne, Color.Gold);
            }
            drawLinesForDreptunghiAndChildrenRecursively(mainDreptunghiNeomogen);

            figures.ForEach(figure =>
            {
                FillFigure(figure.points);
            }
                );
            drawStartPoint();
            drawStopPoint();
        }

        private void desc_OmogenaTest()
        {
            int numOfCells = (int)Math.Pow(this.numOfCells, this.deepnessLevel + 1);
            this.newNumOfCells = numOfCells;
            matrix = new int[numOfCells, numOfCells];
            matrixDreptunghi = new Dreptunghi[numOfCells, numOfCells];
            dreptunghis = new List<Dreptunghi>();
            cellSizeHorizontal = (float)pctDesen.Width / numOfCells;
            cellSizeVertical = (float)pctDesen.Height / numOfCells;

            for (int i = 0; i < numOfCells; i++)
            {
                for (int j = 0; j < numOfCells; j++)
                {
                    Dreptunghi dreptunghi = new Dreptunghi();
                    dreptunghi.SusStanga = new System.Drawing.Point((int)(j * cellSizeHorizontal), (int)(i * cellSizeVertical));
                    dreptunghi.SusDreapta = new System.Drawing.Point((int)(j * 2 * cellSizeHorizontal), (int)(i * cellSizeVertical));
                    dreptunghi.JosStanga = new System.Drawing.Point((int)(j * cellSizeHorizontal), (int)(i * 2 * cellSizeVertical));
                    dreptunghi.JosDreapta = new System.Drawing.Point((int)(j * 2 * cellSizeHorizontal), (int)(i * 2 * cellSizeVertical));
                    dreptunghi.SusStangaX = (int)(j * cellSizeHorizontal);
                    dreptunghi.SusStangaY = (int)(i * cellSizeVertical);
                    dreptunghi.width = (int)cellSizeHorizontal;
                    dreptunghi.height = (int)cellSizeVertical;
                    dreptunghi.indexIMatrice = i;
                    dreptunghi.indexJMatrice = j;
                    dreptunghis.Add(dreptunghi);
                    matrixDreptunghi[i, j] = dreptunghi;
                    matrix[i, j] = 1;
                }
            }

            LogMatrice();
            v++;

            figures.ForEach(figure =>
            {
                figure.lines.ForEach(line =>
                {
                    dreptunghis.ForEach(
                        dreptunghi =>
                        {
                            if (doesLineEnterDreptunghi(line, dreptunghi))
                            {
                                matrix[dreptunghi.indexIMatrice, dreptunghi.indexJMatrice] = -1;
                            }
                        });
                });
            });

            LogMatrice();
        }

        DreptunghiNeomogen whichDreptunghiSeAflaNeomogenRecursiv(Point point, DreptunghiNeomogen dreptunghiNeomogen)
        {
            DreptunghiNeomogen temp = null;
            if (dreptunghiNeomogen.childDreptunghisList != null)
                foreach (DreptunghiNeomogen dreptunghi in dreptunghiNeomogen.childDreptunghisList)
                {
                    temp = whichDreptunghiSeAflaNeomogenRecursiv(point, dreptunghi);
                    if (temp != null)
                        break;
                }

            if (temp == null)
            {
                if (dreptunghiNeomogen.SusStangaX <= point.x)
                    if ((dreptunghiNeomogen.SusStangaX + dreptunghiNeomogen.width) >= point.x)
                        if (dreptunghiNeomogen.SusStangaY <= point.y)
                            if ((dreptunghiNeomogen.SusStangaY + dreptunghiNeomogen.height) >= point.y)
                                temp = dreptunghiNeomogen;
            }

            return temp;
        }

    }
}