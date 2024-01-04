using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionEvaluation
{
	public class MANAGER
	{
        VariableManager variables;

        public MANAGER()
		{
           variables = new VariableManager();
        }

        public void printer(string userInput)
        {
            var infixExpression = InfixExpression(userInput);

            GiveValue(infixExpression, variables);

            var postfixExpression = PostfixExpression(infixExpression);
            var result = EvaluationResult(postfixExpression);

            string postfix = "";
            foreach (var item in postfixExpression)
                postfix += item.Value + " ";

            Console.WriteLine("\nArithmetic Postfix Expression: " + postfix);
            Console.WriteLine("Result: " + result); //


        }

        private List<Token> InfixExpression(string expression)
        {
            ExpressionParser expressionParser = new ExpressionParser();
            return expressionParser.ParseExpression(expression);
        }

        private List<Token> PostfixExpression(List<Token> infixExpressionList)
        {
            InfixToPostfixConverter converter = new InfixToPostfixConverter();
            return converter.InfixToPostfix(infixExpressionList);
        }

        private object EvaluationResult(List<Token> postfixExpression)
        {
            PostfixEvaluator evaluator = new PostfixEvaluator(variables);
            return evaluator.EvaluatePostfix(postfixExpression);
        }

        static void GiveValue(List<Token> infixExpressions, VariableManager variables)
        {
            Input input = new Input();
            foreach (var item in infixExpressions)
            {
                if (item.Value.ToLower() == "true" || item.Value.ToLower() == "false")
                {
                    variables.SetVariable(item.Value, item.Value.ToLower() == "true" ? 1 : 0);
                }
                else if (item.Type == TokenType.Operand && item.IsVariable)
                {
                    Console.Write($"Please enter a value for {item.Value}: ");
                    variables.SetVariable(item.Value, input.InputType<double>());
                }
            }
            Console.WriteLine();
        }
    }
}

