using AdvancedAlgorithms.Models;
using AdvancedDSA;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDSA.Algorithms
{

    #region Depth First Search
    // 
    // Depth First Search
    // --------------------------------------------------------------------------------
    // 

    // 
    public class DepthFirstSearch
    {
        public static void Test()
        {

            var you = new Person() { Name = "you", IsMangoSeller = false };
            var mohammed = new Person() { Name = "mohammed", IsMangoSeller = false };
            var gamal = new Person() { Name = "gamal", IsMangoSeller = false };
            var omar = new Person() { Name = "omar", IsMangoSeller = false };
            var khalid = new Person() { Name = "khalid", IsMangoSeller = false }; // 
            var samir = new Person() { Name = "samir", IsMangoSeller = false };
            var mostafa = new Person() { Name = "mostafa", IsMangoSeller = true };



            var graph = new Graph<Person, int>(new[] { you, mohammed, gamal, omar, khalid, samir, mostafa });

            graph.Connect(you, (mohammed, 1), (gamal, 1));
            graph.Connect(mohammed, (omar, 1), (khalid, 1) );
            graph.Connect(khalid, samir, 1);
            graph.Connect(omar, samir, 1);
            graph.Connect(samir, mostafa, 1);
            graph.Connect(gamal, samir, 1);
            graph.Connect(samir, mostafa, 1);

            var result = SearchMangoSeller(graph, you);

            Console.WriteLine("".PadLeft(40, '='));
            Console.WriteLine($"result:{(result.HasValue ? result.Value : "NO Mango Seller")}");
            Console.WriteLine("".PadLeft(40, '='));

        }


        public static Maybe<Person> SearchMangoSeller(Graph<Person, int> graph, Person you)
        {
            Stack<Person> people = new Stack<Person>();
            List<Person> visited = new List<Person>();
            people.Push(you);

            while (people.Any())
            {
                var person = people.Pop();
                Console.WriteLine($"---> Visiting {person.Name}");
                visited.Add(person);
                if (CkeckIsPersonMangoSeller(person))
                {
                    Console.WriteLine($"Mango seller is here: {person.Name}");
                    return person;
                }
                else
                {
                    var neighbours = graph.GetNeighbours(person).Value
                        .Where(p => !visited.Contains(p.Node)).Reverse();

                    neighbours.ForEach(ne =>
                    {
                        people.Push(ne.Node);
                    });


                }

            }

            return Maybe.None;
        }

        private static bool CkeckIsPersonMangoSeller(Person person)
        {
            return person.IsMangoSeller;
        }
    }

    #endregion





}
