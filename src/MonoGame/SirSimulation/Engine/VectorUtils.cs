using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirSimulation.Engine
{
    public static class VectorUtils
    {
        //public static Vector2 Rotate(this Vector2 v, float angle)
        //{
        //    return Vector2.Transform(v, Matrix.CreateRotationZ(angle));
        //}

        public static float Normal(this Vector2 v)
        {
            return (float)Math.Pow(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2), 0.5);
        }
    }
}
