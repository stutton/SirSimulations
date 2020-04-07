using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SirSimulation.Engine;
using SirSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SirSimulation.Models
{
    public class Sir : Engine.IUpdatable, Engine.IDrawable
    {
        private TimeSpan _simulationTime;
        private bool _running = false;

        private List<Person> _people = new List<Person>();
        private List<City> _cities = new List<City>();

        private double _lastSampleTime = -1;

        public Sir(Parameters parameters, int cityCount)
        {
            Parameters = parameters;
            CityCount = cityCount;
        }

        public Parameters Parameters { get; }
        public int CityCount { get; set; }
        public double SampleRate { get; set; } = 1;
        public List<float[]> History { get; } = new List<float[]>();

        public void InitializeCities(Rectangle totalBounds, int totalPopulation)
        {
            for (var i = 0; i < CityCount; i++)
            {
                // TODO: Arrange multiple cities
                var city = new City(
                    new Rectangle(totalBounds.X + 20, totalBounds.Y + 20, totalBounds.Width - 40, totalBounds.Height - 40),
                    totalPopulation / CityCount);
                _cities.Add(city);
            }
        }

        public void InitializePeople(Texture2D sSprite, Texture2D iSprite, Texture2D rSprite)
        {
            foreach(var city in _cities)
            {
                for (var i = 0; i < city.Population; i++)
                {
                    var person = new Person(sSprite, iSprite, rSprite);
                    person.InfectionRadius = Parameters.InfectionRadius;
                    city.AddPerson(person);
                    _people.Add(person);
                }
            }
            var index = Utils.Random.Next(0, _people.Count - 1);
            var patientZero = _people[index];
            patientZero.City.People[patientZero.Status].Remove(patientZero);
            patientZero.SetStatus(InfectionStatus.Infected, new GameTime());
            patientZero.City.People[patientZero.Status].Add(patientZero);
        }

        public void Start()
        {
            _running = true;
        }

        public void Pause()
        {
            _running = false;
        }

        public void Reset()
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var city in _cities)
            {
                city.Draw(gameTime, spriteBatch);

            }
            foreach(var person in _people)
            {
                person.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!_running)
            {
                return;
            }

            var simElapsedTime = gameTime.ElapsedGameTime;
            _simulationTime += simElapsedTime;

            foreach(var city in _cities)
            {
                var doUpdate = true;
                var changed = new List<(InfectionStatus status, Person person)>();
                foreach(var ip in city.People[InfectionStatus.Infected])
                {
                    foreach(var sp in city.People[InfectionStatus.Susceptible])
                    {
                        var dist = Vector2.Distance(ip.Position, sp.Position);
                        if (dist < ip.InfectionRadius && Utils.Random.NextFloat() < Parameters.InfectionRate * gameTime.ElapsedGameTime.TotalSeconds)
                        {
                            sp.SetStatus(InfectionStatus.Infected, gameTime);
                            changed.Add((InfectionStatus.Susceptible, sp));
                            ip.NumInfected++;
                        }
                        if (doUpdate)
                        {
                            sp.Update(_simulationTime, simElapsedTime);
                        }
                    }
                    doUpdate = false;
                    if(gameTime.TotalGameTime.TotalSeconds - ip.InfectionStartTime > Parameters.InfectionDuration)
                    {
                        ip.SetStatus(InfectionStatus.Removed, gameTime);
                        changed.Add((InfectionStatus.Infected, ip));
                    }
                    ip.Update(_simulationTime, simElapsedTime);
                }
                foreach(var (status, person) in changed)
                {
                    city.People[status].Remove(person);
                    city.People[person.Status].Add(person);
                }
                if(city.People[InfectionStatus.Infected].Count == 0)
                {
                    foreach(var sp in city.People[InfectionStatus.Susceptible])
                    {
                        sp.Update(_simulationTime, simElapsedTime);
                    }
                }
                foreach(var rp in city.People[InfectionStatus.Removed])
                {
                    rp.Update(_simulationTime, simElapsedTime);
                }
            }

            // Update sample data
            if (gameTime.TotalGameTime.TotalSeconds - _lastSampleTime > SampleRate)
            {
                History.Add(new float[3] 
                {
                    _cities.Sum(c => c.People[InfectionStatus.Infected].Count),
                    _cities.Sum(c => c.People[InfectionStatus.Susceptible].Count),
                    _cities.Sum(c => c.People[InfectionStatus.Removed].Count),
                });
                _lastSampleTime = gameTime.TotalGameTime.TotalSeconds;
            }
        }
    }
}
