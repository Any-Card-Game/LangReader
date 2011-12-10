namespace LangReader
{
    public class SymbolState
    {
        public p_Ruleset Ruleset { get; set; }

        public SymbolState(p_Ruleset ruleset)
        {
            Ruleset = ruleset;
        }

        public SymbolState()
        {
            
        }
    }
}