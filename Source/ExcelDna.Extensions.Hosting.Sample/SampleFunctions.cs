using System;
using System.Threading.Tasks;
using ExcelDna.Integration;

namespace ExcelDna.Extensions.Hosting.Sample
{
    public class SampleFunctions
    {
        private readonly ICustomService _customService;

        public SampleFunctions(ICustomService customService)
        {
            _customService = customService;
        }

        [ExcelFunction]
        public string GetString()
        {
            return _customService.GetString();
        }

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
