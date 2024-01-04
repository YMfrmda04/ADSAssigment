using System;
namespace ExpressionEvaluation
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public bool IsVariable { get; set; }


        public Token(TokenType type, string value, bool isVariable = false)
        {
            Type = type;
            Value = value;
            IsVariable = isVariable;
        }
    }
}

