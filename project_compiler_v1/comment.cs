using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_compiler_v1
{
    public class comment
    {
        private bool MultiLineActivated = false;/*case: <*>hello */
        private bool checkMultiLineStartComment(char one, char two)
        {
            if (one == '<' && two == '*')
            {
                MultiLineActivated = true;
                return true;
            }
            return false;
        }
        private bool checkMultiLineEndComment(char one, char two)
        {
            if (one == '*' && two == '>')
            {
                MultiLineActivated = false;
                return true;
            }
            return false;
        }
        private bool checkSingleLineComment(char one, char two)
        {
            if (one == '/' && two == '/') return true;
            return false;
        }
        private string FindCommentsInLine(string codeString)
        {
            string Str = "";
            string result = Str.Insert(0, ""); //will be returned
            if (codeString.Length == 1) /*example case :  'h\n' */
                return Str.Insert(Str.Length, result + codeString[0].ToString());
            else if (codeString.Length <= 0)
                return "";
            else
            {
                for (int i = 1; i < codeString.Length; i++)
                {
                    if (MultiLineActivated == true)
                    {/*CONTINUEING A MULTI LINE COMMENT*/
                        while (i < codeString.Length && !checkMultiLineEndComment(codeString[i - 1], codeString[i])) i++;
                        if (i + 1 < codeString.Length) i++;/*case : <*Hello
                                                  World*>*/
                    }
                    else
                    {
                        if (checkMultiLineStartComment(codeString[i - 1], codeString[i]))
                        {/*MULTI LINE BEGINNING FOUND*/
                            if (i + 2 > codeString.Length) i++;/*case: <*>*/
                            else i += 2;/*case : <*  *>  */
                            while (i < codeString.Length && !checkMultiLineEndComment(codeString[i - 1], codeString[i])) i++;
                            i++;
                            if (!MultiLineActivated && i + 1 == codeString.Length && !checkMultiLineEndComment(codeString[i - 1], codeString[i]))/*case : <**>hello*/
                                result = Str.Insert(Str.Length, result + codeString[i].ToString());
                            result = Str.Insert(Str.Length, result + " ");
                        }
                        else if (checkSingleLineComment(codeString[i - 1], codeString[i]))/*case://hello */
                        {/*SINGLE LINE FOUND*/
                            result = Str.Insert(Str.Length, result + " ");
                            break;
                        }
                        else
                        {/*NORMAL LETTER*/
                            result = Str.Insert(Str.Length, result + codeString[i - 1].ToString());/*case:hello*/
                            if (i + 1 == codeString.Length && !checkMultiLineEndComment(codeString[i - 1], codeString[i]))
                                result = Str.Insert(Str.Length, result + codeString[i].ToString());
                        }
                    }
                }
            }
            return result;
        }
        public List<string> IgnoreComments(string[] codeStringArray)
        {
            List<string> ExcludedCommentCode = new List<string>();
            string temp = "";
            string tempInserter = temp.Insert(0, "");
            foreach (string AString in codeStringArray)
            {
                string x = AString;
                tempInserter = temp.Insert(temp.Length, FindCommentsInLine(AString));/*SEARCH FOR A COMMENT*/
                if (tempInserter != "")
                    ExcludedCommentCode.Add(tempInserter);
            }
            return ExcludedCommentCode;
        }
    }
}
