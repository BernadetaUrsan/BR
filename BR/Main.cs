using System;
using System.Collections;
using BR.Collections;

namespace BR
{
	/// <summary>
	/// A node class for doing pathfinding on a 2-dimensional map
	/// </summary>
	public class AStarNode2D : AStarNode
	{
		#region Properties

		/// <summary>
		/// The X-coordinate of the node
		/// </summary>
		public int X 
		{
			get 
			{
				return FX;
			}
		}
		private int FX;

		/// <summary>
		/// The Y-coordinate of the node
		/// </summary>
		public int Y
		{
			get
			{
				return FY;
			}
		}
		private int FY;

		#endregion
	
		#region Constructors

		/// <summary>
		/// Constructor for a node in a 2-dimensional map
		/// </summary>
		/// <param name="AParent">Parent of the node</param>
		/// <param name="AGoalNode">Goal node</param>
		/// <param name="ACost">Accumulative cost</param>
		/// <param name="AX">X-coordinate</param>
		/// <param name="AY">Y-coordinate</param>
		public AStarNode2D(AStarNode AParent,AStarNode AGoalNode,double ACost,int AX, int AY) : base(AParent,AGoalNode,ACost)
		{
			FX = AX;
			FY = AY;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Adds a successor to a list if it is not impassible or the parent node
		/// </summary>
		/// <param name="ASuccessors">List of successors</param>
		/// <param name="AX">X-coordinate</param>
		/// <param name="AY">Y-coordinate</param>
		private void AddSuccessor(ArrayList ASuccessors,int AX,int AY) 
		{
			int CurrentCost = MainClass.GetMap(AX,AY);
			if(CurrentCost == -1) 
			{
				return;
			}
			AStarNode2D NewNode = new AStarNode2D(this,GoalNode,Cost + CurrentCost,AX,AY);
			if(NewNode.IsSameState(Parent)) 
			{
				return;
			}
			ASuccessors.Add(NewNode);
		}

		#endregion

		#region Overidden Methods

		/// <summary>
		/// Determines wheather the current node is the same state as the on passed.
		/// </summary>
		/// <param name="ANode">AStarNode to compare the current node to</param>
		/// <returns>Returns true if they are the same state</returns>
		public override bool IsSameState(AStarNode ANode)
		{
			if(ANode == null) 
			{
				return false;
			}
			return ((((AStarNode2D)ANode).X == FX) &&
				(((AStarNode2D)ANode).Y == FY));
		}
		
		/// <summary>
		/// Calculates the estimated cost for the remaining trip to the goal.
		/// </summary>
		public override void Calculate()
		{
			if(GoalNode != null) 
			{
				double xd = FX - ((AStarNode2D)GoalNode).X;
				double yd = FY - ((AStarNode2D)GoalNode).Y;
                // "Euclidean distance" - Used when search can move at any angle.
                //GoalEstimate = Math.Sqrt((xd*xd) + (yd*yd));
                // "Manhattan Distance" - Used when search can only move vertically and 
                // horizontally.
                //GoalEstimate = Math.Abs(xd) + Math.Abs(yd); 
                // "Diagonal Distance" - Used when the search can move in 8 directions.
                GoalEstimate = Math.Abs(xd) + Math.Abs(yd);
            }
			else
			{
				GoalEstimate = 0;
			}
		}

		/// <summary>
		/// Gets all successors nodes from the current node and adds them to the successor list
		/// </summary>
		/// <param name="ASuccessors">List in which the successors will be added</param>
		public override void GetSuccessors(ArrayList ASuccessors)
		{
			ASuccessors.Clear();
			AddSuccessor(ASuccessors,FX-1,FY  );
			//AddSuccessor(ASuccessors,FX-1,FY-1);
			AddSuccessor(ASuccessors,FX  ,FY-1);
			//AddSuccessor(ASuccessors,FX+1,FY-1);
			AddSuccessor(ASuccessors,FX+1,FY  );
			//AddSuccessor(ASuccessors,FX+1,FY+1);
			AddSuccessor(ASuccessors,FX  ,FY+1);
			//AddSuccessor(ASuccessors,FX-1,FY+1);
		}	

		/// <summary>
		/// Prints information about the current node
		/// </summary>
		public override void PrintNodeInfo()
		{
			Console.WriteLine("X:\t{0}\tY:\t{1}\tCost:\t{2}\tEst:\t{3}\tTotal:\t{4}",FX,FY,Cost,GoalEstimate,TotalCost);
		}

		#endregion
	}

