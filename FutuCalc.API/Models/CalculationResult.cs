using Newtonsoft.Json;

namespace FutuCalc.API.Models
{
    public class CalculationResult
    {
        [JsonProperty("error")]
        public bool IsError { get; set; }

        [JsonProperty("result")]
        public double Result { get; set; }

        public static CalculationResult Success(double result)
            => new CalculationResult { IsError = false, Result = result };

        public static CalculationResult Error()
            => new CalculationResult { IsError = true, Result = -1 };
    }
}