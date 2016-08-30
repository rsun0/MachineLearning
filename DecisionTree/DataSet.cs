using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DataSet
    {
        public IList<DataPoint> Points { get; set; } = new List<DataPoint>();

        public DataSet()
        { }

        public void AddDataPoint(DataPoint point)
        {
            Points.Add(point);
        }

        public void WriteToFile(string filename)
        {
            StreamWriter file = File.CreateText(filename);
            file.WriteLine(Points.Count);
            foreach(var point in Points)
            {
                point.WriteToFile(file);
            }
            file.Close();
        }

        public DataSet(string filename)
        {
            StreamReader file = File.OpenText(filename);
            int size = int.Parse(file.ReadLine());

            for (int i = 0; i < size; ++i)
            {
                DataPoint dp = DataPoint.ReadFromFile(file);
                AddDataPoint(dp);
            }

            file.Close();
        }
    }
}
