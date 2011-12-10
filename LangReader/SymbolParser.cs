using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LangReader
{
    public class SymbolParser
    {
        private readonly SymbolGenerator symbolGenerator;

        public SymbolParser(SymbolGenerator symbolGenerator)
        {
            this.symbolGenerator = symbolGenerator;
            symbolGenerator.Skip = Skip;
        }


        public static void Skip(SymbolGenerator generator)
        {
            char f;
            while ((f = generator.lookAhead()) == ' ' || f == '\n')
            {
                generator.index++;
            }
        }
        private bool resetState(Func<bool> func)
        {
            symbolGenerator.PushState();
            var f = func();
            symbolGenerator.PopState();
            return f;
        }



        public SymbolState Compute()
        {
            if (resetState(checkRuleset))
            {
                var ruleset = obtainRuleset();
                Debug.Assert(ruleset != null);
                return new SymbolState(ruleset);
            }
            return new SymbolState();
        }


        public bool checkRuleset()
        {
            symbolGenerator.PushState();
            while (true)
            {
                symbolGenerator.PushState();
                if (checkRule())
                {
                    symbolGenerator.RestoreState();
                    goto nextPiece1;
                }
                else
                {
                    symbolGenerator.PopState();
                    goto loopBad1;
                }

            loopBad1:
                break;
            nextPiece1:
                continue;
            }

        done:

            symbolGenerator.RestoreState();
            return true;
        }

        private bool checkSymbol(string f)
        {
            symbolGenerator.PushState();
            for (int i = 0; i < f.Length; i++)
            {
                if (symbolGenerator.Advance().isSymbol(f[i].ToString()))
                {
                    if (i == f.Length - 1)
                    {
                        symbolGenerator.RestoreState();
                        return true;
                    }
                }

            }
            symbolGenerator.PopState();
            return false;
        }

        private bool checkRule()
        {
            symbolGenerator.PushState();

            if (checkIdentifier())
            {
                goto nextPiece1;
            }
            else goto bad;
        nextPiece1:
            if (checkSymbol(":="))
            {
                goto nextPiece2;
            }
            else goto bad;

        nextPiece2:
            if (checkRuleExpression())
            {
                goto nextPiece3;
            }
            else
                goto bad;
        nextPiece3:
            if (symbolGenerator.Advance().isSymbol(";"))
            {
                goto nextPiece4;
            }
            else goto bad;
        nextPiece4:
            goto good;
        good:

            symbolGenerator.RestoreState();

            return true;

        bad:
            symbolGenerator.PopState();
            return false;
        }




        private bool checkRuleExpression()
        {
            symbolGenerator.PushState();
            int iterations = 0;
        topOfLoop1:
            {
                symbolGenerator.PushState();


                if (checkNamePrefix())
                {
                    goto piece1;
                }
                else goto piece1;

            piece1:
                {
                piece2:
                    {
                        symbolGenerator.PushState();

                        if (checkOrPiece())
                        {
                            symbolGenerator.RestoreState();
                            goto endOfOr;
                        }
                        else
                        {

                            symbolGenerator.PopState();
                            goto piece3;
                        }
                    }
                piece3:
                    if (checkString())
                    {
                        goto endOfOr;
                    }
                    else goto piece4;

                piece4:
                    {
                        symbolGenerator.PushState();

                        if (symbolGenerator.Advance().isIdentifier("tab"))
                        {
                            symbolGenerator.RestoreState();
                            goto piece6;
                        }
                        else
                        {
                            symbolGenerator.PopState();
                            goto piece7;
                        }
                    piece6:
                        if (symbolGenerator.Advance().isIdentifier("<"))
                        {

                            symbolGenerator.RestoreState();
                            goto endOfOr;
                        }
                        else
                        {
                            symbolGenerator.PopState();
                            goto piece7;
                        }
                    }

                piece7:
                    {
                        symbolGenerator.PushState();

                        if (symbolGenerator.Advance().isIdentifier("tab"))
                        {
                            symbolGenerator.RestoreState();
                            goto piece8;
                        }
                        else
                        {
                            symbolGenerator.PopState();
                            goto piece9;
                        }
                    piece8:
                        if (symbolGenerator.Advance().isIdentifier("<"))
                        {
                            symbolGenerator.RestoreState();
                            goto piece9;
                        }
                        else
                        {
                            symbolGenerator.PopState();
                            goto piece9;
                        }
                    }

                piece9:
                    if (checkIdentifier())
                    {
                        goto endOfOr;
                    }
                }

                if (iterations > 0)
                {
                    symbolGenerator.RestoreState();
                    goto good;
                }
                {

                    symbolGenerator.PopState();
                    goto bad;
                }

            endOfOr:

                iterations++;
                {
                    symbolGenerator.PushState();
                    if (symbolGenerator.Advance().isSymbol("?"))
                    {
                        symbolGenerator.RestoreState();
                        goto piece11;
                    }
                    else
                    {
                        symbolGenerator.PopState();
                        goto piece10;
                    }
                }
            piece10:
                {
                    symbolGenerator.PushState();
                    if (symbolGenerator.Advance().isSymbol("*"))
                    {
                        symbolGenerator.RestoreState();
                        goto piece11;
                    }
                    else
                    {
                        symbolGenerator.PopState();
                        goto piece11;
                    }
                }
            piece11:
                symbolGenerator.RestoreState();
                goto topOfLoop1;


            }

        good:
            symbolGenerator.RestoreState();
            return true;
        bad:
            symbolGenerator.PopState();
            return false;
        }

        private bool checkOrPiece()
        {
            symbolGenerator.PushState();
            {
                symbolGenerator.PushState();
                if (symbolGenerator.Advance().isSymbol("("))
                {

                    if (checkRuleExpression())
                    {
                        {
                            while (true)
                            {
                                symbolGenerator.PushState();
                                if (symbolGenerator.Advance().isSymbol("|"))
                                {
                                    if (checkRuleExpression())
                                    {
                                        symbolGenerator.RestoreState();

                                        continue;
                                    }

                                    symbolGenerator.PopState();
                                    break;
                                }
                                symbolGenerator.PopState();
                                break;
                            }


                            if (symbolGenerator.Advance().isSymbol(")"))
                            {
                                symbolGenerator.RestoreState();
                                goto goodDone;
                            }
                        }

                    }


                }
                symbolGenerator.PopState();
            }
        done:
            symbolGenerator.PopState();
            return false;
        goodDone:
            symbolGenerator.RestoreState();
            return true;
        }

        private bool checkNamePrefix()
        {
            symbolGenerator.PushState();
            if (checkIdentifier())
            {
                for (int i = 0; i < "::".Length; i++)
                {
                    if (symbolGenerator.Advance().isSymbol("::"[i].ToString()))
                    {
                        if (i == "::".Length - 1)
                        {

                            symbolGenerator.RestoreState();
                            return true;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }

            symbolGenerator.PopState();
            return false;
        }



        private bool checkString()
        {

            symbolGenerator.PushState();
            if (symbolGenerator.Advance().TokenType == TokenType.String)
            {
                symbolGenerator.RestoreState();
                return true;
            }
            symbolGenerator.PopState();
            return false;
        }

        private bool checkIdentifier()
        {

            symbolGenerator.PushState();

            if (symbolGenerator.Advance().TokenType == TokenType.Identifier)
            {
                symbolGenerator.RestoreState();
                return true;
            }

            symbolGenerator.PopState();
            return false;
        }


        private p_Ruleset obtainRuleset()
        {
            p_Ruleset ruleset = new p_Ruleset();
            ruleset.Rules = new List<p_Rule>();

            while (true)
            {
                if (resetState(checkRule))
                {
                    p_Rule r;
                    ruleset.Rules.Add(r = obtainRule());

                    continue;
                }
                else
                {
                    break;
                }
            }
            return ruleset;
        }

        private p_Rule obtainRule()
        {
            p_Rule rule = new p_Rule();

            rule.ruleName = obtainIdentifier();
            Debug.Assert(symbolGenerator.Advance().isSymbol(":"));
            Debug.Assert(symbolGenerator.Advance().isSymbol("="));

            rule.RuleExpression = obtainRuleExpression();
            Debug.Assert(symbolGenerator.Advance().isSymbol(";"));

            return rule;

        }
        private p_RuleExpression obtainRuleExpression()
        {
            p_RuleExpression ruleExpression = new p_RuleExpression();

            ruleExpression.rulePieces = new List<p_RuleExpression_rulePiece>();
            while (true)
            {

                p_RuleExpression_rulePiece piece = new p_RuleExpression_rulePiece();

                if (piece.HasNamePrefix = resetState(checkNamePrefix))
                {
                    piece.NamePrefix = obtainNamePrefix();
                }

                {

                    if (resetState(checkOrPiece))
                    {
                        piece.rule = new p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType.OrPiece, obtainOrPiece());
                        goto nextPiece1;
                    }
                }
                if (resetState(checkString))
                {
                    piece.rule = new p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType.String, obtainString());
                    goto nextPiece1;
                }


                {
                    symbolGenerator.PushState();
                    if (symbolGenerator.Advance().isIdentifier("tab"))
                    {
                        if (symbolGenerator.Advance().isSymbol("<"))
                        {
                            symbolGenerator.RestoreState();

                            piece.rule = new p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType.DecreaseTab);
                            goto nextPiece1;
                        }
                    }
                    symbolGenerator.PopState();
                }
                {
                    symbolGenerator.PushState();

                    if (symbolGenerator.Advance().isIdentifier("tab"))
                    {
                        if (symbolGenerator.Advance().isSymbol(">"))
                        {
                            symbolGenerator.RestoreState();
                            piece.rule = new p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType.IncreaseTab);
                            goto nextPiece1;
                        }
                    }
                    symbolGenerator.PopState();
                }
                if (resetState(checkIdentifier))
                {
                    piece.rule = new p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType.RulePiece, obtainIdentifier());
                    goto nextPiece1;
                }

                goto goodDone;

            nextPiece1:
                {
                    symbolGenerator.PushState();
                    if (symbolGenerator.Advance().isSymbol("?"))
                    {
                        symbolGenerator.RestoreState();
                        piece.suffix = p_RuleExpression_rulePiece_suffix.QuestionMark;
                    }
                    else
                        symbolGenerator.PopState();
                }
                {
                    symbolGenerator.PushState();
                    if (symbolGenerator.Advance().isSymbol("*"))
                    {
                        symbolGenerator.RestoreState();
                        piece.suffix = p_RuleExpression_rulePiece_suffix.Multiply;
                    }
                    else
                        symbolGenerator.PopState();
                }

                ruleExpression.rulePieces.Add(piece);
                continue;
            }
        goodDone:
            return ruleExpression;

        }

        private string obtainString()
        {

            return symbolGenerator.Advance().Value;
        }

        private p_OrPiece obtainOrPiece()
        {
            p_OrPiece orPiece = new p_OrPiece();
            orPiece.RuleExpressions = new List<p_RuleExpression>();
            {
                Debug.Assert(symbolGenerator.Advance().isSymbol("("));
                orPiece.RuleExpression1 = obtainRuleExpression();

                {
                    while (true)
                    {
                        symbolGenerator.PushState();
                        if (symbolGenerator.Advance().isSymbol("|"))
                        {
                            if (resetState(checkRuleExpression))
                            {
                                symbolGenerator.RestoreState();

                                orPiece.RuleExpressions.Add(obtainRuleExpression());
                                continue;
                            }

                            symbolGenerator.PopState();
                            break;
                        }
                        symbolGenerator.PopState();
                        break;
                    }


                    Debug.Assert(symbolGenerator.Advance().isSymbol(")"));
                    goto goodDone;
                }
            }
        goodDone:
            return orPiece;
        }

        private p_NamePrefix obtainNamePrefix()
        {
            p_NamePrefix namePrefix = new p_NamePrefix();

            namePrefix.name = obtainIdentifier();

            Debug.Assert(symbolGenerator.Advance().isSymbol(":"));
            Debug.Assert(symbolGenerator.Advance().isSymbol(":"));


            return namePrefix;
        }


        private string obtainIdentifier()
        {
            var fc = symbolGenerator.Advance();
            Debug.Assert(fc.TokenType == TokenType.Identifier);
            return fc.Value;
        }




    }

    public class p_RuleExpression_rulePiece_rule
    {

        public p_RuleExpression_rulePiece_ruleType ruleType { get; set; }
        public p_OrPiece OrPiece { get; set; }
        public string String { get; set; }
        public p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType type, p_OrPiece obtainOrPiece)
        {
            ruleType = type;
            OrPiece = obtainOrPiece;
        }
        public p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType type, string str)
        {
            ruleType = type;
            String = str;
        }

        public p_RuleExpression_rulePiece_rule(p_RuleExpression_rulePiece_ruleType ruletype)
        {
            ruleType = ruletype;
        }
    }

    public enum p_RuleExpression_rulePiece_ruleType
    {
        Empty, OrPiece, String, DecreaseTab, IncreaseTab, RulePiece
    }
    public enum p_RuleExpression_rulePiece_suffix
    {
        Empty, QuestionMark, Multiply
    }
    public class p_RuleExpression_rulePiece
    {
        public bool HasNamePrefix { get; set; }
        public p_NamePrefix NamePrefix { get; set; }
        public p_RuleExpression_rulePiece_rule rule { get; set; }

        public p_RuleExpression_rulePiece_suffix suffix { get; set; }
    }

    public class p_Ruleset
    {
        public List<p_Rule> Rules { get; set; }
    }

    public class p_Rule
    {
        public string ruleName { get; set; }
        public p_RuleExpression RuleExpression { get; set; }
    }
    public class p_RuleExpression
    {
        public List<p_RuleExpression_rulePiece> rulePieces { get; set; }
    }
    public class p_OrPiece
    {
        public List<p_RuleExpression> RuleExpressions;
        public p_RuleExpression RuleExpression1;
    }

    public class p_NamePrefix
    {
        public string name;

    }
}