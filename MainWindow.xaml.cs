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
            engine.Components.Add(testGate);

            RenderComponents();
        }

        private void RenderComponents()
        {
            LogicCanvas.Children.Clear();

            foreach (var component in engine.Components)
            {
                var container = new Grid
                {
                    Width = 60,
                    Height = 40,
                    Background = Brushes.Transparent
                };

                var rect = new Rectangle
                {
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                var text = new TextBlock
                {
                    Text = component.GetType().Name.Replace("Gate", ""),
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                container.Children.Add(rect);
                container.Children.Add(text);

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
