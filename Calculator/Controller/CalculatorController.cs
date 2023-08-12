using Calculator.CalculatorLogic;
using Microsoft.AspNetCore.Mvc;

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

        public CalculatorController()
        {
            _calculator = new CalculatorClass();
        }

        [HttpGet]
        public async Task<IActionResult> ComputeExpression(string expression)
        {
            int expressionResult;

            try
            {
                expressionResult = _calculator.Compute(expression);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(new
            {
                resultOfExpression = expressionResult
            });

        }
    }
}
