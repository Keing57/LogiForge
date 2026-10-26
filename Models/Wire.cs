namespace LogicSim.Models
{
    public class Wire
    {
        public Pin Source { get; set; }
        public Pin Target { get; set; }

        public void Propagate()
        {
            if (Source != null && Target != null)
            {
                Target.State = Source.State;
            }
        }
    }
}
