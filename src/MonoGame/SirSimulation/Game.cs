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
        /********** Simulation Parameters ***********/
        int    TOTAL_POPULATION   = 500;
        float  INFECTION_RATE     = 0.5f;
        double INFECTION_DURATION = 10;
        /********************************************/

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<Engine.IDrawable> _drawables = new List<Engine.IDrawable>();
        private List<Engine.IUpdatable> _updateables = new List<Engine.IUpdatable>();

        private Texture2D _sSprite;
        private Texture2D _iSprite;
        private Texture2D _rSprite;

        private SirSimulation _sirSimulation;
        private Engine.Graph _iGraph;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _sSprite = Content.Load<Texture2D>("Sprites/Blue16");
            _iSprite = Content.Load<Texture2D>("Sprites/Red16");
            _rSprite = Content.Load<Texture2D>("Sprites/Gray16");

            _sirSimulation = new SirSimulation(INFECTION_RATE, INFECTION_DURATION, 1);
            _sirSimulation.InitializeCities(
                new Rectangle(
                    GraphicsDevice.Viewport.Bounds.X,
                    GraphicsDevice.Viewport.Bounds.Y,
                    GraphicsDevice.Viewport.Bounds.Width,
                    GraphicsDevice.Viewport.Bounds.Height - 200),
                TOTAL_POPULATION);
            _sirSimulation.InitializePeople(_sSprite, _iSprite, _rSprite);
            _sirSimulation.SampleRate = 0.10;
            _drawables.Add(_sirSimulation);
            _updateables.Add(_sirSimulation);

            _iGraph = new Engine.Graph(GraphicsDevice, new Point(GraphicsDevice.Viewport.Bounds.Width - 40, 200 - 20), TOTAL_POPULATION)
            {
                Position = new Vector2(20, GraphicsDevice.Viewport.Bounds.Height - 20),
                Type = Engine.Graph.GraphType.Fill
            };
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

            _iGraph.Draw(_sirSimulation.InfectedHistory, Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
