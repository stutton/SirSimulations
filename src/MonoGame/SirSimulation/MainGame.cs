using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Gui;
using MonoGame.Extended.ViewportAdapters;
using SirSimulation.Engine;

namespace SirSimulation
{
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private GuiSystem _guiSystem;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _guiSystem.ClientSizeChanged();
        }

        protected override void LoadContent()
        {
            var viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
            var guiRenderer = new GuiSpriteBatchRenderer(GraphicsDevice, () => Matrix.Identity);
            var font = Content.Load<BitmapFont>("Calibri");
            Skin.CreateDefault(font);

            base.LoadContent();
        }
    }
}
