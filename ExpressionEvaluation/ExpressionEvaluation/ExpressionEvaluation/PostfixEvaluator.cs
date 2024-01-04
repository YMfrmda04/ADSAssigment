using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionEvaluation
{
    public class PostfixEvaluator
    {
        private readonly VariableManager variables;
        private readonly Dictionary<TokenType, IOperation> operations;

        public PostfixEvaluator(VariableManager variableManager)
        {
            variables = variableManager;
            operations = new Dictionary<TokenType, IOperation>
            {
                {TokenType.Operator, new NumericOperation<double>()},
                {TokenType.Boolean, new BooleanOperation<bool>()},
                {TokenType.Comparison, new ComparisonOperation<bool>()}
            };
        }

        public object EvaluatePostfix(List<Token> postfix)
        {
            Stack<object> operandStack = new Stack<object>();

            foreach (var token in postfix)
            {
                if (double.TryParse(token.Value, out double operand))
                    operandStack.Push(operand);

                if (token.IsVariable)
                {
                    var tokenValue = variables.GetVariableValue(token.Value);
                    operandStack.Push(tokenValue);
                }
                else if (token.Type == TokenType.Operator || token.Type == TokenType.Boolean || token.Type == TokenType.Comparison)
                {
                    if (operandStack.Count < 2)
                        throw new InvalidOperationException("Not enough operands for the operator.");

                    var operand2 = operandStack.Pop();
                    var operand1 = operandStack.Pop();

                    object result = EvaluateType<object>(token, operand1, operand2);

                    if (result is bool)
                        operandStack.Push(_ = (bool)result == true ? 1.0 : 0.0);
                    else
                        operandStack.Push(result);
                }
            }

            return operandStack.Pop();
        }

        T EvaluateType<T>(Token token, object operand1, object operand2)
        {
            return operations[token.Type].Execute<T>((double)operand1, (double)operand2, token.Value);
        }
    }
}
