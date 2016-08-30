using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class Forest
    {
        private const int nTrees = 10;

        private List<BoostableLeaf> trees;
        private List<double> scores;

        /// <summary>
        /// Constructor
        /// </summary>
        public Forest()
        { }

        /// <summary>
        /// Runs an unknown data point
        /// </summary>
        /// <param name="dataPoint">The unknown data point</param>
        /// <returns>The weighted sum of the data point's leaves</returns>
        public double RunDataPoint(DataPoint dataPoint)
        {
            double weightedSum = 0;
            double totalScore = 0;
            for(int i = 0; i < nTrees; i++)
            {
                weightedSum += scores[i] * trees[i].RunDataPoint(dataPoint);
                totalScore += scores[i];
            }
            return weightedSum / totalScore;
        }

        /// <summary>
        /// Trains the forest
        /// </summary>
        /// <param name="signal">The signal training sample</param>
        /// <param name="background">The background training sample</param>
        public void Train(DataSet signal, DataSet background)
        {
            trees = new List<BoostableLeaf>(nTrees);
            scores = new List<double>(nTrees);
            WeightedDataSet weightedSignal = new WeightedDataSet(signal);
            WeightedDataSet weightedBackground = new WeightedDataSet(background);
            
            for (int i = 0; i < nTrees; i++)
            {
                Console.WriteLine("Training tree " + i + ":");

                BoostableLeaf tree = new BoostableLeaf();
                tree.Train(weightedSignal, weightedBackground);
                trees.Add(tree);

                //Calculate score
                scores.Add(calculateBoost(weightedSignal, weightedBackground, tree));

                //Adjust weights
                weightedSignal = adjustWeights(weightedSignal, weightedBackground, tree).Item1;
                weightedBackground = adjustWeights(weightedSignal, weightedBackground, tree).Item2;
            }
        }

        private Tuple<WeightedDataSet, WeightedDataSet> adjustWeights(WeightedDataSet signal, WeightedDataSet background, BoostableLeaf tree)
        {
            double boost = calculateBoost(signal, background, tree);

            //Increase weight for misclassified events
            foreach (WeightedDataPoint p in signal.Points)
            {
                if (tree.RunDataPoint(p) < 0.5)
                {
                    p.Weight *= Math.Exp(boost);
                }
            }
            foreach (WeightedDataPoint p in background.Points)
            {
                if (tree.RunDataPoint(p) > 0.5)
                {
                    p.Weight *= Math.Exp(boost);
                }
            }

            //Renormalize all events
            double totalWeight = signal.GetWeightedSum() + background.GetWeightedSum();
            double normalization = (signal.Points.Count + background.Points.Count) / totalWeight;
            foreach (WeightedDataPoint p in signal.Points)
            {
                p.Weight *= normalization;
            }
            foreach (WeightedDataPoint p in background.Points)
            {
                p.Weight *= normalization;
            }

            return new Tuple<WeightedDataSet, WeightedDataSet>(signal, background);
        }

        private double calculateBoost(WeightedDataSet signal, WeightedDataSet background, BoostableLeaf tree)
        {
            double weightMisclassified = 0;
            foreach(WeightedDataPoint p in signal.Points){
                if (tree.RunDataPoint(p) < 0.5)
                {
                    weightMisclassified += p.Weight;
                }
            }
            foreach(WeightedDataPoint p in background.Points)
            {
                if (tree.RunDataPoint(p) > 0.5)
                {
                    weightMisclassified += p.Weight;
                }
            }
            double totalWeight = signal.GetWeightedSum() + background.GetWeightedSum();
            double r = weightMisclassified/totalWeight;
            return Math.Log((1-r)/r);
        }
    }
}
