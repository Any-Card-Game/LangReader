using System.IO;

namespace LangReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var sp = new SymbolParser(new SymbolGenerator(File.ReadAllText(@"B:\Code\LangReader\LangReader\LangReader\Symbols\Reader.sym")));
            SymbolState state = sp.Compute();
            ParserBuilder pb = new ParserBuilder(state);
            string j=pb.Compute();

            File.WriteAllText(@"B:\Code\LangReader\LangReader\LangReader\coz.cs", j);
        }
    }
}
