using AdvancedDSA.Models;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDSA.Algorithms
{

    // 
    // Dijkstra's Algorithm
    // --------------------------------------------------------------------------------
    //  • Breadth-first search is used to calculate the shortest path for
    //      an unweighted graph.
    //  
    //  • Dijkstra’s algorithm is used to calculate the shortest path for
    //      a weighted graph.
    //  
    //  • Dijkstra’s algorithm works when all the weights are positive.
    // 
    //  • If you have negative weights, use the Bellman-Ford algorithm.

    // 
    public class DijkstraAlgorithm
    {


        public static void Test()
        {
            string cairo = "cairo";
            string alex = "alex";
            string giza = "giza";
            string mansoura = "mansoura";
            string menouf = "menouf";
            string suez= "suez"; // goal
            
            //var nasrCity= "nasrCity"
            //var menia = "menia"

            var graph = new Graph<string, int>(new[] {cairo,alex,giza,mansoura,menouf,suez });

            graph.Connect(cairo, (alex,5), (giza,2));
            graph.Connect(alex, (menouf,4), (mansoura,2));
            graph.Connect(giza,(mansoura,7));
            graph.Connect(mansoura, (suez, 1));
            graph.Connect(menouf, (suez, 3), (mansoura, 6));


            Maybe<(List<string> path, int cost)> result =  FindBestPathWithDijkstraAlgorithm(graph, cairo,suez);



            Console.WriteLine("".PadLeft(40, '='));
            string pathStr = result == Maybe.None ? "<< No Path Exists >>" : $"{string.Join(", ", result.Value.path)}";
            Console.WriteLine($"best path--: {pathStr}");
            string costStr = result == Maybe.None ? "<< Infinity >>" : result.Value.cost.ToString();
            Console.WriteLine($"Cost ------: {costStr}");
            Console.WriteLine("".PadLeft(40, '='));
        }

        private static Maybe<(List<string> path, int cost)> FindBestPathWithDijkstraAlgorithm(Graph<string, int> graph, string start,string goal)
        {
            var costs = new Dictionary<string, (Maybe<string> parent, int cost)>();
            var processed = new List<string>();

            // init costs 
            graph.Nodes.ForEach(n => costs[n] = (Maybe.None, int.MaxValue));

            costs[start] = (Maybe.None, 0);

            var lowestNode = FindLowestCostNode(costs, processed);

            while (lowestNode != Maybe.None)
            {
                var node = lowestNode.Value; 
                var nodeCost = costs[node];
                var neighbours = graph.GetNeighbours(node).Value;

                neighbours.Where(n => n.Weight + nodeCost.cost < costs[n.Node].cost)
                    .ForEach(n => costs[n.Node] = (parent: node, cost: n.Weight + nodeCost.cost));

                processed.Add(node);

                lowestNode = FindLowestCostNode(costs, processed);
            }

            if (costs[goal].parent == Maybe.None)
                return Maybe.None;

            List<string> path = new List<string>();
            string currentNode = goal;
            path.Add(currentNode);
            while (currentNode != start)
            {
                currentNode = costs[currentNode].parent.Value;
                path.Add(currentNode);
            }

            
            return (path.Reverse<string>().ToList(),costs[goal].cost);
        }

        private static Maybe<string> FindLowestCostNode(Dictionary<string, (Maybe<string> parent, int cost)> costs, List<string> processed)
        {
            var result = costs.Where(c=> !processed.Contains(c.Key))
                .OrderBy(c => c.Value.cost)
                .FirstOrDefault();
            return result.Key;

        }
    }
}
