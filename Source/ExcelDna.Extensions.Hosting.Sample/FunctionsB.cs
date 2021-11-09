using System;
using System.Threading.Tasks;
using ExcelDna.Integration;

namespace ExcelDna.Extensions.Hosting.Sample
{
    public class FunctionsB
    {
        [ExcelFunction]
        public int SampleInt32(int value)
        {
            return value * value;
        }

        [ExcelFunction]
        public async Task<string> SampleAsyncString(string s)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return $"{s} after 1s";
        }
    }
}
