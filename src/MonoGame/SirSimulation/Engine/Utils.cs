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
    }
}
