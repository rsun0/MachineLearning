using System;
using System.Linq;

namespace DecisionTree
{
    public class Leaf
    {
        //The number of split values to test when finding the best
        private const int nTestSplitValue = 100;
        //The minimum number of points per leaf
        private const int minPoints = 50;
        //The number of variables per data point
        protected static int nVariables = 0;

        protected Leaf output1 = null;
        protected Leaf output2 = null;
        protected double split;
        protected int variable;

        protected int nBackground = 0;
        protected int nSignal = 0;

        public Leaf() :
            this(0, 0)
        { }

        public Leaf(int variable, double split)
        {
            this.variable = variable;
            this.split = split;
        }

        public bool IsFinal()
        {
            return output1 == null || output2 == null;
        }

        /// <summary>
        /// Calculates the purity of the leaf
        /// </summary>
        /// <returns>The purity</returns>
        public double GetPurity()
        {
            if (nBackground + nSignal == 0)
            {
                throw new SystemException("Leaf contains no training data points.");
            }
            return (double) nSignal / (nBackground + nSignal);
        }

        /// <summary>
        /// Runs an unknown data point
        /// </summary>
        /// <param name="dataPoint">The unknown data point</param>
        /// <returns>The purity of the data point's leaf</returns>
        public double RunDataPoint(DataPoint dataPoint)
        {
            if (IsFinal())
            {
                return GetPurity();
            }

            if (doSplit(dataPoint))
            {
                return output1.RunDataPoint(dataPoint);
            }
            else
            {
                return output2.RunDataPoint(dataPoint);
            }
        }

        protected bool doSplit(DataPoint dataPoint)
        {
            return dataPoint.Variables[variable] <= split;
        }

        /// <summary>
        /// Classifies a data point with a cut
        /// </summary>
        /// <param name="dataPoint">The data point</param>
        /// <param name="variable">The considered variable</param>
        /// <param name="split">The cut value</param>
        /// <returns>If the data point goes to the left</returns>
        private bool doSplit2(DataPoint dataPoint, int variable, double split)
        {
            return dataPoint.Variables[variable] <= split;
        }

        /// <summary>
        /// Trains the tree
        /// </summary>
        /// <param name="signal">The signal training sample</param>
        /// <param name="background">The background training sample</param>
        public virtual void Train(DataSet signal, DataSet background)
        {
            if (nVariables == 0)
            {
                nVariables = signal.Points[0].Variables.Count();
            }

            nSignal = signal.Points.Count;
            nBackground = background.Points.Count;

            bool branch = chooseVariable(signal, background);

            if (branch)
            {
                string output = "Split at " + split + " for variable " + variable;
                Console.WriteLine(output);

                output1 = new Leaf();
                output2 = new Leaf();

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

        /// <summary>
        /// Sets the split value and variable if a split is desirable
        /// </summary>
        /// <param name="signal">The signal training sample</param>
        /// <param name="background">The background training sample</param>
        /// <returns>If a split is to be executed</returns>
        protected bool chooseVariable(DataSet signal, DataSet background)
        {
            // TODO set the values of variable and split here		
            // Return true if you were able to find a useful variable, and false if you were not and want to stop calculation here

            //Find best purity for the best variable and cut value
            int bestVariable = -1;
            double bestPurity = -1;
            double bestCutValue = 0;
            for (int testVariable = 0; testVariable < nVariables; testVariable++)
            {
                double[] testResults = findBestCut(signal, background, testVariable);
                if (testResults[0] > bestPurity)
                {
                    bestPurity = testResults[0];
                    bestVariable = testVariable;
                    bestCutValue = testResults[1];
                }
            }

            //Check whether bestPurity is better than current purity
            if (bestPurity < GetPurity())
            {
                return false;
            }

            variable = bestVariable;
            split = bestCutValue;
            return true;
        }

        /// <summary>
        /// Finds the best cut value for a given variable
        /// </summary>
        /// <param name="signal">The signal training sample</param>
        /// <param name="background">The background training sample</param>
        /// <param name="variable">The variable to find the cutting value for</param>
        /// <returns>The best purity and cutting value</returns>
        private double[] findBestCut(DataSet signal, DataSet background, int variable)
        {
            //The highest and lowest values of the variable in the data sets
            double max = Double.MinValue;
            double min = Double.MaxValue;
            foreach (DataPoint p in signal.Points)
            {
                if (p.Variables[variable] > max)
                {
                    max = p.Variables[variable];
                }
                if (p.Variables[variable] < min)
                {
                    min = p.Variables[variable];
                }
            }
            foreach (DataPoint p in background.Points)
            {
                if (p.Variables[variable] > max)
                {
                    max = p.Variables[variable];
                }
                if (p.Variables[variable] < min)
                {
                    min = p.Variables[variable];
                }
            }

            double bestPurity = -1;
            double bestValue = min;
            for (double testValue = min; testValue < max; testValue += (max-min)/nTestSplitValue)
            {
                double testPurity = testCut(signal, background, variable, testValue);
                if (testPurity > bestPurity)
                {
                    bestPurity = testPurity;
                    bestValue = testValue;
                }
            }

            double[] results = { bestPurity, bestValue };
            return results;
        }

        /// <summary>
        /// Tests the purity of a given cut
        /// </summary>
        /// <param name="signal">The signal training sample</param>
        /// <param name="background">The background training sample</param>
        /// <param name="variable">The variable to use</param>
        /// <param name="cutValue">The value to cut at</param>
        /// <returns>The purity of the purer daughter leaf or -1 if minPoints is not met</returns>
        private double testCut(DataSet signal, DataSet background, int variable, double cutValue)
        {
            DataSet signalLeft = new DataSet();
            DataSet signalRight = new DataSet();
            DataSet backgroundLeft = new DataSet();
            DataSet backgroundRight = new DataSet();

            foreach (var dataPoint in signal.Points)
            {
                if (doSplit2(dataPoint, variable, cutValue))
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
                if (doSplit2(dataPoint, variable, cutValue))
                {
                    backgroundLeft.AddDataPoint(dataPoint);
                }
                else
                {
                    backgroundRight.AddDataPoint(dataPoint);
                }
            }

            //Do not use this cut if minPoints is not met
            if (signalLeft.Points.Count+backgroundLeft.Points.Count < minPoints ||
                signalRight.Points.Count+backgroundRight.Points.Count < minPoints)
            {
                return -1;
            }

            double purityLeft = (double) signalLeft.Points.Count / (backgroundLeft.Points.Count+signalLeft.Points.Count);
            double purityRight = (double) signalRight.Points.Count / (backgroundRight.Points.Count+signalRight.Points.Count);
            return Math.Max(purityLeft, purityRight);
        }
    }
}
