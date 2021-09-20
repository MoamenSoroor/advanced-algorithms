using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDSA
{
    public static class Extensions
    {

        /// <summary>
        /// a Composer Higher order function
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return (value) => f2(f1(value));
        }


        /// <summary>
        /// adapter higher order function that is used reduce that parameters of a function
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="f1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static Func<T1, T3> ReduceParameters<T1, T2, T3>(this Func<T1, T2, T3> f1, T2 t2)
        {
            return (t1) => f1(t1, t2);
        }

        /// <summary>
        /// adapter higher order function that is used reduce that parameters of a function
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="f1"></param>
        /// <param name="t3"></param>
        /// <returns></returns>
        public static Func<T1, T2, T4> ReduceParameters<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f1, T3 t3)
        {
            return (t1, t2) => f1(t1, t2, t3);
        }


        public static Maybe<V> TryGetValue<K, V>(this Dictionary<K, V> dict, K key)
        {

            return dict.TryGetValue(key, out V value) ? value : Maybe.None;
        }


        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException();
            if (action == null) throw new ArgumentNullException();


            if (source is List<T> list) list.ForEach(action);

            foreach (var item in source)
                action(item);

        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null) throw new ArgumentNullException();
            if (action == null) throw new ArgumentNullException();

            if (source is List<T> list) list.ForEach(action);

            int index = 0;
            foreach (var item in source)
                action(item, index++);
        }


    }
}
