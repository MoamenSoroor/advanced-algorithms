using AdvancedDSA;
using AdvancedDSA.Models;
using CSharpFunctionalExtensions;
using SalesManProblem.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesManProblem.Views
{
    /// <summary>
    /// Interaction logic for MapCreator.xaml
    /// </summary>
    public partial class MapCreator : Window
    {
        public MapCreator()
        {
            InitializeComponent();


            var addCityChecked = Observable.FromEventPattern(CreateCity, "Checked")
                .Select(c => (c.Sender as RadioButton).IsChecked );
            var addCityUnChecked = Observable.FromEventPattern(CreateCity, "Unchecked");


            var addLineChecked = Observable.FromEventPattern(CreateLine, "Checked")
                .Select(c => (c.Sender as RadioButton).IsChecked);

            var addLineUnChecked = Observable.FromEventPattern(CreateLine, "Unchecked");


            var mouseActionSequence = Observable.FromEventPattern<MouseButtonEventArgs>(MyCanvas, nameof(MouseLeftButtonUp))
                .Select(e => e.EventArgs.GetPosition(MyCanvas));

            var citiesSequence = mouseActionSequence
                .Where(m=> CreateCity.IsChecked == true)
                .Select(e => City.Create($"C({(int)e.X},{(int)e.Y})", e.X, e.Y));

            citiesSequence.Subscribe(c => AddToCityGraph(c));


            var lineSequence = mouseActionSequence
                .Select(m=> (position:m, isActive: CreateLine.IsChecked))
                .Buffer(2)
                .Where(pair=> pair.All(p=> p.isActive == true))
                .Select(e => (c1: HitTest(e[0].position), c2: HitTest(e[1].position)))
                .Where(pair => 
                pair.c1.HasValue &&
                pair.c2.HasValue && 
                pair.c1.Value != pair.c2.Value && 
                !IsConnected(pair.c1.Value, pair.c2.Value));



            lineSequence.Subscribe(pair => AddLineToGraph(pair.c1.Value,pair.c2.Value));



        }

        private bool IsConnected(City value1, City value2)
        {
            var result = graph.IsBothConnected(value1, value2);
            return result.IsSuccess ? result.Value : false;
        }

        private Graph<City,double> graph = new Graph<City,double>();

        private static List<UIElement> CreateCityDrawingElements (City city)
        {

            var p1 = city.Position;

            int size = City.Radius;
            var el = new Ellipse()
            {
                Width = size,
                Height = size,
                Stroke = Brushes.Gold,
                StrokeThickness=1,
                Fill = Brushes.Green

            };
            Canvas.SetZIndex(el, 10);
            Canvas.SetLeft(el, p1.X - size / 2);
            Canvas.SetTop(el, p1.Y - size / 2);


            var text = CreateText(city.Name, p1.X + 10, p1.Y + 10);



            return new List<UIElement> { el, text };
        }

        private static List<UIElement> CreateLineDrawingElements(City first, City second, string annotation)
        {
            int halfRadius = City.Radius / 2;
            var p1 = first.Position;
            var p2 = second.Position;

            Polyline path = new Polyline();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 4;
            path.StrokeLineJoin = PenLineJoin.Round;

            path.Points = new PointCollection();



            //double x1 = (p1.X < p2.X) ? p1.X + halfRadius : p1.X - halfRadius;
            //double x2 = (p1.X < p2.X) ? p2.X - halfRadius : p2.X + halfRadius;


            //double y1 = (p1.Y < p2.Y) ? p1.Y + halfRadius : p1.Y - halfRadius;
            //double y2 = (p1.Y < p2.Y) ? p2.Y - halfRadius : p2.Y + halfRadius;

            //p1 = Algorithms.Utils.Geometry.MoveTowords(p1, p2, halfRadius);
            //p2 = Algorithms.Utils.Geometry.MoveTowords(p2, p1, halfRadius);



            path.Points.Add(new System.Windows.Point(p1.X, p1.Y));
            path.Points.Add(new System.Windows.Point(p2.X, p2.Y));

            //arrow
            double dx = p2.X < p1.X ? 10 : -10;
            double dy = p2.Y < p1.Y ? 10 : -10;

            path.Points.Add(new System.Windows.Point(p2.X + dx, p2.Y));
            path.Points.Add(new System.Windows.Point(p2.X, p2.Y + dy));
            path.Points.Add(new System.Windows.Point(p2.X, p2.Y));

            Canvas.SetZIndex(path, 8);


            var txtPosition = AdvancedDSA.Utils.Geometry.Middle(p1, p2);
            var text = CreateText(annotation, txtPosition.X - halfRadius*2, txtPosition.Y - halfRadius*2);

            return new List<UIElement>() { path, text };
        }

        private static TextBlock CreateText(string text, double x, double y)
        {
            var textBlock = new TextBlock()
            {
                Text = text,
                Foreground = Brushes.DarkBlue,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetZIndex(textBlock, 11);
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            return textBlock;
        }

        

        private void DrawElements(List<UIElement> elements)
        {
            elements.ForEach(el => MyCanvas.Children.Add(el));
        }

        private void AddToCityGraph(City city)
        {
            graph.Add(city);
            RenderCanvas();
        }

        private void AddLineToGraph(City p1, City p2)
        {
            graph.Connect(p1, p2, City.Distance(p1, p2));
            RenderCanvas();
        }

        private Maybe<City> HitTest(System.Windows.Point p)
        {
            return graph.FirstOrDefault(c => City.IsOnCity(c.Node , p.ToDrawingPoint()))?.Node;
        }

        private void RenderCanvas()
        {
            MyCanvas.Children.Clear();
            graph.Nodes.ForEach(city => DrawElements(CreateCityDrawingElements(city)));

            graph.Flatten().ForEach(path => DrawElements(CreateLineDrawingElements(path.node, path.neighbour, $"{(int)path.weight}")));
        }


    }


    public enum MapAction
    {
        AddCity, AddLine,RemoveCity,RemoveLine
    }
}
