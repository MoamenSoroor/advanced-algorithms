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

    #region Breadth First Search
    // 
    // Breadth First Search
    // --------------------------------------------------------------------------------
    // 

    // 
    public class BreadthFirstSearch
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

            graph.Connect(you, new[] { (mohammed, 1), (gamal, 1) });
            graph.Connect(mohammed, new[] { (omar, 1), (khalid, 1) });
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
            Queue<Person> people = new Queue<Person>();
            List<Person> visited = new List<Person>();

            people.Enqueue(you);

            while (people.Any())
            {
                var person = people.Dequeue();
                if (!visited.Contains(person))
                {

                    Console.WriteLine($"visiting: {person} ");
                    if (CkeckIsPersonMangoSeller(person))
                    {

                        return person;
                    }
                    else
                    {
                        var friends = graph.GetNeighbours(person).Value;
                        friends.ForEach(f => people.Enqueue(f.Node));

                        visited.Add(person);
                    }

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
