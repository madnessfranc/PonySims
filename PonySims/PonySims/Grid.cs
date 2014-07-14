using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PonySims
{
    class Grid
    {
        private Rectangle[,] rect;

        public Grid(int worldWidth, int worldHeight)
        {
            this.rect = new Rectangle[worldWidth/20,worldHeight/20];
            

            for (int x = 0; x < worldWidth; x += 20)
            {
                for (int y = 0; y < worldHeight; y += 20)
                {
                    this.rect[x/20, y/20] = new Rectangle(x, y, 20, 20);
                }
            }
        }

        public Rectangle[,] getRect()
        {
            return rect;
        }


    }
}
