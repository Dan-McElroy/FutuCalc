using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var equation = DecodeBase64(encodedEquation);

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

        private static string DecodeBase64(string encoded)
        {
            var decodedBytes = Convert.FromBase64String(encoded);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}