using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SirSimulation.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;

namespace SirSimulation.Models
{
    public class Person : Engine.IDrawable
    {
        private readonly Texture2D _sSprite;
        private readonly Texture2D _iSprite;
        private readonly Texture2D _rSprite;

        private Texture2D _currentSprite;
        private double _lastStepChange = -1;

        public Person(Texture2D sSprite, Texture2D iSprite, Texture2D rSprite)
        {
            _sSprite = sSprite;
            _iSprite = iSprite;
            _rSprite = rSprite;

            _currentSprite = _sSprite;
            Status = InfectionStatus.Susceptible;
        }

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float MaxSpeed { get; set; } = 40;
        public Rectangle Bounds { get; set; }
        public Vector2 GravityWell { get; set; } = Vector2.Zero;
        public float GravityStrength { get; set; } = 600000;
        public float WanderStepSize { get; set; } = 140;
        public double WanderStepDuration { get; set; } = 2;
        public float WallBuffer { get; set; } = 20;
        public float WallStrength { get; set; } = 10000;

        public InfectionStatus Status { get; private set; }
        public float InfectionRadius { get; set; }
        public double InfectionStartTime { get; private set; } = float.PositiveInfinity;
        public double InfectionEndTime { get; private set; } = float.PositiveInfinity;
        public int NumInfected { get; set; } = 0;
        public City City { get; set; }

        public void SetStatus(InfectionStatus newStatus, GameTime gameTime)
        {
            switch (newStatus)
            {
                case InfectionStatus.Susceptible:
                    _currentSprite = _sSprite;
                    break;
                case InfectionStatus.Infected:
                    _currentSprite = _iSprite;
                    InfectionStartTime = gameTime.TotalGameTime.TotalSeconds;
                    break;
                case InfectionStatus.Removed:
                    _currentSprite = _rSprite;
                    InfectionEndTime = gameTime.TotalGameTime.TotalSeconds;
                    break;
                default:
                    break;
            }
            Status = newStatus;
        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            var totalForce = Vector2.Zero;

            // Wander
            if (WanderStepSize != 0)
            {
                if (totalTime.TotalSeconds - _lastStepChange > WanderStepDuration)
                {
                    var vect = Constants.Right.Rotate(MathHelper.TwoPi * Utils.Random.NextFloat());
                    GravityWell = Position + (vect * WanderStepSize);
                    _lastStepChange = totalTime.TotalSeconds;
                }
            }

            if (GravityWell != null)
            {
                var toWell = GravityWell - Position;
                var dist = Vector2.Distance(GravityWell, Position);
                if (dist > 0)
                {
                    totalForce += GravityStrength * toWell / ((float)Math.Pow(dist, 3));
                }
            }

            // Avoid walls
            float wallForceX = 0;
            float wallForceY = 0;
            var toRight = Bounds.Right - Position.X;
            if(toRight < 0)
            {
                Velocity = new Vector2(Math.Abs(Velocity.X), Velocity.Y);
            }
            wallForceX -= Math.Max(-1f / WallBuffer + 1f / toRight, 0);
            var toLeft = Position.X - Bounds.Left;
            if(toLeft < 0)
            {
                Velocity = new Vector2(Math.Abs(Velocity.X), Velocity.Y);
            }
            wallForceX += Math.Max(-1f / WallBuffer + 1f / toLeft, 0);
            var toTop = Position.Y - Bounds.Top;
            if(toTop < 0)
            {
                Velocity = new Vector2(Velocity.X, Math.Abs(Velocity.Y));
            }
            wallForceY += Math.Max(-1f / WallBuffer + 1f / toTop, 0);
            var toBottom = Bounds.Bottom - Position.Y;
            if(toBottom < 0)
            {
                Velocity = new Vector2(Velocity.X, -Math.Abs(Velocity.Y));
            }
            wallForceY -= Math.Max(-1f / WallBuffer + 1f / toBottom, 0);
            var wallForce = new Vector2(wallForceX, wallForceY);
            totalForce += wallForce * WallStrength;

            // Limit speed
            var speed = Velocity.Normal();
            if (speed > MaxSpeed)
            {
                Velocity *= MaxSpeed / speed;
            }

            var dt = (float)elapsedTime.TotalSeconds;

            Velocity += totalForce * dt;

            Position += Velocity * dt;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var pos = Position - new Vector2(_sSprite.Width / 2, _sSprite.Height / 2);
            spriteBatch.Draw(_currentSprite, pos, Color.White);
        }

    }
}
