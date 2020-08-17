using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CurrencyCalculator
{
    public class ReactiveViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Property Lametta
        private bool _yenChecked = true;
        public bool YenChecked
        {
            get => _yenChecked;
            set
            {
                if (value == _yenChecked)
                    return;
                _yenChecked = value;
                NotifyPropertyChanged();
            }
        }

        private int _inputAmount;
        public int InputAmount
        {
            get => _inputAmount;
            set
            {
                if (value == _inputAmount)
                    return;
                _inputAmount = value;
                NotifyPropertyChanged();
            }
        }

        private decimal _conversionFactor;
        public decimal ConversionFactor
        {
            get => _conversionFactor;
            set
            {
                if (value == _conversionFactor)
                    return;
                _conversionFactor = value;
                NotifyPropertyChanged();
            }
        }

        private decimal _result = 0;
        public decimal Result
        {
            get => _result;
            set
            {
                if (value == _result)
                    return;
                _result = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

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



        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void Dispose()
            => _subscription.Dispose();
    }
}
