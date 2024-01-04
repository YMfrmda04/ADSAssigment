using System;
namespace ExpressionEvaluation
{
    public class ComparisonOperation<T> : IOperation
    {
        public T Execute<T>(double operand1, double operand2, string operation)
        {
            switch (operation)
            {
                case "<":
                    return (T)(object)(operand1 < operand2 ? true : false);
                case "<=":
                    return (T)(object)(operand1 <= operand2 ? true : false);
                case ">":
                    return (T)(object)(operand1 > operand2 ? true : false);
                case ">=":
                    return (T)(object)(operand1 >= operand2 ? true : false);
                case "=":
                    return (T)(object)(operand1 == operand2 ? true : false);
                case "!=":
                    return (T)(object)(operand1 != operand2 ? true : false);
                default:
                    throw new ArgumentException($"Invalid comparison operator: {operation}");
            }
        }
    }
}

