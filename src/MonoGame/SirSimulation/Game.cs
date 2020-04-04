using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SirSimulation.Models;
using System.Collections.Generic;
using MonoGame.Extended;

namespace SirSimulation
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Engine.IDrawable> _drawables = new List<Engine.IDrawable>();
        List<Engine.IUpdatable> _updateables = new List<Engine.IUpdatable>();

        private Texture2D _smiley;
        private Texture2D _sick;
        private Texture2D _neutral;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _smiley = Content.Load<Texture2D>("Sprites/Smile16");
            _sick = Content.Load<Texture2D>("Sprites/Sick16");
            _neutral = Content.Load<Texture2D>("Sprites/Neutral16");

            var sirSimulation = new SirSimulation(0.5f, 10, 1);
            sirSimulation.InitializeCities(GraphicsDevice.Viewport.Bounds);
            sirSimulation.InitializePeople(_neutral, _sick, _smiley);
            _drawables.Add(sirSimulation);
            _updateables.Add(sirSimulation);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach(var u in _updateables)
            {
                u.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach(var d in _drawables)
            {
                d.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
