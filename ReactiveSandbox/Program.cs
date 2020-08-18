using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace ReactiveSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<int>() { 1, 4, 2, 8, 3, 2, 19, 7 };
            Console.WriteLine("Reactive? j/n");
            var inp = Console.ReadLine();
            if (inp == "j")
                Reactive(data.ToObservable()
                    .Zip(Observable.Interval(TimeSpan.FromMilliseconds(500)), (val, _) => val));
            else
                Traditional(data);
            Console.ReadLine();
        }

        static void Traditional(IEnumerable<int> data)
        {
            var results = data
                .Where(val => val % 2 == 0)
                .Select(val => val / 2)
                .Skip(1);

            foreach (var val in results)
                Console.WriteLine(val);
        }

        static void Reactive(IObservable<int> data)
        {
            var results = data
                .Where(val => val % 2 == 0)
                .Select(val => val / 2)
                .Skip(1);

            results
                .Subscribe(val => Console.WriteLine(val));
        }
    }
}
