using System.Collections.Generic;

namespace NeuralNet
{
    class Bias : Neuron
    {
        public Bias() :
            base(new List<Neuron>())
        { }

        public override double GetOutput()
        {
            return 1;
        }
    }
}
