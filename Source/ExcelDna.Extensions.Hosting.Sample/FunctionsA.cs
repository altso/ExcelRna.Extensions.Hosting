using ExcelDna.Integration;

namespace ExcelDna.Extensions.Hosting.Sample
{
    public class FunctionsA
    {
        private readonly ICustomService _customService;

        public FunctionsA(ICustomService customService)
        {
            _customService = customService;
        }

        [ExcelFunction]
        public string GetString()
        {
            return _customService.GetString();
        }
    }
}
