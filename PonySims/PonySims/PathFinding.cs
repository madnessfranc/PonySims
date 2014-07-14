using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PonySims
{
    class PathFinding : Game1
    {
        Point start;
        Point end;
        Point current;
        List<Point> saved = new List<Point>();
        List<Point> lookingAt = new List<Point>();
        private Obj[,] obj;
        private int worldWidth;
        private int worldHeight;

        public PathFinding(Point _start, Point _end, Obj[,] _obj,int width, int height)
        {
            this.start = _start;
            this.end = _end;
            this.current = new Point(this.start.X, this.start.Y);
            this.worldHeight = height;
            this.worldWidth = width;
            this.obj = _obj;
        }

        public void search()
        {
            while (current != end)
            {

                Point pointToSave = new Point(current.X, current.Y);
                if (current.X < end.X)
                {
                    if (obj[current.X + 1, current.Y] != null)
                    {
                        if (obj[current.X + 1, current.Y].Walkable)
                        {
                            pointToSave.X += 1;
                            current = (new Point(obj[current.X + 1, current.Y].Container.X, obj[current.X + 1, current.Y].Container.Y));
                        }
                    }
                    else
                    {
                        pointToSave.X += 1;
                        current = (new Point(current.X + 1, current.Y));
                    }
                }
                if (current.X > end.X)
                {
                    if (obj[(current.X - 1) / 20, current.Y / 20] != null)
                    {
                        if (obj[current.X - 1, current.Y].Walkable)
                        {
                            pointToSave.X -= 1;
                            current = (new Point(obj[current.X - 1, current.Y].Container.X, obj[current.X - 1, current.Y].Container.Y));
                        }
                    }
                    else
                    {
                        pointToSave.X -= 1;
                        current = (new Point(current.X - 1, current.Y));
                    }
                }
                if (current.Y < end.Y)
                {
                    if (obj[current.X, current.Y + 1] != null)
                    {
                        if (obj[current.X, current.Y + 1].Walkable)
                        {
                            pointToSave.Y += 1;
                            current = (new Point(obj[current.X, current.Y + 1].Container.X, obj[current.X, current.Y + 1].Container.Y));
                        }
                    }
                    else
                    {
                        pointToSave.Y += 1;
                        current = (new Point(current.X, current.Y + 1));
                    }
                }
                if (current.Y > end.Y)
                {
                    if (obj[current.X / 20, (current.Y - 1) / 20] != null)
                    {
                        if (obj[current.X, current.Y - 1].Walkable)
                        {
                            pointToSave.Y -= 1;
                            current = (new Point(obj[current.X, current.Y - 1].Container.X, obj[current.X, current.Y - 1].Container.Y));
                        }
                    }
                    else
                    {
                        pointToSave.Y -= 1;
                        current = (new Point(current.X, current.Y - 1));
                    }
                }
                saved.Add(pointToSave);
                
                

            }
        }

        public Point Current
        {
            get { return this.current; }
            set { this.current = value; }
        }

        public List<Point> Saved
        {
            get { return this.saved; }
            set { this.saved = value; }
        }


        //----------------------------------------------------//
        //----------------------------------------------------//
        //--------------------- A* ---------------------------//
        //----------------------------------------------------//
        //----------------------------------------------------//

        public SearchNode Parent;

        public bool InOpenList;
        public bool InClosedList;

        public float DistanceToGoal;
        public float DistanceTraveled;

        private List<Point> openList = new List<Point>();
        private List<Point> closedList = new List<Point>();

        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }

        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < worldHeight / 20; x++)
            {
                for (int y = 0; y < worldHeight / 20; y++)
                {
                    Obj node = Obj[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;

                }
            }
        }

        private SearchNode FindBestNode()
        {
            S
        }
            
    }
}
