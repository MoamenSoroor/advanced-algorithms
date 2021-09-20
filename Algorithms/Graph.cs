using CSharpFunctionalExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDSA
{


    /// <summary>
    /// flexable graph data strucure
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TWeight"></typeparam>
    public class Graph<TNode, TWeight> : IEnumerable<GraphNode<TNode, TWeight>>, IReadOnlyGraph<TNode, TWeight>, IGraph<TNode, TWeight>
    {
        protected Dictionary<TNode, List<(TNode Node, TWeight Weight)>> graph
            = new Dictionary<TNode, List<(TNode Node, TWeight Weight)>>();


        public int Count => graph.Keys.Count;

        public IReadOnlyList<TNode> Nodes => graph.Keys.ToList();

        public Maybe<GraphNode<TNode, TWeight>> this[TNode node]
        {
            get
            {
                if (Contains(node))
                    return new GraphNode<TNode, TWeight>(node, graph[node]);
                else
                    return Maybe.None;
            }
        }


        public Graph()
        {

        }

        public Graph(IEnumerable<TNode> nodes)
        {
            // check null
            foreach (var item in nodes)
                graph[item] = new List<(TNode, TWeight)>();
        }

        public Graph(IGraph<TNode,TWeight> another)
        {
            // check null
            foreach (var item in another)
                graph[item.Node] = item.Neighbours.ToList();
                
        }


        /// <summary>
        /// add node to graph
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result Add(TNode item)
        {
            if (!graph.ContainsKey(item))
            {
                graph[item] = new List<(TNode, TWeight)>();
                return Result.Success();
            }
            else
                return Result.Failure("key exists before");
        }


        public Result AddRange(IEnumerable<TNode> nodes)
        {
            if(nodes.All(node=> !Contains(node)))
            {
                foreach (var item in nodes)
                    graph[item] = new List<(TNode, TWeight)>();
                return Result.Success();
            }
            else
                return Result.Failure("key exists before");
        }


        /// <summary>
        /// connect two nodes together
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result ConnectBoth(TNode item1, TNode item2, TWeight weight)
        {
            var success = graph.ContainsKey(item1)
                && graph.ContainsKey(item2)
                && !graph[item1].Contains((item2, weight))
                && !graph[item2].Contains((item1, weight));
            if (success)
            {
                graph[item1].Add((item2, weight));
                graph[item2].Add((item1, weight));
                return Result.Success();
            }
            else
                return Result.Failure("can't connect the items");
        }

        /// <summary>
        /// connect the first node (item1) with the second node (item2), and 
        /// second node is not connected with the first.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result Connect(TNode item1, TNode item2, TWeight weight)
        {
            var success = graph.ContainsKey(item1)
                && graph.ContainsKey(item2)
                && !graph[item1].Contains((item2, weight));
            if (success)
            {
                graph[item1].Add((item2, weight));
                return Result.Success();
            }
            else
                return Result.Failure("can't connect the items");
        }

        /// <summary>
        /// connect the current node with itself with a weight.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result ConnectSelf(TNode item1, TWeight weight)
        {
            var success = graph.ContainsKey(item1)
                && !graph[item1].Contains((item1, weight));
            if (success)
            {
                graph[item1].Add((item1, weight));
                return Result.Success();
            }
            else
                return Result.Failure("can't connect the items");
        }


        /// <summary>
        /// disconnect both nodes from each other at the specified weight.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result DisconnectBoth(TNode item1, TNode item2, TWeight weight)
        {
            var success = graph.ContainsKey(item1)
                && graph.ContainsKey(item2)
                && graph[item1].Contains((item2, weight))
                && graph[item2].Contains((item1, weight));
            if (success)
            {
                graph[item1].Remove((item2, weight));
                graph[item2].Remove((item1, weight));
                return Result.Success();
            }
            else
                return Result.Failure("can't disconnect the items");
        }

        /// <summary>
        /// connect the node with all other nodes , if one of them is connected before 
        /// , method will fail.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public Result Connect(TNode node, IEnumerable<(TNode node, TWeight weight)> items) 
        {
            var success = graph.ContainsKey(node)
                && !items.Any(i=> !graph.ContainsKey(i.node))
                && !graph[node].Any(c=> items.Contains((c.Node,c.Weight)));
            if (success)
            {
                graph[node].AddRange(items);
                return Result.Success();
            }
            else
                return Result.Failure("can't connect the items");
        }

        /// <summary>
        /// connect the node with all other nodes , if one of them is connected before 
        /// , method will fail.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public Result Connect(TNode node, params (TNode node, TWeight weight)[] items)
        {
            return Connect(node, items.AsEnumerable());
        }

        /// <summary>
        /// connect the node with all other nodes and all other node with that node, if one of them is connected before 
        /// , method will fail.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public Result ConnectBoth(TNode node, IEnumerable<(TNode node, TWeight weight)> items)
        {
            var success = graph.ContainsKey(node)
                && !items.Any(i => !graph.ContainsKey(i.node))
                && !graph[node].Any(c => items.Contains(c))
                && !items.Any(item => graph[item.node].Contains((node, item.weight)));

            if (success)
            {
                graph[node].AddRange(items);
                items.ForEach(item => graph[item.node].Add((node, item.weight)));
                return Result.Success();
            }
            else
                return Result.Failure("can't connect the items");
        }

        /// <summary>
        /// connect the node with all other nodes and all other node with that node, if one of them is connected before 
        /// , method will fail.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public Result ConnectBoth(TNode node, params (TNode node, TWeight weight)[] items)
        {
            return ConnectBoth(node, items.AsEnumerable());
        }


        /// <summary>
        /// disconnect first node form the second one at the specified weight. 
        /// and the second still connected.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result Disconnect(TNode item1, TNode item2, TWeight weight)
        {
            var success = graph.ContainsKey(item1)
                && graph.ContainsKey(item2)
                && graph[item1].Contains((item2, weight));
            if (success)
            {
                graph[item1].Remove((item2, weight));
                return Result.Success();
            }
            else
                return Result.Failure("can't disconnect the items");
        }


        /// <summary>
        /// disconnect node from itself with the specified weight
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result DisconnectSelf(TNode item1, TWeight weight)
        {
            var success = graph.ContainsKey(item1)
                && graph[item1].Contains((item1, weight));
            if (success)
            {
                graph[item1].Remove((item1, weight));
                return Result.Success();
            }
            else
                return Result.Failure("can't connect the items");
        }




        /// <summary>
        /// disconnect both nodes from each other.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public Result DisconnectBoth(TNode item1, TNode item2)
        {
            var success = graph.ContainsKey(item1)
                && graph.ContainsKey(item2)
                && graph[item1].Any(i => i.Node.Equals(item2))
                && graph[item2].Any(i => i.Node.Equals(item1));
            if (success)
            {
                graph[item1].RemoveAll(i => i.Node.Equals(item2));
                graph[item2].RemoveAll(i => i.Node.Equals(item1));
                return Result.Success();
            }
            else
                return Result.Failure("can't disconnect the items");
        }

        /// <summary>
        /// disconnect first node form the second one
        /// and the second still connected.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public Result Disconnect(TNode item1, TNode item2)
        {
            var success = graph.ContainsKey(item1)
                && graph.ContainsKey(item2)
                && graph[item1].Any(i => i.Node.Equals(item2));
            if (success)
            {
                graph[item1].RemoveAll(i => i.Node.Equals(item2));
                return Result.Success();
            }
            else
                return Result.Failure("can't disconnect the items");
        }


        /// <summary>
        /// disconnect node from itself.
        /// </summary>
        /// <param name="item1"></param>
        /// <returns></returns>
        public Result DisconnectSelf(TNode item1)
        {
            return Disconnect(item1, item1);
        }




        /// <summary>
        /// all nodes in the graph
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<TNode> ListAll()
        {
            return graph.Keys.ToList();
        }


        /// <summary>
        /// get all neighbours of a node
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Maybe<IReadOnlyList<(TNode Node, TWeight Weight)>> GetNeighbours(TNode item)
        {
            var isExist = graph.TryGetValue(item, out var result);
            return isExist? result : Maybe.None;
        }


        /// <summary>
        /// check if a node exists in the graph
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TNode item)
        {
            return graph.ContainsKey(item);
        }

        /// <summary>
        /// check if the node is connected with the other one 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="neighbour"></param>
        /// <returns></returns>
        public Result<bool> IsConnected(TNode item, TNode neighbour)
        {
            var fail = !graph.ContainsKey(item) && !graph.ContainsKey(neighbour);
            if (fail) return Result.Failure<bool>("item or it's neighbour are not found");

            if (graph[item].Any(n => n.Node.Equals(neighbour)))
                return true;
            else return false;
        }

        /// <summary>
        /// check if the node is connected with the other one within the passed weight
        /// </summary>
        /// <param name="item"></param>
        /// <param name="neighbour"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result<bool> IsConnected(TNode item, TNode neighbour, TWeight weight)
        {
            var fail = !graph.ContainsKey(item) && !graph.ContainsKey(neighbour);
            if (fail) return Result.Failure<bool>("item or it's neighbour are not found");

            return graph[item].Contains((neighbour, weight));

        }




        /// <summary>
        /// check if the both node is connected two gether.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public Result<bool> IsBothConnected(TNode item, TNode item2)
        {
            var fail = !graph.ContainsKey(item) && !graph.ContainsKey(item2);
            if (fail) return Result.Failure<bool>("item or it's item2 are not found");

            if (graph[item].Any(n => n.Node.Equals(item2)) && graph[item2].Any(n => n.Node.Equals(item)))
                return true;
            else return false;
        }

        /// <summary>
        /// check if the node is self connected
        /// </summary>
        /// <param name="item"></param>
        /// <param name="neighbour"></param>
        /// <returns></returns>
        public Result<bool> IsSelfConnected(TNode item)
        {
            return IsConnected(item, item);
        }


        /// <summary>
        /// check if the node is self connected within the passed weight
        /// </summary>
        /// <param name="item"></param>
        /// <param name="neighbour"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public Result<bool> IsSelfConnected(TNode item, TWeight weight)
        {
            return IsConnected(item, item, weight);
        }


        /// <summary>
        /// remove node from the graph.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result Remove(TNode item)
        {
            bool result = graph.Remove(item);
            if (result == false) return Result.Failure("item not found");

            foreach (var entry in graph)
            {
                entry.Value.RemoveAll(i => i.Node.Equals(item));
            }
            return Result.Success();

        }

        /// <summary>
        /// remove all nodes from the graph that satisfy the predicate
        /// </summary>
        /// <param name="predicate"></param>
        public void RemoveAll(Func<TNode, bool> predicate)
        {

            foreach (var item in graph)
            {
                if (predicate(item.Key))
                    Remove(item.Key);
            }
        }

        IEnumerator<GraphNode<TNode, TWeight>> IEnumerable<GraphNode<TNode, TWeight>>.GetEnumerator()
        {
            foreach (var node in graph)
            {
                yield return new GraphNode<TNode, TWeight>(node.Key, node.Value);
            }
        }

        public IEnumerable<(TNode node, TNode neighbour, TWeight weight)> Flatten()
        {
            return (this as IEnumerable<GraphNode<TNode, TWeight>>)
                .SelectMany(v => v.Flatten());
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<GraphNode<TNode, TWeight>>).GetEnumerator();
        }


        public IEnumerable<(TNode node, TNode neighbour, TWeight weight)> Flatten(TNode node)
        {
            if (Contains(node))
                return graph[node].Select(n => (node: node, neighbour: n.Node, weight: n.Weight));
            else
                return Enumerable.Empty<(TNode node, TNode neighbour, TWeight weight)>();
        }


    }

    public class GraphNode<TNode,TWeight>
    {
        protected readonly TNode node;
        protected readonly IReadOnlyList<(TNode Node, TWeight Weight)> neighbours;


        public GraphNode(TNode node, IReadOnlyList<(TNode Node, TWeight Weight)> neighbours)
        {
            this.node = node;
            this.neighbours = neighbours;
        }

        public TNode Node { get => node; }
        public IReadOnlyList<(TNode Node, TWeight Weight)> Neighbours { get => neighbours; }
    
        public IEnumerable<(TNode node,TNode neighbour,TWeight weight)> Flatten()
        {
            return neighbours.Select(n => (node:Node, neighbour: n.Node, weight: n.Weight));
        }
    
    }

}
