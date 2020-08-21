using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DoubleClick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var clickStream = Observable
                .FromEventPattern<MouseButtonEventHandler, MouseButtonEventArgs>(
                    x => MouseUp += x,
                    x => MouseUp -= x);

            var doubleClickStream = clickStream
                .Buffer(() => clickStream.Throttle(TimeSpan.FromMilliseconds(200), DispatcherScheduler.Current))
                .Select(l => l.Count)
                .Where(c => c >= 2);

            doubleClickStream
                .Scan(0, (acc, _) => ++acc)
                .Subscribe(count => TextBlock.Text = count.ToString());

            InitializeComponent();
        }
    }
}
