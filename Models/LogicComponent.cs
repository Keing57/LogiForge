using System.Collections.Generic;

namespace LogicSim.Models
{
    public abstract class LogicComponent
    {
        public List<Pin> Inputs { get; set; } = new List<Pin>();
        public List<Pin> Outputs { get; set; } = new List<Pin>();

        public abstract void Evaluate();
    }
}
