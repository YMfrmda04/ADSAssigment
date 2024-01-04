using System;
namespace ExpressionEvaluation
{
    public class NumericOperation<T> : IOperation
    {
        public T Execute<T>(double operand1, double operand2, string operation)
        {
            switch (operation)
            {
                case "+":
                    return (T)(object)(operand1 + operand2);
                case "-":
                    return (T)(object)(operand1 - operand2);
                case "*":
                    return (T)(object)(operand1 * operand2);
                case "/":
                    if (operand2 == 0)
                        throw new DivideByZeroException("Cannot divide by zero.");
                    return (T)(object)(operand1 / operand2);
                case "^":
                    return (T)(object)Math.Pow(operand1, operand2);
                default:
                    throw new ArgumentException($"Invalid operator: {operation}");
            }
        }
    }
}

