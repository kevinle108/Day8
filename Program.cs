using System;
using System.Collections.Generic;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                List<string> infix = InfixListFromUser();
                List<string> postfix = PostFixFromInfix(infix);
                double result = ValueOfPostfixList(postfix);
                if (result == -999.999 || result == -999999) // WARNING: Valid computed expressions that actually equal -999999 or -999.999 will print as invalid!  
                {
                    Console.WriteLine("Invalid expression! Please try again.\n");
                }
                else
                    Console.WriteLine($"The answer is {ValueOfPostfixList(postfix)}\n");
            }

            /// FUNCTIONS ///
            static List<string> TokensFromString(string expression)
            {
                List<string> tokensList = new List<string>();
                string token = "";
                bool insideNum = false;
                for (int i = 0; i < expression.Length; i++)
                {
                    char cur = expression[i];
                    if (CharType(cur) == "digit" || CharType(cur) == "decimal")
                    {
                        insideNum = true;
                        token += cur;
                        continue;
                    }
                    else
                    {
                        if (CharType(cur) == "space" && insideNum)
                        {
                            tokensList.Add(token);
                            token = "";
                            insideNum = false;
                            continue;
                        }
                        if (CharType(cur) == "space" && !insideNum) continue;
                        if (insideNum)
                        {
                            tokensList.Add(token);
                            token = "";
                            insideNum = false;
                        }
                        tokensList.Add(cur.ToString());
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
                            bool emptyStack = opStack.Count == 0;
                            while (!emptyStack && opStack.Peek() != openParenthesis)
                            {
                                postfixList.Add(opStack.Pop());
                                if (opStack.Count == 0) emptyStack = true;    
                            }
                            if (emptyStack) return new List<string>();
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

            static double OpResult(double num1, double num2, string op) 
            {
                switch (op)
                {
                    case "^":
                        return Math.Pow(num1, num2);
                    case "*":
                        return num1 * num2;
                    case "/":
                        return num1 / num2;
                    case "%":
                        return num1 % num2;
                    case "+":
                        return num1 + num2;
                    case "-":
                        return num1 - num2;
                    default:
                        return -999999;
                }
            }

            static double ValueOfPostfixList(List<string> postfixList)
            {
                Stack<double> valueStack = new Stack<double>();
                string cur;
                for (int i = 0; i < postfixList.Count; i++)
                {
                    cur = postfixList[i];
                    if (double.TryParse(cur, out double num))
                    {
                        valueStack.Push(num);
                    }
                    else
                    {
                        // cur is an operator
                        if (valueStack.Count < 2) return -999.999;
                        double num2 = valueStack.Pop();
                        double num1 = valueStack.Pop();
                        double result = OpResult(num1, num2, cur);
                        if (result == -999999) return -999999;
                        valueStack.Push(result);
                    }
                }
                if (valueStack.Count != 1) return -999.999;
                return valueStack.Pop();
            }
            // for debugging purposes: 
            // a return of -999.999 means stack error
            // a return of -999999 means OpResult error

            static List<string> InfixListFromUser()
            {
                Console.Write("Enter a mathematical expression: ");
                List<string> infix = TokensFromString(Console.ReadLine());
                return infix;
            }
            
        }
    }
}
