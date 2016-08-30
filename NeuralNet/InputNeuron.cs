using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet
{
    class InputNeuron : Neuron
    {
        public double Input { get; set; }

        public InputNeuron() :
            base(new List<Neuron>())
        { }

        public override double GetOutput()
        {
            return Input;
        }
    }
}
