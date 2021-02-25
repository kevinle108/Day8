using System;
using System.Collections.Generic;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            while (true) 
            { 
                Console.Write("Enter a mathematical expression (or 0 to quit): ");
                input = Console.ReadLine();
                if (input == "0") break;
                List<string> infix = TokensFromString(input);
                List<string> postfix = PostFixFromInfix(infix);
                Console.WriteLine($"The answer is {ValueOfPostfixList(postfix)}\n");
            }
            Console.WriteLine("Program finished...");

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
                        valueStack.Push(OpResult(num1, num2, cur));
                    }
                }
                if (valueStack.Count != 1) return -999.999;
                return valueStack.Pop();
            }
        }
    }
}
