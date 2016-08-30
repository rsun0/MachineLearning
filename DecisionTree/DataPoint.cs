using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    abstract public class DataPoint
    {
        public double[] Variables { get; set; }

        public DataPoint(int nvar)
        {
            Variables = new double[nvar];
        }

        abstract public string GetName(int index);

        public void WriteToFile(StreamWriter file)
        {
            file.WriteLine(GetType());
            foreach (var variable in Variables)
            {
                file.WriteLine(variable);
            }
        }

        static public DataPoint ReadFromFile(StreamReader file) 
        {
            string name = file.ReadLine();
            Assembly assembly = typeof(DataPoint).Assembly;
            Type type = assembly.GetType(name);
            DataPoint answer = Activator.CreateInstance(type) as DataPoint;

            for (int i = 0; i < answer.Variables.Length; ++i)
            {
                answer.Variables[i] = Double.Parse(file.ReadLine());
            }

            return answer;
        }
    }
}
