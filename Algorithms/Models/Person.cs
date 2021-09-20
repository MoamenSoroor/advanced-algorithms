using System.Collections.Generic;

namespace AdvancedAlgorithms.Models
{

    public class Person
    {

        //public static List<Person> GetPersons()
        //{
        //    return new List<Person>()
        //    {
        //        new Person(){ Name="Ahmed", IsMangoSeller=false },
        //        new Person(){ Name="mohammed", IsMangoSeller=false },
        //    }
        //}

        public static Person Create(string name, bool isMangoSeller=false)
        {
            return new Person() { Name = name, IsMangoSeller = isMangoSeller };
        }

        public static Person Create(Person person)
        {
            return new Person() { Name = person.Name, IsMangoSeller = person.IsMangoSeller };
        }


        public string Name { get; init; }

        public bool IsMangoSeller { get; init; }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj?.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
            => $"Person:{Name}, Is mango seller:{IsMangoSeller}";


    }





}
