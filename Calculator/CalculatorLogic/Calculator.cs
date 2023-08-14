using System.Text.RegularExpressions;


namespace Calculator.CalculatorLogic
{
    // Класс выполняющий вычисления из строки
    public class Calculator
    {
        

        // Порядок выполнения и реализация математических выражений
        private static readonly Dictionary<string, Func<int, int, int>> Operators = new()
    {
        {"^", (a, b) => (int)Math.Pow(a, b)}, // Выскоий приоритет
        {"/", (a, b) => a / b}, 
        {"*", (a, b) => a * b},
        {"+", (a, b) => a + b},
        {"-", (a, b) => a - b} // Низкий приоритет

    };
        private static readonly Regex BracketExpressionRegex = new($"-?\\([0-9{string.Join("\\", Operators.Keys)}]*\\)");
        private static readonly Regex TokenRegex = new($"([\\{string.Join("\\", Operators.Keys)}()])");
        public int Compute(string expression)
        {
            while (BracketExpressionRegex.IsMatch(expression))
            {
                var match = BracketExpressionRegex.Match(expression).Value;

                var computeResult = ComputeList(TokenRegex          // Вычисляем список токенов
                        .Split(match[0] == '-' ? match[1..] : match)         // Делим выражение на array токенов, убираем минус перед скобками, если есть
                        .ToList()                                            // Конвертация в List<string> 
                        .FindAll(e =>                                 // Удаление скобок и пробелов
                            e != "("
                            && e != ")"
                            && e != " "
                            && e != ""));

                computeResult *= match[0] == '-' ? -1 : 1; // Если перед скобками был минус, то результат в скобках берём отрицательным

                expression = expression.Replace(match, computeResult.ToString()); // Заменяем выражение со скобками на результат в скобках

            }

            // После решения всех скобок, решаем полученное выражение
            var result = ComputeList(TokenRegex.Split(expression).ToList().FindAll(e => e != "" && e != " "));

            return result;
        }

        private static int ComputeList(List<string> TokenList)
        {
            if (TokenList.Count == 2)
                return int.Parse(TokenList[0] + TokenList[1]);

            foreach (var _operator in Operators.Keys)
            {

                while (TokenList.FindIndex(x => x == _operator) != -1)
                {
                    var operatorPos = TokenList.FindIndex(x => x == _operator);

                    if (operatorPos > 1 && TokenList[operatorPos - 2] == "-") // Замена двух токенов, числа и минуса, на один токен отрицательного числа
                    {
                        TokenList[operatorPos - 1] = "-" + TokenList[operatorPos - 1];
                        TokenList.RemoveAt(operatorPos - 2);
                        operatorPos--;
                    }

                    int firstValue = int.Parse(TokenList[operatorPos - 1]),
                        secondValue = int.Parse(TokenList[operatorPos + 1]);

                    var result = Operators[_operator](firstValue, secondValue);

                    TokenList.RemoveRange(operatorPos - 1, 3);

                    TokenList.Insert(operatorPos - 1, result.ToString());
                }
            }

            return int.Parse(TokenList[0]);
        }
    }
}
