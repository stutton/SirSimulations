using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirSimulation.Models
{
    public class Parameters
    {
        public int TotalPopulation { get; set; }
        public float InfectionRate { get; set; }
        public double InfectionDuration { get; set; }
        public float InfectionRadius { get; set; }
    }
}
