namespace LangReader
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }
        public bool isSymbol(string j)
        {
            return TokenType == TokenType.Symbol && Value == j;
        }

        public bool isIdentifier(string j)
        {
            return TokenType == TokenType.Identifier && Value == j;
        }
    }
}