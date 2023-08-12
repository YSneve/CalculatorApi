using Calculator.CalculatorLogic;
using FluentAssertions;

namespace CalculatorUnitTests;

public class CalculatorClassTests
{
    private CalculatorClass _calculatorClass = null!;

    [SetUp]
    public void Setup()
    {
        // Максимально непонятно как задать список разрешенных операций,
        const string operators = "+|*";
        _calculatorClass = new CalculatorClass(operators);
    }

    /// <summary>
    ///     Тест работы калькулятора
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="expectedResult"></param>
    [TestCase("2+2", 4)]
    [TestCase(" 3 + 3 ", 6)]
    [TestCase("2+2+2", 6)]
    [TestCase("-1+2", 1)]
    [TestCase("-5+2", -3)]
    [TestCase("2*2", 4)]
    [TestCase("2*2*2", 8)]
    [TestCase("2+2*2",6)]
    [TestCase("(2+2)*2",8)]
    public void TestCompute(string expression, int expectedResult)
    {
        _calculatorClass.Compute(expression).Should().Be(expectedResult);
    }
}