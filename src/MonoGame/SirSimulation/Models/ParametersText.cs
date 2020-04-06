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
        private static readonly Vector2 _headerHeight = new Vector2(0, 30f);
        private static readonly Vector2 _bodyHeight = new Vector2(0, 20f);
        public ParametersText(Parameters parameters, SpriteFont header, SpriteFont body, Vector2 position)
        {
            Header = header;
            Body = body;
            Position = position;
            Parameters = parameters;
        }

        public Parameters Parameters { get; }
        public SpriteFont Header { get; }
        public SpriteFont Body { get; }
        public Vector2 Position { get; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var currentPos = Position;
            spriteBatch.DrawString(Header, "Parameters", currentPos, Color.White);
            currentPos += _headerHeight;
            spriteBatch.DrawString(Body, $"Population: {Parameters.TotalPopulation}", currentPos, Color.White);
            currentPos += _bodyHeight;
            spriteBatch.DrawString(Body, $"Infection Rate: {Parameters.InfectionRate}", currentPos, Color.White);
            currentPos += _bodyHeight;
            spriteBatch.DrawString(Body, $"Infection Duration: {Parameters.InfectionDuration}", currentPos, Color.White);
            currentPos += _bodyHeight;
            spriteBatch.DrawString(Body, $"Infection Radius: {Parameters.InfectionRadius}", currentPos, Color.White);
        }
    }
}
