using System;
using System.Globalization;

namespace QuickStart
{
    public interface ICustomService
    {
        string GetString();
    }

    public class CustomService : ICustomService
    {
        public string GetString() => DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}
