using System;
using System.Reactive.Linq;

namespace ReactiveSandbox
{
    static class FizzBuzz
    {
        public static IObservable<string> Create()
        {
            return Observable.Never<string>();
        }
    }
}
