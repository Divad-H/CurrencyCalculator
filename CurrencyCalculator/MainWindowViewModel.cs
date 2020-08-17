using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CurrencyCalculator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _isReactive = false;
        public bool IsReactive
        {
            get => _isReactive;
            set
            {
                if (value == _isReactive)
                    return;
                _isReactive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsReactive)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReactiveViewModel ReactiveVersion => new ReactiveViewModel();
        public TraditionalViewModel TraditionalVersion => new TraditionalViewModel();
    }
}
