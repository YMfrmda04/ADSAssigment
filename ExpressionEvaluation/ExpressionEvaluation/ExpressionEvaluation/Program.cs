using System;
using System.Linq.Expressions;

namespace ExpressionEvaluation
{
    class Program
    {


        static void Main()
        {
            callReup();
        }

        static void callReup()
        {

            CommandLine cl = new CommandLine();

            cl.Start();

            /*
            reup("5+3*2");      //  11
            reup("(7-4)/2+5");  //  6.5
            reup("8/(4-2)");     // 4
            reup("10-(2+1)*2");  //  4
            reup("3 * 5+(8/2)");   // 19
            reup("-3 * 5+(8/2)");   // -11 

            reup("5 and 0");
            reup("5 or 0");
            reup("5 not 0");

            reup("5 < 0");
            reup("5 <= 0");
            reup("5 > 0");
            reup("5 >= 0");
            reup("5 = 0");
            reup("5 != 0");

            */

            //reup("true and false"); only one not done

        }


        static void reup(string expression)
        {
            VariableManager variables = new VariableManager();


            ExpressionParser expressionParser = new ExpressionParser();
            List<Token> infixExpressions = expressionParser.ParseExpression(expression);

            InfixToPostfixConverter converter = new InfixToPostfixConverter();
            List<Token> postfixExpression = converter.InfixToPostfix(infixExpressions);

            string postfix = "";
            foreach (var item in postfixExpression)
            {
                postfix += item.Value + " ";
            }
            Console.WriteLine("\nArithmetic Postfix Expression: " + postfix);

            PostfixEvaluator evaluator = new PostfixEvaluator(variables);
            var result = evaluator.EvaluatePostfix(postfixExpression);
            Console.WriteLine("Result: " + result); //
        }
    }
}