using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace SirSimulation.Engine
{
    public static class Utils
    {
        public static readonly Random Random = new Random();
        public static float NextFloat(this Random r)
        {
            return (float)r.NextDouble();
        }

        public static Vector2 GetCenter(this RectangleF rectangle)
        {
            return new Vector2(
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2);
        }

        [Flags]
        public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }

        public static void DrawStringAligned(this SpriteBatch spriteBatch, SpriteFont font, string text, RectangleF bounds, Alignment align, Color color)
        {
            var size = font.MeasureString(text);
            var pos = bounds.GetCenter();
            var origin = size / 2f;

            if (align.HasFlag(Alignment.Left))
            {
                origin.X += bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Right))
            {
                origin.X -= bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Top))
            {
                origin.Y += bounds.Height / 2 - size.Y / 2;
            }

            if (align.HasFlag(Alignment.Bottom))
            {
                origin.Y -= bounds.Height / 2 - size.Y / 2;
            }

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
