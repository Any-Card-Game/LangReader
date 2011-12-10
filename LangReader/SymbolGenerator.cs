using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangReader
{
    public class SymbolGenerator
    {
        private readonly string content;
        public int index = 0;

        public Action<SymbolGenerator> Skip { get; set; }
        public SymbolGenerator(string content)
        {
            this.content = content;
            Skip = (generator) => { };
        }


        private int lineIndex = 0;
        public char nextChar()
        {
            if (index >= content.Length)
                return '\v';

            var c = content[index++];
            if (c == '\r' && content[index + 1] == '\n')
            {
                lineIndex++;
                return '\n';
            }
            return c;
        }

        
        public char lookAhead(int ind = 0)
        {
            if (index + ind >= content.Length)
                return '\v';
            
            var c= content[index + ind];
            if (c == '\r' && content[index + ind + 1] == '\n')
            { 
                return '\n';
            }
            return c;

        }

        private char[] symbols = new char[] {':', '?', '=', '<', '>', '*', '/', '\\', '!', '@', '#', '$', '%', '^', '&', '(', ')', '-', '+', '-', '{', '[', '}', ']', '|', ';', ',', '.', '~', '`'};
  
        private bool isSymbol(char c)
        {
            return symbols.Contains(c);
        }
        private bool isNumber(char c)
        {
            return Enumerable.Range(0, 10).Select(a => a.ToString()).Contains(c.ToString());
        }
        private bool isWord(char c)
        {
            return Helper.CombineArray(
                Enumerable.Range('a', 'z').Select(a => ((char)a)),
                Enumerable.Range('A', 'Z').Select(a => ((char) a)),
                Enumerable.Range(0, 9).Select(a => char.Parse(a.ToString())),
                Enumerable.Range('_', '_').Select(a => ((char) a))).Contains(c);
        }

      


        public string eatWhile(Func<char, bool> func)
        {
            StringBuilder j = new StringBuilder();
            while (func(lookAhead()))
            {
                j.Append(nextChar());
            }
            return j.ToString();
        } 

        public Token Advance()
        {
            Skip(this);

            if (index == content.Length)
            {
                return new Token(TokenType.EOF, null);
            }
            char nextChar = this.nextChar();
            if (isSymbol(nextChar))
            { 
             
                return new Token(TokenType.Symbol, nextChar.ToString());
            }
            else if (isNumber(nextChar))
            {
                string intConst = nextChar.ToString();
                intConst += eatWhile(isNumber);
                if (lookAhead() == '.')
                {
                    intConst += "." + eatWhile(isNumber);
                }

                int result;
                if (!int.TryParse(intConst, out result))
                {
                    float r2;

                    if (!intConst.Contains("."))
                        throw new SyntaxException("Float const must be in range. Got: " + intConst, lineIndex);
                    if (!float.TryParse(intConst, out r2))
                    {
                        throw new SyntaxException("Int const must be in range [0,2147483648), but got: " + intConst, lineIndex);
                    }
                    return new Token(TokenType.Float, intConst);
                }

                return new Token(TokenType.Int, intConst);
            }

            else if (nextChar == '\"')
            {
                //This token is going to be a string constant.
                string strConst = eatWhile(c => c != '\"');
                this.nextChar();//Swallow the end of the string constant
                return new Token(TokenType.String, strConst);
            }
            else if (isWord(nextChar))
            {

                string keywordOrIdent = nextChar.ToString();
                keywordOrIdent += eatWhile(isWord);


           
                    return new Token(
                        TokenType.Identifier, keywordOrIdent);
               
            }

            throw new SyntaxException("Unexpected character: " + nextChar, lineIndex);

        }

        public void PushState()
        {
            state.Add(index);
        }
        List<int> state=new List<int>(); 
        public void PopState()
        {
            index = state[state.Count - 1];
            state.RemoveAt(state.Count-1);
        }

        public void RestoreState()
        {
            state.RemoveAt(state.Count - 1);//idnex is already set
        }
        public override string ToString()
        {
            int wrap = 40;
            int fc = Math.Max(0, index - wrap);

            string d;

            IEnumerable<int> inds;
            if(state.Count>5)
            {
                inds = state.Skip(state.Count - 5);
            }
            else
            {
                inds = state;
            }

            return string.Format("({0}){1}   |{2}|    {3}", state.Count, 
                inds.Aggregate("", (a, b) => a + " " + b),
                index,
                (d = content.Substring(fc, Math.Min(content.Length, index + wrap) - fc)).Insert(index - fc, "|     ><     |"));

        }
    }
}