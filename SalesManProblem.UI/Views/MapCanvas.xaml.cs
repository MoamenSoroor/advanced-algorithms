using AdvancedDSA.Extensions;
using AdvancedDSA.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesManProblem.Views
{
    /// <summary>
    /// Interaction logic for MapCanvas.xaml
    /// </summary>
    public partial class MapCanvas : UserControl
    {
        public MapCanvas()
        {
            InitializeComponent();


            WeakReferenceMessenger.Default
                .Register<Map>(this, HandleMap);


            WeakReferenceMessenger.Default
                .Register<MapPath>(this, HandlePath);

        }




        private Map map;




        private void HandleMap(object recipient, Map message)
        {
            map = message;
            DrawMap(map);
        }


        private void HandlePath(object recipient, MapPath message)
        {
            MyCanvas.Children.Clear();

            Polygon path = new Polygon();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeLineJoin = PenLineJoin.Round;
            path.StrokeStartLineCap = PenLineCap.Round;

            path.Points = new PointCollection(message.Positions
                .Select(p => new System.Windows.Point(p.X, p.Y)));

            this.MyCanvas.Children.Add(path);

            //DrawCities();
        }


        private void DrawMap(Map map)
        {
            MyCanvas.Children.Clear();
            var cities = CreateCities(map);
            var ways = CreateAllConncetions(map);

            UIElement txt = CreateText("hello world", 10, 10);
            MyCanvas.Children.Add(txt);

            ways.ForEach(c => MyCanvas.Children.Add(c));
            cities.ForEach(c => MyCanvas.Children.Add(c));
        }



        private List<UIElement> CreateCities(Map graph)
        {
            List<UIElement> result = new List<UIElement>();

            result.AddRange(CreateCity(graph.Cities.FirstOrDefault(),Brushes.Red));

            result.AddRange(graph.Cities.SelectMany((city) 
                => CreateCity(city, Brushes.Green)));

            return result;
        }


        private List<UIElement> CreateCity(City city,Brush brush)
        { 

            var p1 = city.Position;

            int size = 20;
            var el = new Ellipse() { 
                Width = size, 
                Height = size, 
                Fill = brush
                
            };
            Canvas.SetZIndex(el, 10);
            Canvas.SetLeft(el, p1.X - size / 2);
            Canvas.SetTop(el, p1.Y - size / 2);


            var text = CreateText(city.Name, p1.X + 10, p1.Y + 10);



            return new List<UIElement> { el,text };
        }

        private List<UIElement> CreateAllConncetions(Map graph)
        {
            List<UIElement> result = graph.Graph.SelectMany(node => node.Neighbours.Select(n => (first: node.Node, second: n.Node, weight: n.Weight)))
                .Select(tuple => CreateConncetion(tuple.first, tuple.second, $"{(int)tuple.weight}"))
                .SelectMany(r => r).ToList();

            return result;
        }

        private List<UIElement> CreateConncetion(City first, City second, string annotation)
        {
            int citySize = 20/2;
            var p1 = first.Position;
            var p2 = second.Position;

            Polyline path = new Polyline();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 4;
            path.StrokeLineJoin = PenLineJoin.Round;

            path.Points = new PointCollection();


            
            int x1 =  (p1.X < p2.X) ? p1.X + citySize / 2: p1.X - citySize / 2; 
            int x2 =  (p1.X < p2.X) ? p2.X - citySize / 2: p2.X + citySize / 2;


            int y1 = (p1.Y < p2.Y) ? p1.Y + citySize / 2 : p1.Y - citySize / 2;
            int y2 = (p1.Y < p2.Y) ? p2.Y - citySize / 2 : p2.Y + citySize / 2;


            path.Points.Add(new System.Windows.Point(x1,y1));
            path.Points.Add(new System.Windows.Point(x2,y2));

            //arrow
            int dx = p2.X < p1.X ? 10 : -10;
            int dy = p2.Y < p1.Y ? 10 : -10;

            path.Points.Add(new System.Windows.Point(x2 - dx, y2));
            path.Points.Add(new System.Windows.Point(x2, y2 - dy));
            path.Points.Add(new System.Windows.Point(x2, y2));

            Canvas.SetZIndex(path, 8);


            var txtPosition = AdvancedDSA.Utils.Geometry.Middle(p1, p2);
            var text = CreateText(annotation, txtPosition.X - 10, txtPosition.Y - 10);

            return new List<UIElement>() { path, text };
        }

        private TextBlock CreateText(string text, double x, double y)
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


    }
}
