using System.Collections.Generic;
using LogicSim.Models;

namespace LogicSim.Engine
{
    public class SimulationEngine
    {
        public List<LogicComponent> Components { get; set; } = new List<LogicComponent>();
        public List<Wire> Wires { get; set; } = new List<Wire>();

        public void Update()
        {
            foreach (var wire in Wires)
            {
                wire.Propagate();
            }

            foreach (var component in Components)
            {
                component.Evaluate();
            }
        }
    }
}
