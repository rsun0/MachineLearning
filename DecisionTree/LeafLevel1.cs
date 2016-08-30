using System;
using System.Linq;

namespace DecisionTree
{
    class LeafLevel1 : Leaf
    {
        private static bool splitted = false;

        /// <summary>
        /// Trains the tree
        /// </summary>
        /// <param name="signal">The signal training sample</param>
        /// <param name="background">The background training sample</param>
        public override void Train(DataSet signal, DataSet background)
        {
            if (nVariables == 0)
            {
                nVariables = signal.Points[0].Variables.Count();
            }

            nSignal = signal.Points.Count;
            nBackground = background.Points.Count;

            bool branch = chooseVariable(signal, background);

            if (branch && !splitted)
            {
                splitted = true;

                //Log of branching
                string output = "Split at " + split + " for variable " + variable;
                Console.WriteLine(output);

                output1 = new LeafLevel1();
                output2 = new LeafLevel1();

                DataSet signalLeft = new DataSet();
                DataSet signalRight = new DataSet();
                DataSet backgroundLeft = new DataSet();
                DataSet backgroundRight = new DataSet();

                foreach (var dataPoint in signal.Points)
                {
                    if (doSplit(dataPoint))
                    {
                        signalLeft.AddDataPoint(dataPoint);
                    }
                    else
                    {
                        signalRight.AddDataPoint(dataPoint);
                    }
                }

                foreach (var dataPoint in background.Points)
                {
                    if (doSplit(dataPoint))
                    {
                        backgroundLeft.AddDataPoint(dataPoint);
                    }
                    else
                    {
                        backgroundRight.AddDataPoint(dataPoint);
                    }
                }

                output1.Train(signalLeft, backgroundLeft);
                output2.Train(signalRight, backgroundRight);
            }
            else
            {
                Console.WriteLine("End of branch");
            }
        }
    }
}
