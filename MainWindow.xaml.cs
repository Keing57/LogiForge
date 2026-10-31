using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LogicSim.Engine;
using LogicSim.Models;

namespace LogicSim
{
    public partial class MainWindow : Window
    {
        private SimulationEngine engine;

        public MainWindow()
        {
            InitializeComponent();
            engine = new SimulationEngine();
            
            var testGate = new GateAND();
            engine.Components.Add(testGate);

            RenderComponents();
        }

        private void RenderComponents()
        {
            LogicCanvas.Children.Clear();

            foreach (var component in engine.Components)
            {
                var rect = new Rectangle
                {
                    Width = 60,
                    Height = 40,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(rect, 100);
                Canvas.SetTop(rect, 100);
                LogicCanvas.Children.Add(rect);

                var text = new TextBlock
                {
                    Text = component.GetType().Name.Replace("Gate", ""),
                    FontWeight = FontWeights.Bold
                };

                text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                
                Canvas.SetLeft(text, 100 + (60 - text.DesiredSize.Width) / 2);
                Canvas.SetTop(text, 100 + (40 - text.DesiredSize.Height) / 2);
                LogicCanvas.Children.Add(text);
            }
        }
    }
}
