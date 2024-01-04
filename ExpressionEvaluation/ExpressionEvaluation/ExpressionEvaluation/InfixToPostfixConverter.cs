using System;
namespace ExpressionEvaluation
{
    public class InfixToPostfixConverter
    {
        private static int GetPrecedence(char op)
        {
            switch (op)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                default:
                    return 0;
            }
        }

        private static void ProcessOperand(List<Token> tokens, ref int i, ref List<Token> postfix)
        {
            while (i < tokens.Count && tokens[i].Type == TokenType.Operand)
            {
                if (i > 0 && IsUnaryOperator(tokens[i - 1].Value, tokens, i - 1))
                {
                    string operand = tokens[i - 1].Value.ToString();
                    operand += tokens[i].Value;

                    postfix.Add(new Token(tokens[i].Type, operand, tokens[i].IsVariable));
                }
                else
                {
                    postfix.Add(new Token(tokens[i].Type, tokens[i].Value, tokens[i].IsVariable));
                }

                i++;
            }
            i--;
        }

        private static void ProcessOpeningParenthesis(Stack<Token> operators, Token token)
        {
            operators.Push(token);
        }

        private static void ProcessClosingParenthesis(Stack<Token> operators, ref List<Token> postfix)
        {
            while (operators.Count > 0 && operators.Peek().Value != "(")
            {
                postfix.Add(new Token(operators.Peek().Type, operators.Peek().Value, operators.Pop().IsVariable));
            }
            operators.Pop();
        }

        private static void ProcessOperator(Token token, Stack<Token> operators, ref List<Token> postfix)
        {
            while (operators.Count > 0 && GetPrecedence(Char.Parse(operators.Peek().Value)) >= GetPrecedence(Char.Parse(token.Value)))
            {
                postfix.Add(new Token(operators.Peek().Type, operators.Peek().Value, operators.Pop().IsVariable));
            }
            operators.Push(token);
        }

        private static bool IsUnaryOperator(string token, List<Token> tokens, int i)
        {
            if ((token == "-" || token == "+") && (i == 0 || tokens[i - 1].Value == "("))
            {
                return true;
            }
            return false;
        }


        public List<Token> InfixToPostfix(List<Token> tokens)
        {
            List<Token> postfix = new List<Token>();
            Stack<Token> operators = new Stack<Token>();


            for (int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];

                if (IsUnaryOperator(token.Value, tokens, i))
                    operators.Push(token);

                else if (token.Type == TokenType.Operand)
                {
                    if (operators.Count > 0 && i > 0 && IsUnaryOperator(operators.Peek().Value.ToString(), tokens, i - 1))
                        operators.Pop();

                    ProcessOperand(tokens, ref i, ref postfix);
                }

                else if (token.Value == "(")
                {
                    ProcessOpeningParenthesis(operators, token);
                }

                else if (token.Value == ")")
                {
                    ProcessClosingParenthesis(operators, ref postfix);
                }

                else if (token.Type == TokenType.Operator)
                    ProcessOperator(token, operators, ref postfix);

                else if (token.Type == TokenType.Boolean
                    || token.Type == TokenType.Comparison)
                {
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                postfix.Add(new Token(operators.Peek().Type, operators.Peek().Value, operators.Peek().IsVariable));
                operators.Pop();
            }

            return postfix;
        }
    }
}

