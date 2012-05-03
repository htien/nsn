namespace SaberLily.Text
{
    public enum TokenKind
    {
        Unknown,
        Word,
        Number,
        QuotedString,
        WhiteSpace,
        Symbol,
        EOL,
        EOF
    }

    public class Token
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public string Value { get; private set; }
        public TokenKind Kind { get; private set; }

        public Token(TokenKind kind, string value, int line, int column)
        {
            this.Kind = kind;
            this.Value = value;
            this.Line = line;
            this.Column = column;
        }
    }
}
