using System.IO;

namespace DecisionTree
{
    class Program
    {
        private const string unknownDataPath = @"decisionTreeData.dat";
        private const string backgroundDataPath = @"stars.dat";
        private const string signalDataPath = @"galaxies.dat";

        private const string level1OutputPath = @"Level1Output.txt";
        private const string level2OutputPath = @"Level2Output.txt";
        private const string level3OutputPath = @"Level3Output.txt";
        private const string header = "Event\tGalaxy-likeness";

        static void Main(string[] args)
        {
            //runLevel1();
            //runLevel2();
            runLevel3();
        }

        private static void runLevel1()
        {
            //Training the tree
            DataSet signal = new DataSet(signalDataPath);
            DataSet background = new DataSet(backgroundDataPath);
            LeafLevel1 tree = new LeafLevel1();
            tree.Train(signal, background);

            //Using the tree
            DataSet unknown = new DataSet(unknownDataPath);
            StreamWriter writer = File.CreateText(level1OutputPath);
            writer.WriteLine(header);
            for(int i = 0; i < unknown.Points.Count; i++)
            {
                int result;
                if (tree.RunDataPoint(unknown.Points[i]) > 0.5)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
                writer.WriteLine(i + "\t" + result);
            }
            writer.Close();
        }

        private static void runLevel2()
        {
            //Training the tree
            DataSet signal = new DataSet(signalDataPath);
            DataSet background = new DataSet(backgroundDataPath);
            Leaf tree = new Leaf();
            tree.Train(signal, background);

            //Using the tree
            DataSet unknown = new DataSet(unknownDataPath);
            StreamWriter writer = File.CreateText(level2OutputPath);
            writer.WriteLine(header);
            for (int i = 0; i < unknown.Points.Count; i++)
            {
                writer.WriteLine(i + "\t" + tree.RunDataPoint(unknown.Points[i]));
            }
            writer.Close();
        }

        private static void runLevel3()
        {
            //Training the forest
            DataSet signal = new DataSet(signalDataPath);
            DataSet background = new DataSet(backgroundDataPath);
            Forest forest = new Forest();
            forest.Train(signal, background);

            //Using the forest
            DataSet unknown = new DataSet(unknownDataPath);

            StreamWriter writer = File.CreateText(level3OutputPath);
            writer.WriteLine(header);
            for (int i = 0; i < unknown.Points.Count; i++)
            {
                writer.WriteLine(i + "\t" + forest.RunDataPoint(unknown.Points[i]));
            }
            writer.Close();
        }
    }
}
