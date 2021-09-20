using AdvancedDSA.Utils;
using System.Drawing;

namespace AdvancedDSA.Models
{
    public class City
    {

        public static bool IsOnCity(City node, Point point)
        {
            return Geometry.Distance(node.Position, point) <= Radius;
        }

        public static bool IsOnCity(City node, City another)
        {
            return Geometry.Distance(node.Position, another.Position) <= Radius;
        }

        public static double Distance(City node, City neighbour)
        {
            return Geometry.Distance(node.Position, neighbour.Position);
        }

        public static City Create(string name, int x, int y)
        {
            return new City() { Name = name, Position = new Point(x, y) };
        }

        public static City Create(string name, double x, double y)
        {
            return new City() { Name = name, Position = new Point((int)x, (int)y) };
        }

        public static City Create(string name, Point position)
        {
            return new City() { Name = name, Position = position };
        }


        public static City Create(Point position)
        {
            return new City() { Name = "_", Position = position };
        }

        public const int Radius = 20;

        private City() { }




        public string Name { get; init; }

        public Point Position { get; init; }


        public override string ToString()
        {
            return $"City: {Name:20} At ({Position.X},{Position.Y})";
        }

        public override bool Equals(object obj)
        {
            return Position == (obj as City)?.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }



    }


}
