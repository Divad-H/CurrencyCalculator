using PropertyChanged;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CurrencyCalculator
{
    [AddINotifyPropertyChangedInterface]
    public class ReactiveViewModel : INotifyPropertyChanged, IDisposable
    {
        public bool YenChecked { get; set; }
        public int InputAmount { get; set; }
        public decimal ConversionFactor { get; set; }
        public decimal Result { get; set; }

        private readonly ExchangeRateService _exchangeRateService = new ExchangeRateService();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ReactiveViewModel()
        {
            var yenCheckedObservable = this.FromProperty(vm => vm.YenChecked);
            var inputAmountObservable = this.FromProperty(vm => vm.InputAmount);

            var rateObservable = yenCheckedObservable
                .Select(yenChecked => Observable
                    .FromAsync(() => _exchangeRateService.GetExchangeRate(yenChecked)))
                .Switch();

            var resultObservable = inputAmountObservable
                .CombineLatest(rateObservable, (inputAmount, rate)
                    => rate * inputAmount);

            var subscription = resultObservable.Subscribe(res => Result = res);
            _disposables.Add(subscription);

            subscription = rateObservable.Subscribe(rate => ConversionFactor = rate);
            _disposables.Add(subscription);
        }
#pragma warning disable CS0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067
        public void Dispose()
            => _disposables.Dispose();
    }
}
