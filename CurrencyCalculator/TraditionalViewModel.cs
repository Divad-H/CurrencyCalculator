using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CurrencyCalculator
{
    public class TraditionalViewModel : INotifyPropertyChanged
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
                CurrencyChanged(value);
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
                InputAmountChanged(value);
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

        private ExchangeRateService _exchangeRateService = new ExchangeRateService();

        async void InputAmountChanged(int newAmount)
        {
            ConversionFactor = await _exchangeRateService.GetExchangeRate(YenChecked);
            Result = ConversionFactor * newAmount;
        }

        async void CurrencyChanged(bool isYen)
        {
            ConversionFactor = await _exchangeRateService.GetExchangeRate(isYen);
            Result = ConversionFactor * InputAmount;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
