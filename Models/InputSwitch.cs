namespace LogicSim.Models
{
    public class InputSwitch : LogicComponent
    {
        public bool IsOn { get; set; }

        public override void Evaluate()
        {
            if (Outputs.Count >= 1)
            {
                Outputs[0].State = IsOn;
            }
        }
    }
}
