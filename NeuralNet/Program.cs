using DecisionTree;
using System.IO;

namespace NeuralNet
{
    class Program
    {
        private const string unknownDataPath = @"decisionTreeData.dat";
        private const string backgroundDataPath = @"stars.dat";
        private const string signalDataPath = @"galaxies.dat";

        private const string challengeOutputPath = @"ChallengeOutput.txt";
        private const string header = "Event\tGalaxy-likeness";

        static void Main(string[] args)
        {
            //Training the neural net
            DataSet signal = new DataSet(signalDataPath);
            DataSet background = new DataSet(backgroundDataPath);
            NeuralNet net = new NeuralNet();
            net.Train(signal, background);

            //Using the neural net
            DataSet unknown = new DataSet(unknownDataPath);
            StreamWriter writer = File.CreateText(challengeOutputPath);
            writer.WriteLine(header);
            for (int i = 0; i < unknown.Points.Count; i++)
            {
                writer.WriteLine(i + "\t" + net.RunDataPoint(unknown.Points[i]));
            }
            writer.Close();
        }
    }
}
