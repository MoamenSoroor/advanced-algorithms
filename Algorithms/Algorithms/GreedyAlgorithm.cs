using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDSA.Algorithms
{





    #region Greedy Algorithm
    // 
    // Greedy Algorithm
    // --------------------------------------------------------------------------------
    //  In technical terms: at each step you pick the locally optimal
    // solution, and in the end you’re left with the globally optimal solution.
    // Believe it or not, this simple algorithm finds the optimal solution to this
    // scheduling problem!

    // • Greedy algorithms optimize locally, hoping to end up with a global
    //  optimum.
    // • NP-complete problems have no known fast solution.
    // • If you have an NP-complete problem, your best bet is to use an
    //  approximation algorithm.
    // • Greedy algorithms are easy to write and fast to run, so they make
    //  good approximation algorithms.

    // 
    public class GreedyAlgorithm
    {
        public static void Test()
        {
            var statesNeeded = new HashSet<string>()
            {
                "mt", "wa", "or", "id", "nv", "ut","ca", "az","dummy" // note that dummy is not covered with any station
            };
            var stations = new Dictionary<string, HashSet<string>>()
            {
                ["kone"] = new HashSet<string> { "id", "nv", "ut" },
                ["ktwo"] = new HashSet<string> { "wa", "id", "mt" },
                ["kthree"] = new HashSet<string> { "or", "nv", "ca" },
                ["kfour"] = new HashSet<string> { "nv", "ut" },
                ["kfive"] = new HashSet<string> { "ca", "az" },
            };




            var result = new List<string>();
            while (statesNeeded.Any())
            {
                var bestStation = stations.Select(station
                    => (station, covered: station.Value.Intersect(statesNeeded)))
                    .OrderByDescending(c => c.covered.Count()).FirstOrDefault();

                // NOTE That if a state is not exists in the station dictionary 
                // the while loop will continue to work forever if you comment the nextline
                if (bestStation.covered.Count() == 0) break;

                statesNeeded.ExceptWith(bestStation.covered);
                result.Add(bestStation.station.Key);
            }


            Console.WriteLine("Result: ");
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(r=> $"Station {r}: {string.Join(", ", stations[r])}" )));


        }
    }

    #endregion

    // states_needed = set([“mt”, “wa”, “or”, “id”, “nv”, “ut”,“ca”, “az”])
    // stations = {}
    // stations[“kone”] = set([“id”, “nv”, “ut”])
    // stations[“ktwo”] = set([“wa”, “id”, “mt”])
    // stations[“kthree”] = set([“or”, “nv”, “ca”])
    // stations[“kfour”] = set([“nv”, “ut”])
    // stations[“kfive”] = set([“ca”, “az”])
}
