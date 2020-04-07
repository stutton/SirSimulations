using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace SirSimulation.Engine.UI
{
    public abstract class Control : IUpdatable, IDrawable
    {
        protected static MouseState CurrentMouseState { get; private set; }
        protected static MouseState PreviousMouseState { get; private set; }

        public Control(RectangleF bounds)
        {
            Bounds = bounds;
            CurrentColor = Color;
        }

        protected Color CurrentColor { get; private set; }

        public RectangleF Bounds { get; set; }
        public Color Color { get; set; } = Color.White;
        public Color MouseOverColor { get; set; } = Color.CornflowerBlue;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime)
        {
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            if(Bounds.Contains(CurrentMouseState.Position))
            {
                if (CurrentMouseState.LeftButton == ButtonState.Pressed &&
                   PreviousMouseState.LeftButton == ButtonState.Released)
                {
                    OnClick();
                }

                if (!Bounds.Contains(PreviousMouseState.Position))
                {
                    OnMouseOver();
                }
            }
            else
            {
                if (Bounds.Contains(PreviousMouseState.Position))
                {
                    OnMouseLeave();
                }
            }
        }

        protected virtual void OnClick() { }
        protected virtual void OnMouseOver() 
        {
            CurrentColor = MouseOverColor;
        }
        protected virtual void OnMouseLeave() 
        {
            CurrentColor = Color;
        }
    }
}