	/// <summary>
	/// Test class for doing A* pathfinding on a 2D map.
	/// </summary>
	class MainClass
	{
        #region Test Maps
        static int nrOfCells = 0;
		static int[,] Map = {
			{ 1,-1, 1, 1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 2, 1, 1 },
			{ 1, 1, 1,-1, 1, 1, 2, 3, 2, 1 }
		};
//		static int[,] Map = {
//			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1,-1, 1 },
//			{ 1,-1,-1,-1,-1,-1,-1,-1,-1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
//		};
//		static int[,] Map = {
//			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
//			{ 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 },
//			{ 1, 1, 1, 1, 2, 3, 2, 1, 1, 1 },
//			{ 1, 1, 1, 2, 3, 4, 3, 2, 1, 1 },
//			{ 1, 1, 2, 3, 4, 5, 4, 3, 2, 1 },
//			{ 1, 1, 1, 2, 3, 4, 3, 2, 1, 1 },
//			{ 1, 1, 1, 1, 2, 3, 2, 1, 1, 1 },
//			{ 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
//			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
//		};

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets movement cost from the 2-dimensional map
		/// </summary>
		/// <param name="x">X-coordinate</param>
		/// <param name="y">Y-coordinate</param>
		/// <returns>Returns movement cost at the specified point in the map</returns>
		static public int GetMap(int x,int y)
		{
			if((x < 0) || (x >= nrOfCells))
				return(-1);
			if((y < 0) || (y >= nrOfCells))
				return(-1);
			return(Map[x,y]);
		}

		/// <summary>
		/// Prints the solution
		/// </summary>
		/// <param name="ASolution">The list that holds the solution</param>
		static public System.Collections.Generic.List<Point> PrintSolution(ArrayList ASolution)
		{
            Console.WriteLine("Starting printing solution");
            System.Collections.Generic.List<Point> list = new System.Collections.Generic.List<Point>();

            for (int i=0;i<nrOfCells;i++) 
			{
				for(int j=0;j<nrOfCells;j++) 
				{
					bool solution = false;
					foreach(AStarNode2D n in ASolution) 
					{
						AStarNode2D tmp = new AStarNode2D(null,null,0,i,j);
						solution = n.IsSameState(tmp);
						if(solution)
							break;
					}
					if(solution)
                    {
                        Console.Write("S ");
                        list.Add(new Point(i, j));
                    }
						
					else
						if(MainClass.GetMap(i,j) == -1)
						Console.Write("X ");
					else
						Console.Write(". ");
				}
				Console.WriteLine("");
			}
            return list;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static System.Collections.Generic.List<Point> GO(string[] args, int nr, int [,]matrix, Point start, Point end)
		{
            for (int i = 0; i < nr; i++)
            {
                for (int j = 0; j < nr; j++)
                {
                    Console.Write(matrix[i, j].ToString() + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Starting...");
            Map = matrix;
            nrOfCells = nr;
			BR.AStar astar = new BR.AStar();

			AStarNode2D GoalNode = new AStarNode2D(null,null,0,end.x,end.y);
			AStarNode2D StartNode = new AStarNode2D(null,GoalNode,0,start.x,start.y);
			StartNode.GoalNode = GoalNode;
			astar.FindPath(StartNode,GoalNode);

			return PrintSolution(astar.Solution);
			Console.ReadLine();
		}

		#endregion
	}
}
