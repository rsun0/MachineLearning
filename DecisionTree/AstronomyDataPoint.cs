using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class AstronomyDataPoint : DataPoint
    {
        private const int eventSize = 8;

        private static string[] names = { "MTot", "MCore", "log(Area)", "Ellip", "IR1", "S", "IR2", "IR3" };

        public AstronomyDataPoint() :
            base(eventSize)
        { }

        public override string GetName(int index)
        {
            return names[index];
        }
    }
}
