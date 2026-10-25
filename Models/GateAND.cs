namespace LogicSim.Models
{
    public class GateAND : LogicComponent
    {
        public override void Evaluate()
        {
            if (Inputs.Count >= 2 && Outputs.Count >= 1)
            {
                Outputs[0].State = Inputs[0].State && Inputs[1].State;
            }
        }
    }
}
