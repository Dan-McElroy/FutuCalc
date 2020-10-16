using FutuCalc.API.Extensions;
using FutuCalc.API.Models;
using FutuCalc.Core.Calculation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FutuCalc.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly ILogger<CalculationController> _logger;
        private readonly ICalculator _calculator;

        public CalculationController(ILogger<CalculationController> logger, ICalculator calculator)
        {
            _logger = logger;
            _calculator = calculator;
        }

        [HttpGet]
        [Route("{encodedEquation}")]
        public CalculationResult Get(string encodedEquation)
        {
            var paddedEquation = encodedEquation.PadForBase64();
            if (!paddedEquation.IsValidBase64())
            {
                return CalculationResult.Error();
            }
            var equation = paddedEquation.DecodeBase64();

            try
            {
                var result = _calculator.Calculate(equation);
                return CalculationResult.Success(result);
            }
            catch
            {
                return CalculationResult.Error();
            }
        }
    }
}