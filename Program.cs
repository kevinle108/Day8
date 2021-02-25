using System;
using System.Collections;
using System.Collections.Generic;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            //string expression = "32 + 5.2 * ((4.6 ^ 2 – 20 / 3)) – 4 * 2";
            //string expression = "3 + 6 - 2";
            //string expression = "3 + 6 * 2";
            //string expression = "3 + 6 * 2 / 4 - 2";
            //string expression = "3 + 6 ^ 2 / ((4 - 2) * 2)";
            //List<string> tokens = TokensFromString(expression);

            Console.Write("Enter a mathematical expression: ");
            List<string> tokens = TokensFromString(Console.ReadLine());
            //OutputTokens(tokens);
            List<string> postfix = PostFixFromInfix(tokens);
            OutputTokens(postfix);


            /// FUNCTIONS ///
            static List<string> TokensFromString(string expression)
            {
                List<string> tokensList = new List<string>();
                string token = "";
                bool insideNum = false;
                for (int i = 0; i < expression.Length; i++)
                {
                    char c = expression[i];
                    if (CharType(expression[i]) == "digit" || CharType(expression[i]) == "decimal")
                    {
                        insideNum = true;
                        token += expression[i];
                        continue;
                    }
                    else
                    {
                        if (CharType(expression[i]) == "space") continue;
                        if (insideNum)
                        {
                            tokensList.Add(token);
                            token = "";
                            insideNum = false;
                        }
                        tokensList.Add(expression[i].ToString());
                    }
                }
                if (token != "")
                {
                    tokensList.Add(token);
                }
                return tokensList;
            }

            static string CharType(char c)
            {
                if (c == ' ') return "space";
                else if (c == '+' || c == '-' || c == '–' || c == '*' || c == '/' || c == '^' || c == '%') return "operand";
                else if (c == '(' || c == ')' || c == '{' || c == '}' || c == '[' || c == ']') return "parenthesis";
                else if (c == '.') return "decimal";
                else return "digit";
            }

            static void OutputTokens(List<string> tokens)
            {
                tokens.ForEach(token => Console.WriteLine(token));
            }

            static List<string> PostFixFromInfix(List<string> infixList)
            {
                List<string> postfixList = new List<string>();
                Stack<string> opStack = new Stack<string>();
                for (int i = 0; i < infixList.Count; i++)
                {
                    string cur = infixList[i];
                    if (double.TryParse(cur, out double token))
                    {
                        postfixList.Add(cur);
                    }
                    else
                    {
                        if (cur == "(" || cur == "[" || cur == "{")
                        {
                            opStack.Push(cur);
                            continue;
                        }
                        if (cur == ")" || cur == "}" || cur == "]")
                        {
                            string openParenthesis = matchingParenthesis(cur);
                            while (opStack.Peek() != openParenthesis)
                            {
                                postfixList.Add(opStack.Pop());
                            }
                            opStack.Pop(); // to remove the open parenthesis from the stack
                            continue;
                        }
                        while (PrevOpShouldBeExecutedBeforeCurOp(opStack, cur))
                        {
                            postfixList.Add(opStack.Pop());
                        }
                        opStack.Push(cur);
                    }
                }
                while (opStack.Count != 0)
                {
                    postfixList.Add(opStack.Pop());
                }
                return postfixList;
            }

            static bool PrevOpShouldBeExecutedBeforeCurOp(Stack<string> opStack, string curOp)
            {
                if (opStack.Count < 1) return false;
                string prevOp = opStack.Peek();
                return PriorityValue(prevOp) >= PriorityValue(curOp);
            }

            static int PriorityValue(string op)
            {
                switch (op)
                {
                    //case "(":
                    //case "[":
                    //case "{":
                    //    return 4;

                    case "^": return 3;
                        
                    case "*":
                    case "/":
                    case "%":
                        return 2;

                    case "+":
                    case "-":
                        return 1;

                    default: // assumes that op is a (, [, or {
                        return 0;
                }
            }
            
            static string matchingParenthesis(string parenthesis)
            {
                switch (parenthesis)
                {
                    case ")": return "(";
                    case "]": return "[";
                    case "}": return "{";
                    default:
                        return "";
                }
            }
        }
    }
}
