using System;
using System.Collections.Generic;

namespace NeuralNet
{
    class Neuron
    {
        protected List<Neuron> inputs;
        public List<double> Weights { get; set; }

        public Neuron(List<Neuron> inputNeurons)
        {
            if(inputNeurons.Count > 0)
            {
                inputs = new List<Neuron>(inputNeurons);
                inputs.Add(new Bias());
                Weights = new List<double>();
                Random generator = new Random();
                for (int i = 0; i < inputs.Count; i++)
                {
                    Weights.Add(2*generator.NextDouble() - 1);
                }
            }
        }

        public virtual double GetOutput()
        {
            double signal = 0;
            for(int i = 0; i < inputs.Count; i++)
            {
                signal += inputs[i].GetOutput() * Weights[i];
            }
            double output = 1 / (1 + Math.Exp(-signal));
            return output;
        }
    }
}
