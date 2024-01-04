using System;
namespace ExpressionEvaluation
{
    public interface IOperation
    {
        T Execute<T>(double operand1, double operand2, string operation);
    }
}

