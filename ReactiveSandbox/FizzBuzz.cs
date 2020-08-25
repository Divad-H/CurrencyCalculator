using System;
using System.Reactive.Linq;

namespace ReactiveSandbox
{
    static class FizzBuzz
    {
        public static IObservable<string> Create()
        {
            const int count = 1000;

            var ticks = Observable
                .Interval(TimeSpan.FromMilliseconds(500));

            var fizzes = Observable
                .Repeat("")
                .Take(2)
                .Append("fizz")
                .Repeat()
                .Take(count);

            var buzzes = Observable
                .Repeat("")
                .Take(4)
                .Append("buzz")
                .Repeat()
                .Take(count);

            var fizzesBuzzes = fizzes
                .Zip(buzzes, (f, b) => $"{f}{b}");

            var fizzBuzz = ticks
                .Scan(0, (acc, _) => acc + 1)
                .Zip(fizzesBuzzes, (i, fb) => string.IsNullOrEmpty(fb) ? i.ToString() : fb);

            return fizzBuzz;
        }
    }
}
