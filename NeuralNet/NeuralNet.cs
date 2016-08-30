using System.Collections.Generic;
using DecisionTree;
using System.Linq;
using System;

namespace NeuralNet
{
    class NeuralNet
    {
        private const int nHidden = 10;
        private const int nOutputs = 2;
        //How many times the entire training sample is run through
        private const int iterations = 100;
        private const double learningRate = 10;

        private List<InputNeuron> inputs;
        private List<Neuron> hidden;
        private List<Neuron> outputs;

        public NeuralNet()
        { }

        public double RunDataPoint(DataPoint point)
        {
            setInputs(point);
            if(outputs[0].GetOutput() > outputs[1].GetOutput())
            {
                return 1;
            }
            return -1;
        }

        public void Train(DataSet signal, DataSet background)
        {
            //Initialize neurons
            inputs = new List<InputNeuron>(signal.Points[0].Variables.Count());
            for(int i = 0; i < signal.Points[0].Variables.Count(); i++)
            {
                inputs.Add(new InputNeuron());
            }
            hidden = new List<Neuron>(nHidden);
            for(int i = 0; i < nHidden; i++)
            {
                hidden.Add(new Neuron(new List<Neuron>(inputs)));
            }
            outputs = new List<Neuron>(nOutputs);
            for(int i = 0; i < nOutputs; i++)
            {
                outputs.Add(new Neuron(hidden));
            }

            //Training
            for(int iteration = 0; iteration < iterations; iteration++)
            {
                Console.WriteLine("Iteration " + iteration + ":");
                for(int pointIndex = 0; pointIndex < signal.Points.Count; pointIndex++)
                {
                    //Run a signal data point
                    DataPoint signalEvent = signal.Points[pointIndex];
                    adjustWeights(signalEvent, true);

                    //Run a background data point
                    DataPoint backgroundEvent = background.Points[pointIndex];
                    adjustWeights(backgroundEvent, false);
                }
            }
        }

        private void adjustWeights(DataPoint point, bool signal)
        {
            List<double> target = new List<double>(nOutputs);
            if (signal)
            {
                target.Add(1);
                target.Add(0);
            }
            else
            {
                target.Add(0);
                target.Add(1);
            }

            List<double> deltaHidden = new List<double>(hidden.Count);
            List<double> deltaOutputs = new List<double>(outputs.Count);

            //Get the delta value for the output layer
            for(int i = 0; i < outputs.Count; i++)
            {
                deltaOutputs.Add(outputs[i].GetOutput() * (1 - outputs[i].GetOutput()) * (target[i] - outputs[i].GetOutput()));
            }

            //Get the delta value for the hidden layer
            for(int i = 0; i < hidden.Count; i++)
            {
                double error = 0;
                for(int j = 0; j < outputs.Count; j++)
                {
                    error += outputs[j].Weights[i] * deltaOutputs[j];
                }
                deltaHidden.Add(hidden[i].GetOutput() * (1 - hidden[i].GetOutput()) * error);
            }

            //Update the weights for the output layer
            for(int i = 0; i < outputs.Count; i++)
            {
                for(int j = 0; j < hidden.Count; j++)
                {
                    outputs[i].Weights[j] += learningRate * deltaOutputs[i] * hidden[j].GetOutput();
                }
            }

            //Update the weights for the hidden layer
            for(int i = 0; i < hidden.Count; i++)
            {
                for(int j = 0; j < inputs.Count; j++)
                {
                    hidden[i].Weights[j] += learningRate * deltaHidden[i] * inputs[j].GetOutput();
                }
            }
        }

        private void setInputs(DataPoint point)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Input = point.Variables[i];
            }
        }
    }
}
