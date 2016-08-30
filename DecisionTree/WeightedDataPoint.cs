using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class WeightedDataPoint : AstronomyDataPoint
    {
        public double Weight { get; set; }

        public WeightedDataPoint(DataPoint p, double w) :
            base()
        {
            Variables = p.Variables;
            Weight = w;
        }
    }
}
