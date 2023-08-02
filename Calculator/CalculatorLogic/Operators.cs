namespace Calculator.CalculatorLogic
{
    public interface IOperators
    {
        int GetResult(int firstValue, int secondValue);
    }

    public class Addition : IOperators
    {

        public int GetResult(int firstValue, int secondValue)
        {
            var result = firstValue + secondValue;
            return result;
        }
    }

    public class Multiplication : IOperators
    {
        public int GetResult(int firstValue, int secondValue)
        {
            var result = firstValue * secondValue;
            return result;
        }
    }
}
