using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LogicSim.Engine;
using LogicSim.Models;

namespace LogicSim
{
    public partial class MainWindow : Window
    {
        private SimulationEngine engine;
        private bool isDragging = false;
        private Point clickPosition;
        private UIElement draggedElement;

        public MainWindow()
        {
            InitializeComponent();
            engine = new SimulationEngine();
            
            var testGate = new GateAND();
            testGate.Inputs.Add(new Pin());
            testGate.Inputs.Add(new Pin());
            testGate.Outputs.Add(new Pin());
            engine.Components.Add(testGate);

            RenderComponents();
        }

        private void RenderComponents()
        {
            LogicCanvas.Children.Clear();

            foreach (var component in engine.Components)
            {
                var container = new Canvas
                {
                    Width = 80,
                    Height = 40,
                    Background = Brushes.Transparent
                };

                var rect = new Rectangle
                {
                    Width = 60,
                    Height = 40,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(rect, 10);
                Canvas.SetTop(rect, 0);

                var text = new TextBlock
                {
                    Text = component.GetType().Name.Replace("Gate", ""),
                    FontWeight = FontWeights.Bold
                };
                text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Canvas.SetLeft(text, 10 + (60 - text.DesiredSize.Width) / 2);
                Canvas.SetTop(text, (40 - text.DesiredSize.Height) / 2);

                container.Children.Add(rect);
                container.Children.Add(text);

                for (int i = 0; i < component.Inputs.Count; i++)
                {
                    var pinNode = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    double yPos = (40.0 / (component.Inputs.Count + 1)) * (i + 1) - 5;
                    Canvas.SetLeft(pinNode, 5);
                    Canvas.SetTop(pinNode, yPos);
                    container.Children.Add(pinNode);
                }

                for (int i = 0; i < component.Outputs.Count; i++)
                {
                    var pinNode = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    double yPos = (40.0 / (component.Outputs.Count + 1)) * (i + 1) - 5;
                    Canvas.SetLeft(pinNode, 65);
                    Canvas.SetTop(pinNode, yPos);
                    container.Children.Add(pinNode);
                }

                container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
                container.MouseMove += Container_MouseMove;
                container.MouseLeftButtonUp += Container_MouseLeftButtonUp;

                Canvas.SetLeft(container, 100);
                Canvas.SetTop(container, 100);
                LogicCanvas.Children.Add(container);
            }
        }

        private void Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            draggedElement = (UIElement)sender;
            clickPosition = e.GetPosition(draggedElement);
            draggedElement.CaptureMouse();
        }

        private void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedElement != null)
            {
                Point currentPosition = e.GetPosition(LogicCanvas);
                Canvas.SetLeft(draggedElement, currentPosition.X - clickPosition.X);
                Canvas.SetTop(draggedElement, currentPosition.Y - clickPosition.Y);
            }
        }

        private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging && draggedElement != null)
            {
                isDragging = false;
                draggedElement.ReleaseMouseCapture();
                draggedElement = null;
            }
        }
    }
}
