using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BR
{
    public class DreptunghiNeomogen : Dreptunghi
    {
        public int[,] childDreptunghisPresenceMatrix;
        public DreptunghiNeomogen[,] childDreptunghis = null;
        public List<DreptunghiNeomogen> childDreptunghisList = null;
        public int nrOfCells;
        public int deepnessLevel = 0;
        public DreptunghiNeomogen parent = null;

        public DreptunghiNeomogen()
        {

        }

        public DreptunghiNeomogen(DreptunghiNeomogen _parent, int susStangaX, int susStangaY, int _width, int _height)
        {
            this.parent = _parent;
            this.SusStangaX = susStangaX;
            this.SusStangaY = susStangaY;
            this.width = _width;
            this.height = _height;

        }

        public Boolean isItAContainer()
        {
            return childDreptunghis != null;
        }


        public void buildChildDreptunghis(int _nrOfCells)
        {
            if (childDreptunghis == null)
            {
                nrOfCells = _nrOfCells;

                childDreptunghisPresenceMatrix = new int[nrOfCells, nrOfCells];
                for (int i = 0; i < nrOfCells; i++)
                    for (int j = 0; j < nrOfCells; j++)
                        childDreptunghisPresenceMatrix[i, j] = 1;

                childDreptunghis = new DreptunghiNeomogen[nrOfCells, nrOfCells];
                childDreptunghisList = new List<DreptunghiNeomogen>();

                float cellSizeHorizontal = (float)width / nrOfCells;
                float cellSizeVertical = (float)height / nrOfCells;
                for (int i = 0; i < nrOfCells; i++)
                    for (int j = 0; j < nrOfCells; j++)
                    {
                        DreptunghiNeomogen dreptunghiNeomogenTemp = new DreptunghiNeomogen(this, (int)(SusStangaX + j * cellSizeHorizontal), (int)(SusStangaY + i * cellSizeVertical), (int)(cellSizeHorizontal), (int)(cellSizeVertical));
                        dreptunghiNeomogenTemp.deepnessLevel = deepnessLevel + 1;
                        dreptunghiNeomogenTemp.indexIMatrice = i;
                        dreptunghiNeomogenTemp.indexJMatrice = j;
                        childDreptunghis[i, j] = dreptunghiNeomogenTemp;
                        childDreptunghisList.Add(dreptunghiNeomogenTemp);
                    }
            }

        }
    }
}
