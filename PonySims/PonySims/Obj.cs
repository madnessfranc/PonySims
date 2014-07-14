using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PonySims
{
    class Obj : Game1
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private bool walkable;
        private List<Vector2> neighbors = new List<Vector2>();
        private Vector2 neighborTop;
        private Vector2 neighborBottom;
        private Vector2 neighborLeft;
        private Vector2 neighborRight;
        private Vector2 neighborTopLeft;
        private Vector2 neighborTopRight;
        private Vector2 neighborBottomLeft;
        private Vector2 neighborBottomRight;
        private Rectangle container;
        private Texture2D texture;
        

        public Obj (int _x, int _y,int _width, int _height,bool _walkable, Texture2D _texture){
            this.x = _x;
            this.y = _y;
            this.width = _width;
            this.height = _height;

            this.container = new Rectangle(this.x, this.y, _width, _height);
            this.texture = _texture;
            setNeighbors();
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }
        public int X
        {
            get { return this.x; }
            set { this.x = value; this.container.X = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; this.container.Y = value; }
        }

        public Rectangle Container
        {
            get { return this.container; }
            set { this.container = value; }
        }

        public bool Walkable
        {
            get { return this.walkable; }
            set { this.walkable = value; }
        }

        private void setNeighbors(){
            this.neighborTop = new Vector2(this.x, this.y - this.height);
            this.neighborBottom = new Vector2(this.x, this.y + this.height);
            this.neighborLeft = new Vector2(this.x - this.width, this.y);
            this.neighborRight = new Vector2(this.x + this.width, this.y);
            this.neighborBottomLeft = new Vector2(this.x - this.width, this.y + this.height);
            this.neighborBottomRight = new Vector2(this.x + this.width, this.y + this.height);
            this.neighborTopLeft = new Vector2(this.x - this.width, this.y - this.height);
            this.neighborTopRight = new Vector2(this.x + this.width, this.y - this.height); 
            
            neighbors.Add(this.neighborTop);
            neighbors.Add(this.neighborBottom);
            neighbors.Add(this.neighborLeft);
            neighbors.Add(this.neighborRight);
            neighbors.Add(this.neighborBottomLeft);
            neighbors.Add(this.neighborBottomRight);
            neighbors.Add(this.neighborTopLeft);
            neighbors.Add(this.neighborTopRight);
        }

        public List<Vector2> getNeighbors()
        {
            return neighbors;
        }
    }
}
