namespace LogicSim.Models
{
    public class OutputLED : LogicComponent
    {
        public bool IsLit { get; private set; }

        public override void Evaluate()
        {
            if (Inputs.Count >= 1)
            {
                IsLit = Inputs[0].State;
            }
        }
    }
}
