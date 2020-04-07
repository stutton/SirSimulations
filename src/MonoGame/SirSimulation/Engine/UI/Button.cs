using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirSimulation.Engine.UI
{
    public class Button : Control
    {
        public Button(RectangleF bounds)
            :base(bounds)
        {

        }

        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Action ClickAction { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(Bounds, CurrentColor);
            spriteBatch.DrawStringAligned(Font, Text, Bounds, Utils.Alignment.Center, CurrentColor);
        }

        protected override void OnClick()
        {
            ClickAction();
        }
    }
}
