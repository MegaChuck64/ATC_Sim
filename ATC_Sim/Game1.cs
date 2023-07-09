using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ATC_Sim
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Texture2D RadarBackground;
        private Texture2D RadarLine;
        private float radarAngle;
        private List<Plane> planes;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 720;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            RadarBackground = GraphicsHelper.CreateCircle(360, GraphicsDevice, new Color(20, 20, 20));
            RadarLine = GraphicsHelper.CreateRect(360, 1, GraphicsDevice, Color.Green);
            planes = new List<Plane>();
            var rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                planes.Add(new Plane(10, new Vector2(rand.Next(20, 700), rand.Next(20, 700)), GraphicsDevice));
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //var keyState = Keyboard.GetState();            
            
            radarAngle += 100f * dt;
            if (radarAngle >= 360f)
                radarAngle = 0f;



            foreach (var plane in planes)
            {
                plane.Update(dt, IsRadarLineOverPlane(plane));
            }

            base.Update(gameTime);
        }

        private bool IsRadarLineOverPlane(Plane plane)
        {
            var center = new Vector2(360, 360);
            var direction = center - plane.Location;
            var angle = (float)System.Math.Atan2(direction.Y, direction.X);
            var angleDegrees = MathHelper.ToDegrees(angle);
            //var radarAngleDegrees = MathHelper.ToDegrees(radarAngle);
            var diff = System.Math.Abs(angleDegrees - (radarAngle - 180));
            return diff < 1f;
        }    
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                null,
                null,
                null);

            _spriteBatch.Draw(RadarBackground, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(RadarLine, new Vector2(360, 360), null, Color.White, MathHelper.ToRadians(radarAngle), new Vector2(0, 0), 1, SpriteEffects.None, 0);
            foreach (var plane in planes)
            {
                plane.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}