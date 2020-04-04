using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using SirSimulation.Engine;

namespace SirSimulation.Models
{
    public class City: Engine.IDrawable
    {
        public City(Rectangle bounds, int population)
        {
            Bounds = bounds;
            Population = population;
        }

        public Rectangle Bounds { get; set; }
        public int Population { get; set; }
        public Dictionary<InfectionStatus, List<Person>> People { get; set; } = new Dictionary<InfectionStatus, List<Person>>
        {
            { InfectionStatus.Susceptible, new List<Person>() },
            { InfectionStatus.Infected, new List<Person>() },
            { InfectionStatus.Removed, new List<Person>() }
        };

        public void AddPerson(Person p)
        {
            People[p.Status].Add(p);
            p.City = this;
            p.Position = new Vector2(
                Bounds.Width * Utils.Random.NextFloat() + Bounds.X,
                Bounds.Height * Utils.Random.NextFloat() + Bounds.Y);
            p.Bounds = Bounds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(Bounds, Color.White, 2);
        }
    }
}
