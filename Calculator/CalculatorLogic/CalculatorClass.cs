using System.Text.RegularExpressions;


namespace Calculator.CalculatorLogic
{
    // Класс выполняющий вычисления из строки
    public class CalculatorClass
    {
        private static readonly Regex BracketRegex = new("-\\([0-9+\\-*^]*\\)|\\([0-9+\\-*^]*\\)");
        private static readonly Regex SplitRegex = new("([*+\\-()^])");

        // Порядок выполнения и реализация математических выражений
        private static readonly Dictionary<string, Func<int, int, int>> Operators = new()
    {
        {"^", (a, b) => (int)Math.Pow(a, b)}, // Выскоий приоритет
        {"/", (a, b) => a / b}, 
        {"*", (a, b) => a * b},
        {"+", (a, b) => a + b},
        {"-", (a, b) => a - b} // Низкий приоритет

    };
        public int Compute(string expression)
        {
            while (BracketRegex.IsMatch(expression))
            {
                var match = BracketRegex.Match(expression).Value;

                var computeResult = ComputeStringList(SplitRegex          // Вычисляем список токенов
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
            var result = ComputeStringList(SplitRegex.Split(expression).ToList().FindAll(e => e != "" && e != " "));

            return result;
        }

        private int ComputeStringList(List<string> exprList)
        {
            if (exprList.Count == 2)
                return int.Parse(exprList[0] + exprList[1]);

            foreach (var _operator in Operators.Keys)
            {

                while (exprList.FindIndex(x => x == _operator) != -1)
                {
                    var operatorPos = exprList.FindIndex(x => x == _operator);

                    if (operatorPos > 1 && exprList[operatorPos - 2] == "-") // Замена двух токенов, числа и минуса, на один токен отрицательного числа
                    {
                        exprList[operatorPos - 1] = "-" + exprList[operatorPos - 1];
                        exprList.RemoveAt(operatorPos - 2);
                        operatorPos--;
                    }

                    int firstValue = int.Parse(exprList[operatorPos - 1]),
                        secondValue = int.Parse(exprList[operatorPos + 1]);

                    var result = Operators[_operator](firstValue, secondValue);

                    exprList.RemoveRange(operatorPos - 1, 3);

                    exprList.Insert(operatorPos - 1, result.ToString());
                }
            }

            return int.Parse(exprList[0]);
        }
    }
}
