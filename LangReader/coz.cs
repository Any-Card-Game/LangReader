
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangReader
{public class SpkeSymb:Symbolz {public bool checkSkip() {  nextPiece0:
symbolGenerator.PushState(); 
  nextPiece2:
symbolGenerator.PushState(); 


                                    if (checkSymbol(" "))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr1;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece3;
                                    }
                  
                                nextPiece3: goto bottomPiece2;
bottomPiece2:
goto nextPiece4;
  nextPiece4:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkNewline())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr1;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece5;
                                    } 
                                }nextPiece5: goto bottomPiece4;
bottomPiece4:
goto curBad1;
curBad1: goto nextPiece1;
endOfOr1:
goto good;
nextPiece6: goto bad;
good:  

                                    symbolGenerator.RestoreState();
                                    return true;bad:  

                                    symbolGenerator.PopState();
                                    return false;}




public bool checkRuleset() {  nextPiece7:
symbolGenerator.PushState(); 
  nextPiece9:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkRule())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr8;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece10;
                                    } 
                                }nextPiece10: goto bottomPiece9;
bottomPiece9:
goto curBad8;
curBad8: goto loopBad7;
endOfOr8:
goto good;
nextPiece11: 
                        symbolGenerator.RestoreState();
                        goto topOfLoop7;
loopBad7: 
                        symbolGenerator.PopState();
                        goto nextPiece8;
nextPiece12: goto bad;
good:  

                                    symbolGenerator.RestoreState();
                                    return true;bad:  

                                    symbolGenerator.PopState();
                                    return false;}




public bool checkRule() {  nextPiece13:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkidentifier())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  good;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece14;
                                    } 
                                }  nextPiece14:
symbolGenerator.PushState(); 


                                    if (checkSymbol(":="))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto good;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece15;
                                    }
                  
                                  nextPiece15:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkRuleExpression())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  good;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece16;
                                    } 
                                }  nextPiece16:
symbolGenerator.PushState(); 


                                    if (checkSymbol(";"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto good;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece17;
                                    }
                  
                                nextPiece17: goto bad;
good:  

                                    symbolGenerator.RestoreState();
                                    return true;bad:  

                                    symbolGenerator.PopState();
                                    return false;}




public bool checkRuleExpression() {  nextPiece18:
symbolGenerator.PushState(); 
  nextPiece20:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkNamePrefix())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr19;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  endOfOr19;
                                    } 
                                }  nextPiece21:
symbolGenerator.PushState(); 
  nextPiece23:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkOrPiece())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr22;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece24;
                                    } 
                                }nextPiece24: goto bottomPiece23;
bottomPiece23:
goto nextPiece25;
  nextPiece25:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkString())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr22;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece26;
                                    } 
                                }nextPiece26: goto bottomPiece25;
bottomPiece25:
goto nextPiece27;
  nextPiece27:
symbolGenerator.PushState(); 
  nextPiece29:
symbolGenerator.PushState(); 
  nextPiece31:
