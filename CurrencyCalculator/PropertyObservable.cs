using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace CurrencyCalculator
{
    static class PropertyObservable
    {
        public static IObservable<T> FromProperty<T, VM>(this VM viewModel, Expression<Func<VM, T>> getProperty)
            where VM : INotifyPropertyChanged
        {
            var func = getProperty.Compile();
            var propertyName = (getProperty.Body as MemberExpression)?.Member?.Name ?? throw new ArgumentException(nameof(getProperty));
            return Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    x => viewModel.PropertyChanged += x,
                    x => viewModel.PropertyChanged -= x)
                .Where(args => args.EventArgs.PropertyName == propertyName)
                .Select(_ => func(viewModel))
                .StartWith(func(viewModel));
        }
    }
}
