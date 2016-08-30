using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class WeightedDataSet
    {
        public IList<WeightedDataPoint> Points { get; set; }

        public WeightedDataSet(DataSet s)
        {
            List<WeightedDataPoint> points = new List<WeightedDataPoint>();
            foreach(DataPoint p in s.Points)
            {
                points.Add(new WeightedDataPoint(p, 1));
            }
            Points = points;
        }

        public WeightedDataSet()
        {
            Points = new List<WeightedDataPoint>();
        }

        public void AddDataPoint(WeightedDataPoint point)
        {
            Points.Add(point);
        }

        public WeightedDataSet(List<WeightedDataPoint> points)
        {
            Points = points;
        }

        public double GetWeightedSum()
        {
            double total = 0;
            foreach(WeightedDataPoint p in Points)
            {
                total += p.Weight;
            }
            return total;
        }
    }
}
