using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangReader
{
    internal class ParserBuilder
    {
        private readonly SymbolState state;
        private StringBuilder checkBuilder = new StringBuilder();
        private StringBuilder obtainBuilder = new StringBuilder();
        private StringBuilder classesBuilder = new StringBuilder();
        public class BuildState
        {
            public int nextGoodState;

            List<bool> notNeeded = new List<bool>();
            List<string> currentName = new List<string>();
            List<string> goodLocation = new List<string>();
            List<string> badLocation = new List<string>();
            List<bool> inLoop = new List<bool>();

            public bool NotNeeded { get { return notNeeded.Last(); } }

            public string CurrentName { get { return currentName.Last(); } }
            public string GoodLocation { get { return goodLocation.Last(); } }
            public string BadLocation(object inde = null) { return string.Format(badLocation.Last(),inde); }



            public bool InLoop { get { return inLoop.Count != 0 && inLoop.Last(); } }

            public void SetCurrentName(string name)
            {
                currentName[currentName.Count - 1] = name;
            }
            public void PushCurrentName(string name)
            {
                currentName.Add(name);
            }

            public void PopCurrentName()
            {
                currentName.RemoveAt(currentName.Count - 1);
            }


            public void SetNotNeeded(bool name)
            {
                notNeeded[notNeeded.Count - 1] = name;
            }
            public void PushNotNeeded(bool name)
            {
                notNeeded.Add(name);
            }

            public void PopNotNeeded()
            {
                notNeeded.RemoveAt(notNeeded.Count - 1);

            }


            public void SetBadLocation(string name)
            {
                badLocation[badLocation.Count - 1] = name;
            }
            public void PushBadLocation(string name)
            {
                badLocation.Add(name);
            }
            public void PopBadLocation()
            {
                badLocation.RemoveAt(badLocation.Count - 1);
            }


            public void SetGoodLocation(string name)
            {
                goodLocation[goodLocation.Count - 1] = name;
            }
            public void PushGoodLocation(string name)
            {
                goodLocation.Add(name);
            }
            public void PopGoodLocation()
            {
                goodLocation.RemoveAt(goodLocation.Count - 1);
            }

            public void SetInLoop(bool name)
            {
                inLoop[inLoop.Count - 1] = name;
            }
            public void PushInLoop(bool name)
            {
                inLoop.Add(name);
            }
            public void PopInLoop()
            {
                inLoop.RemoveAt(inLoop.Count - 1);
            }
        }
        public ParserBuilder(SymbolState state)
        {
            this.state = state;
        }

        public string Compute()
        {
            buildRuleset(new BuildState(), state.Ruleset);

            string prefix = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangReader
{";

            return prefix + "public class SpkeSymb:Symbolz {" + checkBuilder.ToString() + obtainBuilder.ToString() + classesBuilder.ToString() + "}  }";
        }

        private void buildRuleset(BuildState state, p_Ruleset ruleset)
        {
            foreach (var pRule in ruleset.Rules)
            {
                buildRule(state, pRule);
            }
        }

        private void buildRule(BuildState state, p_Rule pRule)
        {
            state.PushBadLocation("bad");
            state.PushGoodLocation("good");
            checkBuilder.AppendFormat("public bool check{0}() {{", pRule.ruleName);
            obtainBuilder.AppendFormat("public p_{0} obtain{0}() {{", pRule.ruleName);
            classesBuilder.AppendFormat("public class p_{0} {{", pRule.ruleName);

            buildRuleExpression(state, pRule.RuleExpression);

            checkBuilder.Append(@"good:  

                                    symbolGenerator.RestoreState();
                                    return true;");
            checkBuilder.Append(@"bad:  

                                    symbolGenerator.PopState();
                                    return false;");


            checkBuilder.AppendLine("}\r\n\r\n\r\n\r\n");
            obtainBuilder.AppendLine("}\r\n\r\n\r\n\r\n");
            classesBuilder.AppendLine("}\r\n\r\n\r\n\r\n");

            state.PopGoodLocation();
            state.PopBadLocation();
        }


        private void buildRuleExpression(BuildState state, p_RuleExpression ruleExpression)
        {
            foreach (var pRuleExpressionRulePiece in ruleExpression.rulePieces)
            {
                var myLoc = state.nextGoodState;
                checkBuilder.AppendLine("  nextPiece" + state.nextGoodState++ + ":");
                checkBuilder.AppendLine("symbolGenerator.PushState(); ");


                state.PushCurrentName(null);
                state.PushNotNeeded(false);

                state.PushBadLocation("nextPiece" + state.nextGoodState);

                if (pRuleExpressionRulePiece.HasNamePrefix)
                {
                    state.SetCurrentName(pRuleExpressionRulePiece.NamePrefix.name);
                }

                switch (pRuleExpressionRulePiece.suffix)
                {
                    case p_RuleExpression_rulePiece_suffix.QuestionMark:
                        state.SetNotNeeded(true);

                        break;
                    case p_RuleExpression_rulePiece_suffix.Multiply: 
                        state.PushInLoop(true);

                        state.PushBadLocation("loopBad" + myLoc);

                        break;
                    default:

                        break;
                }


                switch (pRuleExpressionRulePiece.rule.ruleType)
                {
                    case p_RuleExpression_rulePiece_ruleType.OrPiece:
                        buildOrPiece(state, pRuleExpressionRulePiece.rule.OrPiece);
                        break;
                    case p_RuleExpression_rulePiece_ruleType.String:
                        checkBuilder.AppendFormat(
                            @"

                                    if (checkSymbol(""{0}""))
                                    {{
                                        symbolGenerator.RestoreState();
                                        goto {1};
                                    }} else {{ 
                                        symbolGenerator.PopState();
                                        goto {2};
                                    }}
                  
                                ",
                            pRuleExpressionRulePiece.rule.String,
                            state.GoodLocation,
                            state.NotNeeded ? state.GoodLocation : state.BadLocation(state.nextGoodState));

      

                        break;
                    case p_RuleExpression_rulePiece_ruleType.DecreaseTab:
                        break;
                    case p_RuleExpression_rulePiece_ruleType.IncreaseTab:
                        break;
                    case p_RuleExpression_rulePiece_ruleType.RulePiece:

                        checkBuilder.AppendFormat(
                            @"{{
                                    symbolGenerator.PushState();

                                    if (check{0}())
                                    {{
                                        symbolGenerator.RestoreState();
                                       goto  {1};
                                    }} else {{ 
                                        symbolGenerator.PopState();
                                       goto  {2};
                                    }} 
                                }}",
                            pRuleExpressionRulePiece.rule.String, state.GoodLocation,
                            state.NotNeeded ? state.GoodLocation : state.BadLocation(state.nextGoodState));


                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                if (pRuleExpressionRulePiece.suffix == p_RuleExpression_rulePiece_suffix.Multiply)
                {
                    state.PopBadLocation();

                    checkBuilder.AppendLine(string.Format(@"nextPiece{0}: 
                        symbolGenerator.RestoreState();
                        goto topOfLoop{1};", state.nextGoodState++, myLoc));

                    checkBuilder.AppendLine(string.Format(@"loopBad{0}: 
                        symbolGenerator.PopState();
                        goto {1};", myLoc++, state.BadLocation(state.nextGoodState)));
                     
                    state.PopInLoop();
                }

                state.PopCurrentName();
                state.PopNotNeeded();
                state.PopBadLocation();
            }

           
                checkBuilder.AppendLine("nextPiece" + state.nextGoodState++ + ": goto " + state.BadLocation(state.nextGoodState) + ";");
            
        }

        private void buildOrPiece(BuildState state, p_OrPiece orPiece)
        {
           
            var curState = state.nextGoodState++;

            state.PushGoodLocation("endOfOr" + curState);
            state.PushBadLocation("curBad" + curState);

            var itms = Helper.CombineArray(orPiece.RuleExpression1, orPiece.RuleExpressions).ToArray();
            int ind = 0;
            foreach (p_RuleExpression expression in itms)
            {
                var j = state.nextGoodState;
                state.PushBadLocation(string.Format("bottomPiece{0}", j)); 

                buildRuleExpression(state, expression); 

                state.PopBadLocation();
                checkBuilder.AppendLine(string.Format("bottomPiece{0}:", j));

                if (ind++ == itms.Length-1)
                    checkBuilder.AppendLine("goto " + state.BadLocation() + ";");
                else 
                    checkBuilder.AppendLine("goto nextPiece" + state.nextGoodState + ";");

                
            }
            state.PopBadLocation();
            state.PopGoodLocation();

            checkBuilder.AppendLine("curBad" + curState + ": goto " + state.BadLocation(state.nextGoodState) + ";");

   

            checkBuilder.AppendLine("endOfOr" + curState + ":");
            checkBuilder.AppendLine(string.Format("goto {0};", state.GoodLocation));

        }
    }
}