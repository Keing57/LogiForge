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
        
        private bool isDrawingWire = false;
        private Line currentWireLine;
        private Ellipse currentStartPinNode;

        public MainWindow()
        {
            InitializeComponent();
            engine = new SimulationEngine();
            LogicCanvas.MouseMove += LogicCanvas_MouseMove;
            LogicCanvas.MouseLeftButtonUp += LogicCanvas_MouseLeftButtonUp;
        }

        private void BtnAnd_Click(object sender, RoutedEventArgs e)
        {
            var gate = new GateAND();
            gate.Inputs.Add(new Pin());
            gate.Inputs.Add(new Pin());
            gate.Outputs.Add(new Pin());
            SpawnComponent(gate);
        }

        private void BtnOr_Click(object sender, RoutedEventArgs e)
        {
            var gate = new GateOR();
            gate.Inputs.Add(new Pin());
            gate.Inputs.Add(new Pin());
            gate.Outputs.Add(new Pin());
            SpawnComponent(gate);
        }

        private void BtnNot_Click(object sender, RoutedEventArgs e)
        {
            var gate = new GateNOT();
            gate.Inputs.Add(new Pin());
            gate.Outputs.Add(new Pin());
            SpawnComponent(gate);
        }

        private void BtnSwitch_Click(object sender, RoutedEventArgs e)
        {
            var switchComp = new InputSwitch();
            switchComp.Outputs.Add(new Pin());
            SpawnComponent(switchComp);
        }

        private void BtnLed_Click(object sender, RoutedEventArgs e)
        {
            var led = new OutputLED();
            led.Inputs.Add(new Pin());
            SpawnComponent(led);
        }

        private void SpawnComponent(LogicComponent component)
        {
            engine.Components.Add(component);
            
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
                Text = component.GetType().Name.Replace("Gate", "").Replace("Input", "").Replace("Output", ""),
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
                pinNode.MouseLeftButtonDown += PinNode_MouseLeftButtonDown;
                container.Children.Add(pinNode);
            }

            container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
            container.MouseMove += Container_MouseMove;
            container.MouseLeftButtonUp += Container_MouseLeftButtonUp;

            Canvas.SetLeft(container, 300);
            Canvas.SetTop(container, 250);
            LogicCanvas.Children.Add(container);
        }

        private void PinNode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            isDrawingWire = true;
            currentStartPinNode = (Ellipse)sender;

            Point center = currentStartPinNode.TranslatePoint(new Point(currentStartPinNode.Width / 2, currentStartPinNode.Height / 2), LogicCanvas);

            currentWireLine = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = center.X,
                Y1 = center.Y,
                X2 = center.X,
                Y2 = center.Y
            };

            LogicCanvas.Children.Add(currentWireLine);
            LogicCanvas.CaptureMouse();
        }

        private void LogicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawingWire && currentWireLine != null)
            {
                Point currentPos = e.GetPosition(LogicCanvas);
                currentWireLine.X2 = currentPos.X;
                currentWireLine.Y2 = currentPos.Y;
            }
        }

        private void LogicCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawingWire)
            {
                isDrawingWire = false;
                LogicCanvas.ReleaseMouseCapture();
                LogicCanvas.Children.Remove(currentWireLine);
                currentWireLine = null;
                currentStartPinNode = null;
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
