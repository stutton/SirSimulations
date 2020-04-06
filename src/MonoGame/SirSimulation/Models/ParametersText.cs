using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirSimulation.Models
{
    public class ParametersText : Engine.IDrawable
    {
        public ParametersText(SpriteFont header, SpriteFont body, Vector2 position)
        {
            Header = header;
            Body = body;
            Position = position;
        }

        public SpriteFont Header { get; }
        public SpriteFont Body { get; }
        public Vector2 Position { get; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Header, "Parameters", Position, Color.White);

        }
    }
}
