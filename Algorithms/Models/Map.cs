using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdvancedDSA.Models
{
    public class Map
    {

        public const int Dimensions = 1000;

        public static Map CreateRandomMap(int cityWidth, int cityHeight, int cityCount, int spaces, int maxConnectionsPerCity)
        {
            Graph<City, double> map =
                new Graph<City, double>(
                    RandomGenerator.RandomSequence(cityCount, 0, cityHeight, spaces)
                    .Zip(
                    Enumerable.Range(10, cityWidth / spaces).Select(c => c * spaces)
                    .Where(c => RandomGenerator.NextDouble() > 0.3)
                    )
                    .Select((pair, i)
                        => City.Create($"C{i + 1}", new Point(pair.Second, pair.First))));


            map.ForEach((node) =>
            {
                int randomValue = maxConnectionsPerCity > 1 ? RandomGenerator.NextInt(1, maxConnectionsPerCity) : 1;
                map.Where(m => !m.Node.Equals(node) && !node.Neighbours.Any(c => c.Node.Equals(m)))
                    .OrderBy(c => RandomGenerator.NextInt())
                    .Take(randomValue)
                    .ForEach(neighbour =>
                        map.ConnectBoth(node.Node, neighbour.Node, City.Distance(node.Node, neighbour.Node)));

            });

            return new Map(map);
        }

        public static Map CreateSampleMap1(int cityWidth, int cityHeight)
        {
            Graph<City, double> map =
                new Graph<City, double>();



            return new Map(map);
        }

        public static Map Create(IEnumerable<City> cities)
        {
            return new Map(cities);
        }

        public static Map Empty()
        {
            return new Map();
        }


        private Map(Graph<City, double> graph)
        {
            mapGraph = graph;
        }


        private Map(IEnumerable<City> _cities)
        {
            mapGraph.AddRange(Cities);
        }

        private Map() { }


        private Graph<City, double> mapGraph = new Graph<City, double>();

        public IReadOnlyList<City> Cities { get => mapGraph.Nodes; }

        public IReadOnlyGraph<City, double> Graph { get => mapGraph; }










    }


}
