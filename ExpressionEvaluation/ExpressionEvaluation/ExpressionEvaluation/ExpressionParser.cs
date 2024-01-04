namespace ExpressionEvaluation
{
    public class ExpressionParser
    {
        private readonly List<Token> tokens;
        private string currentDigit = "";
        private string currentBool = "";
        private string currentComparison = "";

        public ExpressionParser()
        {
            tokens = new List<Token>();
        }

        public List<Token> ParseExpression(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char character = input[i];

                UpdateTokenTypes(character, ref i, input);

                HandleCurrentToken(character);
            }

            AddRemainingTokens();

            return tokens;
        }

        private void UpdateTokenTypes(char character, ref int index, string input)
        {
            if (Char.IsNumber(character))
            {
                currentDigit += character;
            }
            else if (Char.IsLetter(character))
            {
                UpdateLetterTokenTypes(character, index, input);
            }
            else
            {
                UpdateNonLetterTokenTypes(character, ref index, input);
            }
        }

        private void UpdateLetterTokenTypes(char character, int index, string input)
        {

                currentBool += character;
        }

        private void UpdateNonLetterTokenTypes(char character, ref int index, string input)
        {
            if (IsOperator(character))
            {
                AddRemainingTokens();
                tokens.Add(new Token(TokenType.Operator, character.ToString()));
            }
            else if (IsParenthesis(character))
            {
                AddRemainingTokens();
                tokens.Add(new Token(TokenType.Parenthesis, character.ToString()));
            }
            else if (index < input.Length - 1 && IsComparisonOperator(character.ToString() + input[index + 1]))
            {
                currentComparison += character.ToString() + input[index + 1];
                index++;
            }
            else if (IsComparisonOperator(character.ToString()))
            {
                currentComparison += character.ToString();
            }
        }

        private void HandleCurrentToken(char character)
        {
            if (!Char.IsLetterOrDigit(character))
            {
                AddRemainingTokens();
            }
        }

        private void AddRemainingTokens()
        {
            if (!string.IsNullOrEmpty(currentDigit))
            {
                tokens.Add(new Token(TokenType.Operand, currentDigit));
                currentDigit = "";
            }



            if (!string.IsNullOrEmpty(currentBool))
            {
                if (IsBoolean(currentBool))
                    tokens.Add(new Token(TokenType.Boolean, currentBool));
                else
                    tokens.Add(new Token(TokenType.Operand, currentBool, isVariable: true));

                currentBool = "";
            }

            if (!string.IsNullOrEmpty(currentComparison))
            {
                tokens.Add(new Token(TokenType.Comparison, currentComparison));
                currentComparison = "";
            }
        }

        private bool IsOperator(char character)
        {
            return character == '+' || character == '-' || character == '*' || character == '/' || character == '^';
        }

        private bool IsParenthesis(char character)
        {
            return character == '(' || character == ')';
        }

        private bool IsBoolean(string input)
        {
            return input.ToLower() == "and" || input.ToLower() == "or" || input.ToLower() == "not";
        }

        private bool IsComparisonOperator(string input)
        {
            return input == "<" || input == "<=" || input == ">" || input == ">=" || input == "=" || input == "!=";
        }
    }
}