namespace LogicSim.Models
{
    public class GateNOT : LogicComponent
    {
        public override void Evaluate()
        {
            if (Inputs.Count >= 1 && Outputs.Count >= 1)
            {
                Outputs[0].State = !Inputs[0].State;
            }
        }
    }
}
