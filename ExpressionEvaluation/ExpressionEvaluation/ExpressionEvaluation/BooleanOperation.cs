using System;
namespace ExpressionEvaluation
{
    public class BooleanOperation<T> : IOperation
    {
        public T Execute<T>(double operand1, double operand2, string operation)
        {
            switch (operation)
            {
                case "and":
                    return (T)(object)(operand1 > 0 && operand2 > 0 ? true : false);
                case "or":
                    return (T)(object)(operand1 > 0 || operand2 > 0 ? true : false);
                case "not":
                    return (T)(object)(operand1 != operand2 ? true : false);
                default:
                    throw new ArgumentException($"Invalid boolean operator: {operation}");
            }
        }
    }
}

