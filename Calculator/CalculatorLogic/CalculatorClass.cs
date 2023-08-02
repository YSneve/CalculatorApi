using System.Text.RegularExpressions;
using Calculator.Controller;
using Microsoft.Extensions.Options;

namespace Calculator.CalculatorLogic
{
    // Класс выполняющий вычисления из строки
    public class CalculatorClass
    {
        // Хранение уже реализованных математических операторов
        private static readonly Dictionary<char, IOperators> Operators = new()
        {
            {'+', new Addition()},
            {'*', new Multiplication()}
        };

        private readonly string _allowedOperators;
        
        public CalculatorClass(string operators)
        {
            _allowedOperators = operators;
        }

        public int Compute(string expression)
        {
            // Разбиение выражения на список строк для упрощенного доступа и работы
            var splitExpression = new Regex($"([{_allowedOperators}])").Split(expression).ToList();

            // Перебор всех доступных операторов и вычисление выражения
            foreach (var symbol in _allowedOperators)
            {
                while (splitExpression.FindIndex(x => x.Equals(symbol.ToString())) != -1)
                {
                    var exprPosition = splitExpression.FindIndex(x => x.Equals(symbol.ToString()));

                    int firstValue = int.Parse(splitExpression[exprPosition - 1]),
                        secondValue = int.Parse(splitExpression[exprPosition + 1]);

                    var result = Operators[symbol].GetResult(firstValue, secondValue);

                    // Замена вычесленных чисел и оператора на их результат
                    splitExpression.RemoveRange(exprPosition - 1, 3);

                    splitExpression.Insert(exprPosition - 1, result.ToString());
                }
            }
            return int.Parse(splitExpression[0]);
        }
    }
}
