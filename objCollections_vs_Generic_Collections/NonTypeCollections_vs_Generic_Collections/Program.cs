using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NonTypeCollections_vs_Generic_Collections
{
    class Program
    {
        static void Main(string[] args)
        {

            var arraylist = new ArrayList();

            var genericListVal = new List<int>();

            var genericListRef = new List<Random>();


            DoTimeTest<int>(arraylist, delegate { return 5; }, 50000);

            DoTimeTest<Random>(arraylist, delegate { return new Random(); }, 50000);


            DoTimeTest<int>(genericListVal, delegate { return 5; }, 50000);

            DoTimeTest<Random>(genericListRef, delegate { return new Random(); }, 50000);


            Console.ReadKey();

        }


        public static void DoTimeTest<T>(IList collection, Func<T> ctor, int countOfItem = 1000)
        {
            var sw = new Stopwatch();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Adding to {0} {1} number of {2}", collection.GetType().Name, countOfItem, ctor().GetType().Name);

            sw.Start();

            for (int i = 0; i == countOfItem; i++)
            {
                T a = ctor();
        
                collection.Add(a);
            }
            sw.Stop();

            Console.WriteLine("Adding timing {0} ticks", sw.ElapsedTicks);
            Console.WriteLine(new string('-', 30));

            sw.Reset();

            Console.WriteLine("Reading (with cast if need) from {0} {1} number of {2}", collection.GetType().Name, countOfItem, ctor().GetType().Name);
            sw.Start();
            foreach (var item in collection)
            {
                if (collection.GetType().IsGenericType)
                {
                    var x = item;
                }

                else
                {
                    T x = (T)item;
                }
            }
            sw.Stop();
            Console.WriteLine("Reading timing {0} ticks", sw.ElapsedTicks);
            Console.WriteLine(new string('*', 50));
      
            sw.Reset();

        }

    }
}
