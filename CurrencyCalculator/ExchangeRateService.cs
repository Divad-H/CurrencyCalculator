using System;
using System.Threading.Tasks;

namespace CurrencyCalculator
{
    public class ExchangeRateService
    {
        Random _random = new Random();
        public async Task<decimal> GetExchangeRate(bool isYen)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(_random.NextDouble() * 1500));
            return isYen ? 0.0079m : 0.84m;
        }
    }
}
