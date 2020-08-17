using PropertyChanged;
using System;
using System.ComponentModel;
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
        private readonly IDisposable _subscription;

        public ReactiveViewModel()
        {
            var yenCheckedObservable = this.Create(vm => vm.YenChecked);
            var inputAmountObservable = this.Create(vm => vm.InputAmount);

            var resultOvservable = inputAmountObservable
                .CombineLatest(yenCheckedObservable, (inputAmount, isYen)
                    => Observable.FromAsync(async () =>
                    {
                        var rate = await _exchangeRateService.GetExchangeRate(isYen);
                        return new { rate, result = rate * inputAmount };
                    }))
                .Switch();

            _subscription = resultOvservable.Subscribe(res =>
            {
                Result = res.result;
                ConversionFactor = res.rate;
            });
        }
#pragma warning disable CS0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067
        public void Dispose()
            => _subscription.Dispose();
    }
}
