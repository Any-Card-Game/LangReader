using System;

namespace LangReader
{
    public class SyntaxException : Exception
    {
        private readonly string message;
        private readonly int lineIndex;

        public SyntaxException(string message, int lineIndex)
        {
            this.message = message;
            this.lineIndex = lineIndex;
        }

        public override string ToString()
        {
            return message + " Line:" + lineIndex; ;
        }
    }
}