symbolGenerator.PushState(); 


                                    if (checkSymbol("tab"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr30;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece32;
                                    }
                  
                                  nextPiece32:
symbolGenerator.PushState(); 


                                    if (checkSymbol("<"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr30;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece33;
                                    }
                  
                                nextPiece33: goto bottomPiece31;
bottomPiece31:
goto curBad30;
curBad30: goto nextPiece30;
endOfOr30:
goto endOfOr28;
nextPiece34: goto bottomPiece29;
bottomPiece29:
goto curBad28;
curBad28: goto nextPiece28;
endOfOr28:
goto endOfOr22;
nextPiece35: goto bottomPiece27;
bottomPiece27:
goto nextPiece36;
  nextPiece36:
symbolGenerator.PushState(); 
  nextPiece38:
symbolGenerator.PushState(); 
  nextPiece40:
symbolGenerator.PushState(); 


                                    if (checkSymbol("tab"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr39;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece41;
                                    }
                  
                                  nextPiece41:
symbolGenerator.PushState(); 


                                    if (checkSymbol(">"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr39;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece42;
                                    }
                  
                                nextPiece42: goto bottomPiece40;
bottomPiece40:
goto curBad39;
curBad39: goto nextPiece39;
endOfOr39:
goto endOfOr37;
nextPiece43: goto bottomPiece38;
bottomPiece38:
goto curBad37;
curBad37: goto nextPiece37;
endOfOr37:
goto endOfOr22;
nextPiece44: goto bottomPiece36;
bottomPiece36:
goto nextPiece45;
  nextPiece45:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkidentifier())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr22;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece46;
                                    } 
                                }nextPiece46: goto bottomPiece45;
bottomPiece45:
goto curBad22;
curBad22: goto nextPiece22;
endOfOr22:
goto endOfOr19;
  nextPiece47:
symbolGenerator.PushState(); 
  nextPiece49:
symbolGenerator.PushState(); 


                                    if (checkSymbol("?"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr48;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece50;
                                    }
                  
                                nextPiece50: goto bottomPiece49;
bottomPiece49:
goto nextPiece51;
  nextPiece51:
symbolGenerator.PushState(); 


                                    if (checkSymbol("*"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr48;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece52;
                                    }
                  
                                nextPiece52: goto bottomPiece51;
bottomPiece51:
goto curBad48;
curBad48: goto nextPiece48;
endOfOr48:
goto endOfOr19;
nextPiece53: goto bottomPiece20;
bottomPiece20:
goto curBad19;
curBad19: goto loopBad18;
endOfOr19:
goto good;
nextPiece54: 
                        symbolGenerator.RestoreState();
                        goto topOfLoop18;
loopBad18: 
                        symbolGenerator.PopState();
                        goto nextPiece19;
nextPiece55: goto bad;
good:  

                                    symbolGenerator.RestoreState();
                                    return true;bad:  

                                    symbolGenerator.PopState();
                                    return false;}




public bool checkOrPiece() {  nextPiece56:
symbolGenerator.PushState(); 


                                    if (checkSymbol("("))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto good;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece57;
                                    }
                  
                                  nextPiece57:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkRuleExpression1())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  good;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece58;
                                    } 
                                }  nextPiece58:
symbolGenerator.PushState(); 
  nextPiece60:
symbolGenerator.PushState(); 


                                    if (checkSymbol("|"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto endOfOr59;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece61;
                                    }
                  
                                  nextPiece61:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkRuleExpression())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  endOfOr59;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece62;
                                    } 
                                }nextPiece62: goto bottomPiece60;
bottomPiece60:
goto curBad59;
curBad59: goto loopBad58;
endOfOr59:
goto good;
nextPiece63: 
                        symbolGenerator.RestoreState();
                        goto topOfLoop58;
loopBad58: 
                        symbolGenerator.PopState();
                        goto nextPiece59;
  nextPiece64:
symbolGenerator.PushState(); 


                                    if (checkSymbol(")"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto good;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece65;
                                    }
                  
                                nextPiece65: goto bad;
good:  

                                    symbolGenerator.RestoreState();
                                    return true;bad:  

                                    symbolGenerator.PopState();
                                    return false;}




public bool checkNamePrefix() {  nextPiece66:
symbolGenerator.PushState(); 
{
                                    symbolGenerator.PushState();

                                    if (checkidentifier())
                                    {
                                        symbolGenerator.RestoreState();
                                       goto  good;
                                    } else { 
                                        symbolGenerator.PopState();
                                       goto  nextPiece67;
                                    } 
                                }  nextPiece67:
symbolGenerator.PushState(); 


                                    if (checkSymbol("::"))
                                    {
                                        symbolGenerator.RestoreState();
                                        goto good;
                                    } else { 
                                        symbolGenerator.PopState();
                                        goto nextPiece68;
                                    }
                  
                                nextPiece68: goto bad;
good:  

                                    symbolGenerator.RestoreState();
                                    return true;bad:  

                                    symbolGenerator.PopState();
                                    return false;}




public p_Skip obtainSkip() {}




public p_Ruleset obtainRuleset() {}




public p_Rule obtainRule() {}




public p_RuleExpression obtainRuleExpression() {}




public p_OrPiece obtainOrPiece() {}




public p_NamePrefix obtainNamePrefix() {}




public class p_Skip {}




public class p_Ruleset {}




public class p_Rule {}




public class p_RuleExpression {}




public class p_OrPiece {}




public class p_NamePrefix {}




}  }