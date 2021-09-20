using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDSA.Tests
{
    public class GraphTests
    {
        

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Graph_AddTwoNodes_Count2()
        {
            Graph<string, int> graph = new Graph<string, int>();
            
            var result1 = graph.Add("C1");
            var result2 = graph.Add("C2");

            Assert.IsTrue(result1.IsSuccess);
            Assert.IsTrue(result2.IsSuccess);
            Assert.IsTrue(graph.Count == 2);
        }


        [Test]
        public void Graph_AddNode_ContainsTrue()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");

            Assert.IsTrue(graph.Contains("C1"));
        }


        [Test]
        public void Graph_Connect_FirstConnectedSecondNotConnected()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");

            var result = graph.Connect("C1", "C2",1);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(graph.IsConnected("C1","C2").Value);
            Assert.IsTrue(graph.IsConnected("C2","C1").Value == false);
            Assert.IsTrue(graph.IsConnected("C1","C2",1).Value);
            Assert.IsTrue(graph.IsConnected("C2","C1",1).Value == false);
        }

        [Test]
        public void Graph_ConnectBoth_FirstAndSecondAreConnected()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");

            var result = graph.ConnectBoth("C1", "C2", 1);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(graph.IsBothConnected("C1", "C2").Value);
            Assert.IsTrue(graph.IsConnected("C1", "C2").Value);
            Assert.IsTrue(graph.IsConnected("C2", "C1").Value);
            Assert.IsTrue(graph.IsConnected("C1", "C2", 1).Value);
            Assert.IsTrue(graph.IsConnected("C2", "C1", 1).Value);
        }



        [Test]
        public void Graph_ConnectEnumerable_ConnectedCorrectly()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");
            graph.Add("C3");
            graph.Add("C4");

            var result = graph.Connect("C1",new[] {("C2",1), ("C3", 1) });

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(graph.IsConnected("C1", "C2").Value);
            Assert.IsTrue(graph.IsConnected("C1", "C3").Value);
            Assert.IsTrue(graph.IsConnected("C1", "C4").Value == false);


            Assert.IsTrue(graph.IsBothConnected("C1", "C2").Value == false);
            Assert.IsTrue(graph.IsBothConnected("C1", "C3").Value == false);
            Assert.IsTrue(graph.IsBothConnected("C1", "C4").Value == false);
        }


        [Test]
        public void Graph_DisconnectBothConnected_FirstConnectedSecondNotConnected()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");
            graph.ConnectBoth("C1", "C2", 1);

            // act
            graph.Disconnect("C1", "C2",1);

            Assert.IsTrue(graph.IsConnected("C1", "C2").Value == false);
            Assert.IsTrue(graph.IsConnected("C2", "C1").Value);
            Assert.IsTrue(graph.IsConnected("C1", "C2", 1).Value == false);
            Assert.IsTrue(graph.IsConnected("C2", "C1", 1).Value);
        }

        [Test]
        public void Graph_EnumerateGraph_ReturnsTheRightNodes()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");
            graph.ConnectBoth("C1", "C2", 1);

            List<GraphNode<string, int>> nodes = new List<GraphNode<string, int>>();


            // act
            foreach (GraphNode<string, int> item in graph)
            {
                nodes.Add(item);
            }


            Assert.IsTrue(nodes[0].Node == "C1");
            Assert.IsTrue(nodes[1].Node == "C2");
            Assert.IsTrue(nodes[0].Neighbours[0].Node == "C2");
            Assert.IsTrue(nodes[1].Neighbours[0].Node == "C1");
            Assert.IsTrue(nodes[0].Neighbours[0].Weight == 1);
            Assert.IsTrue(nodes[1].Neighbours[0].Weight == 1);

        }


        [Test]
        public void Graph_Indexer_ReturnsTheRightNode()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");
            graph.ConnectBoth("C1", "C2", 1);

            var node = graph["C1"].Value;

            Assert.IsTrue(node.Node == "C1");
            Assert.IsTrue(node.Neighbours[0].Node == "C2");
            
        }




        [Test]
        public void Graph_GetNeighbours_ReturnsTheRightNeighbours()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");
            graph.Add("C3");
            graph.Connect("C1", "C2", 1);
            graph.Connect("C1", "C3", 1);

            var expected = new List<(string, int)>()
            {
                ("C2",1),
                ("C3",1),
            };
            
            // act
            var actual = graph.GetNeighbours("C1").Value;

            // assert
            CollectionAssert.AreEqual(expected, actual);

        }


        [Test]
        public void Graph_FlattenGraph_ReturnsTheRightNodes()
        {
            Graph<string, int> graph = new Graph<string, int>();

            graph.Add("C1");
            graph.Add("C2");
            graph.Add("C3");
            graph.ConnectBoth("C1", "C2", 1);
            graph.ConnectBoth("C1", "C3", 2);

            List<(string, string, double)> expected = new List<(string, string, double)>()
            {
                ("C1","C2",1d),
                ("C1","C3",2d),
                ("C2","C1",1d),
                ("C3","C1",2d),
            };


            
            var actual = graph.Flatten().ToList();

            CollectionAssert.AreEqual(expected, actual);

        }



    }
}