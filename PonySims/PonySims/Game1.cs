using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PonySims
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        SpriteFont Font1;

        int num = 0;
        int gameWidth;
        int gameHeight;
        int worldWidth;
        int worldHeight;
        int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int frameRate = 0;
        int frameCounter = 0;

        TimeSpan elapsedTime = TimeSpan.Zero;

        Rectangle TestUI;
        Rectangle[,] rect;
        Rectangle mouseSquare;

        Matrix Trans;

        Obj[,] objects;
        PathFinding path;

        Dictionary<Rectangle,Obj> notWalkable;
        
        Texture2D grass;
        Texture2D pony;
        Texture2D texture;

        protected Vector2 _pos;
        protected Viewport _viewport;
        protected MouseState _mState;
        protected KeyboardState _keyState;

        protected int lastX = 0;
        protected int lastY = 0;

        Color[] colorData;

        Viewport Ui;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.IsMouseVisible = true;
            Font1 = Content.Load<SpriteFont>("MouseCoor");
            Trans = Matrix.CreateTranslation(_pos.X, _pos.Y, 0);
            gameWidth = GraphicsDevice.Viewport.Width;
            gameHeight = GraphicsDevice.Viewport.Height;
            worldWidth = 10000;
            worldHeight = 7200;
            Ui = new Viewport(0, 0, gameWidth, gameHeight);
            notWalkable = new Dictionary<Rectangle,Obj>();


            Grid grid = new Grid(worldWidth, worldHeight);
            
            
        #region "load texture"
            texture = new Texture2D(this.GraphicsDevice, 1, 1);
            colorData = new Color[1];
            colorData[0] = Color.Gray;
            texture.SetData<Color>(colorData);
            grass = Content.Load<Texture2D>("grass");
            pony = Content.Load<Texture2D>("testst");
        #endregion

            rect = grid.getRect();
            objects = new Obj[worldWidth / 20,worldHeight / 20];
            objects[4,4] = new Obj(50, 50, 20, 20, false, pony);
            
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            Input();
            Trans = Matrix.CreateTranslation(_pos.X, _pos.Y, 0);
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            //if (path != null && path.Saved.Count != 0)
            //{
            //        if(objects[4, 4].Container.X < path.Saved[num].X * 20)
            //        objects[4, 4].X += 1 ;
            //        if(objects[4, 4].Container.Y < path.Saved[num].Y * 20)
            //        objects[4, 4].Y += 1 ;
            //        if (objects[4, 4].Container.X > path.Saved[num].X * 20)
            //            objects[4, 4].X -= 1;
            //        if (objects[4, 4].Container.Y > path.Saved[num].Y * 20)
            //            objects[4, 4].Y -= 1;

            //        if (objects[4, 4].Container.X == (path.Saved[num].X * 20) && objects[4, 4].Container.Y == (path.Saved[num].Y * 20) && num < path.Saved.Count - 1)
            //            num++;
            //}
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _mState = Mouse.GetState();
            Mouse.WindowHandle = this.Window.Handle;

            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,null,null,null,null,Trans);
                DrawGrid();
                foreach(Obj oo in objects){
                    if (oo != null)
                    {
                        spriteBatch.Draw(oo.Texture, oo.Container, Color.White);
                    } 
                }

                if (path != null)
                {
                    foreach (var al in path.Saved)
                    {
                        Rectangle circle = new Rectangle(al.X * 20, al.Y * 20, 20, 20);
                        spriteBatch.Draw(texture, circle, Color.Red);
                    }
                }
            spriteBatch.End();

            frameCounter++;
            _mState = Mouse.GetState();
            Mouse.WindowHandle = this.Window.Handle;

            string fps = string.Format("{0}", frameRate);

            spriteBatch.Begin();
                DrawUI();
                spriteBatch.DrawString(Font1, "Ca va etre le UI", new Vector2(50, 430), Color.White);
                spriteBatch.DrawString(Font1, "fps : " + fps, new Vector2(600, 430), Color.White);

                
                
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        protected virtual void DrawGrid()
        {
            int i = 0;
            int j = 0;

            float currentStateX = (_mState.X - _pos.X);
            float currentStateY = (_mState.Y - _pos.Y);

            mouseSquare = new Rectangle(Convert.ToInt32(currentStateX), Convert.ToInt32(currentStateY), 1, 1);

            // which pixel are we?
            float pixelX = _pos.X * -1.0f;
            float pixelY = _pos.Y * -1.0f;

            // 800 / 20 is the number of required square we can see in the viewport
            // +2 is the buffer we need to pixel scrolling
            for (i = 0; i < (800 / 20) + 2; i++)
            {
                // where is the X for the buffer
                // We start at pixelX, we substract the "over 20" and sub 20 for the buffer
                float bufferX = pixelX - (pixelX % 20) - 20 + (20 * i);
                if (bufferX < 0) bufferX = 0;

                // Same old
                for (j = 0; j < (480 / 20) + 2; j++)
                {

                    float bufferY = pixelY - (pixelY % 20) - 20 + (20 * j);
                    if (bufferY < 0) bufferY = 0;

                    if (mouseSquare.Intersects(rect[Convert.ToInt32(bufferX / 20), Convert.ToInt32(bufferY / 20)]))
                    {
                        if (_mState.RightButton == ButtonState.Pressed)
                        {
                            Input(Convert.ToInt32(bufferX / 20), Convert.ToInt32(bufferY / 20), "right");
                        }
                        else if (_mState.LeftButton == ButtonState.Pressed)
                        {
                            num = 0;
                            Input(Convert.ToInt32(bufferX / 20), Convert.ToInt32(bufferY / 20), "left");
                        }
                        else
                        {
                            spriteBatch.Draw(grass, rect[Convert.ToInt32(bufferX / 20), Convert.ToInt32(bufferY / 20)], Color.Orange);
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(grass, rect[Convert.ToInt32(bufferX / 20), Convert.ToInt32(bufferY / 20)], Color.White);
                    }
                }
            }
        }

        protected virtual void DrawUI()
        {
            TestUI = new Rectangle(0,400,800,90);
            
            spriteBatch.Draw(texture,TestUI,Color.Blue);
        }


        protected virtual void Input(int _x, int _y, String button)
        {
            if (button == "left")
            {
                if (objects[_x ,_y] == null)
                {
                    texture.SetData(colorData);
                    
                    
                    path = new PathFinding(new Point(objects[4, 4].Container.X  / 20, objects[4, 4].Container.Y / 20), new Point(_x, _y), objects);
                    path.search();
                }
            }
            if (button == "right")
            {
                if (objects[_x, _y] == null)
                {
                    objects[_x, _y] = new Obj(rect[_x, _y].X, rect[_x, _y].Y, rect[_x, _y].Width, rect[_x, _y].Height, false, texture);
                    //notWalkable.Add(rect[_x, _y], objects[_x, _y]);
                    //notWalkable.Remove(rect[_x, _y]);
                    texture.SetData(colorData);
                    //objects[_x, _y] = null;
                }
            }
            
        }

        protected virtual void Input()
        {
            
            
            _keyState = Keyboard.GetState();

            //Check Move
            if (this.IsActive)
            {
                if (_mState.MiddleButton == ButtonState.Pressed && !mouseSquare.Intersects(TestUI))
                {
                    if (lastX == 0 && lastY == 0)
                    {
                        lastX = _mState.X;
                        lastY = _mState.Y;
                    }
                    if (_mState.X <= lastX && _pos.X > (-worldWidth + gameWidth))
                    {
                        _pos.X = _pos.X + (_mState.X - lastX);
                        lastX = _mState.X;
                        if (_pos.X < (-worldWidth + gameWidth))
                            _pos.X = (-worldWidth + gameWidth);
                    }
                    if (_mState.X >= lastX && _pos.X < -5)
                    {
                        _pos.X = _pos.X + (_mState.X - lastX);
                        lastX = _mState.X;
                        if (_pos.X > 0)
                            _pos.X = 0;
                    }
                    if (_mState.Y <= lastY && _pos.Y > (-worldHeight + gameHeight))
                    {
                        _pos.Y = _pos.Y + (_mState.Y - lastY);
                        lastY = _mState.Y;
                        if (_pos.Y < (-worldHeight + gameHeight))
                            _pos.Y = (-worldHeight + gameHeight);
                    }
                    if (_mState.Y >= lastY && _pos.Y < -5)
                    {
                        _pos.Y = _pos.Y + (_mState.Y - lastY);
                        lastY = _mState.Y;
                        if (_pos.Y > 0)
                            _pos.Y = 0;
                    }
                }
                if (_mState.MiddleButton == ButtonState.Released)
                        {
                            lastX = 0;
                            lastY = 0;
                        }


                    if (_keyState.IsKeyDown(Keys.Right) && objects[4, 4].X < worldWidth - objects[4, 4].Container.Width)
                    {
                        objects[4, 4].X += 1;
                    }
                    if (_keyState.IsKeyDown(Keys.Left) && objects[4, 4].X > 0)
                    {
                        objects[4, 4].X -= 1;
                    }
                    if (_keyState.IsKeyDown(Keys.Down) && objects[4, 4].Y < worldHeight - objects[4, 4].Container.Height)
                    {
                        objects[4, 4].Y += 1;
                    }
                    if (_keyState.IsKeyDown(Keys.Up) && objects[4, 4].Y > 0)
                    {
                        objects[4, 4].Y -= 1;
                    }        

                if (_keyState.IsKeyDown(Keys.A) && _pos.X < 0)
                {
                    _pos.X += 5f;
                }
                if (_keyState.IsKeyDown(Keys.D) && _pos.X > -worldWidth + gameWidth)
                {
                    _pos.X -= 5f;
                }
                if (_keyState.IsKeyDown(Keys.W) && _pos.Y < 0)
                {
                    _pos.Y += 5f;
                }
                if (_keyState.IsKeyDown(Keys.S) && _pos.Y > -800 + gameHeight)
                {
                    _pos.Y -= 5f;
                }
            }
        }

        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }
    }
}
