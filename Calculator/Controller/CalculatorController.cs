using System.Data;
using System.Text.RegularExpressions;
using Calculator.CalculatorLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Calculator.Controller
{
    public class CalculatorOptions
    {
        public string AllowedOperators { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly CalculatorClass _calculator;
        public CalculatorController(IOptions<CalculatorOptions> options)
        {
            _calculator = new CalculatorClass(options.Value.AllowedOperators);
        }

        [HttpGet]
        public async Task<IActionResult> ComputeExpression(string expression)
        {
            var expressionResult = _calculator.Compute(expression);

            return Ok(new
            {
                resultOfExpression = expressionResult
            });
        }
    }
}
