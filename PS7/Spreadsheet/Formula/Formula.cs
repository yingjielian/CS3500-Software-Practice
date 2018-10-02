// Skeleton written by Joe Zachary for CS 3500, January 2017
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/// <summary>
/// Yingjie Lian
/// U1058784
/// </summary>
namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        // Initializing a list to hold the formula
        private List<String> tokens;

        // variable to keep track of normalized variables
        private HashSet<string> normalized_vars;

        /// <summary>
        /// Convert the Formula class into a Formula struct.  A zero argument constructor will be supplied by 
        /// the compiler.  The Formula created by this constructor.
        /// </summary>
        public Formula(String formula)
             : this(formula, s => s, s => true)
        {

        }


        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(string s, Normalizer normalizer, Validator validator)
        {
            if (s == null || normalizer == null || validator == null)
                throw new ArgumentNullException("Paramaters cannot be null!");

            // Initializing needed variables
            int openParen = 0;
            int closeParen = 0;
            double checkDigit = 0.0;
            this.tokens = new List<string>();
            // keep track of normalized variables
            String nomalizedVars = null;
            normalized_vars = new HashSet<string>();

            string previousToken = null;

            Boolean firstTimeLoop = true;

            foreach (string token in GetTokens(s))
            {

                //There must be at least one token.
                if (token == null)
                    throw new FormulaFormatException("There must be at least one token.");



                //When reading tokens from left to right, at no point should the number of closing parentheses seen so far be greater than the number of opening parentheses seen so far.
                if (token == "(")
                    openParen++;
                else if (token == ")")
                    closeParen++;

                //The first token of a formula must be a number, a variable, or an opening parenthesis.
                if (firstTimeLoop)
                {
                    if (!Regex.IsMatch(token, @"^[a-zA-Z][0-9a-zA-Z]*$") && token != "(" && !Double.TryParse(token, out checkDigit))
                    {
                        throw new FormulaFormatException("Error, formula doesn't begin appropriately.");
                    }
                }

                else
                {
                    // Any token that immediately follows an opening parenthesis or an operator must 
                    // be either a number, a variable, or an opening parenthesis.
                    if (previousToken == "(" || Regex.IsMatch(previousToken, @"^[\+\-*/]$"))
                        if (!(Regex.IsMatch(token, @"^[a-zA-Z][0-9a-zA-Z]*$") || (token == "(") || Double.TryParse(token, out checkDigit)))
                            throw new FormulaFormatException("error: unexpected character after an opening parenthesis or operator.");

                    // Any token that immediately follows a number, a variable, or a closing parenthesis must
                    // be either an operator or a closing parenthesis.
                    if (Regex.IsMatch(previousToken, @"^[a-zA-Z][0-9a-zA-Z]*$") || previousToken == ")" || Double.TryParse(previousToken, out checkDigit))
                        if (!(Regex.IsMatch(token, @"^[\+\-*/]$") || token == ")"))
                            throw new FormulaFormatException("error: unexpected character after a number, a variable, or a closing parenthesis.");
                }

                // If we get all the way through without an exception, it should be a valid formula
                // then we want to normalize the formula and validate it according to the delegate parameters
                // Check if token is a variable
                if (Regex.IsMatch(token, @"^[a-zA-Z][0-9a-zA-Z]*$"))
                {
                    // if the normalized variable is not valid, throw and exception
                    if (!validator(normalizer(token)))
                        throw new FormulaFormatException("A normalized variable does not have a valid variable format!");
                    else // Otherwise normalize it and then add to the list
                    {
                        nomalizedVars = normalizer(token);
                        tokens.Add(nomalizedVars);
                        normalized_vars.Add(nomalizedVars);
                    }
                }
                else
                {
                    tokens.Add(token);
                }

                // Hold to check the last token
                previousToken = token;

                // Set boolean flag to decide whether it is the first loop
                firstTimeLoop = false;
            }

            //If it is all spaces, then the size of list should be 0.
            if (tokens.Count == 0)
                throw new FormulaFormatException("No tokens.");

            //The last token of a formula must be a number, a variable, or a closing parenthesis.
            if (!Regex.IsMatch(previousToken, @"^[a-zA-Z][0-9a-zA-Z]*$") && previousToken != ")" && !Double.TryParse(previousToken, out checkDigit))
                throw new FormulaFormatException("Error, formula doesn't end appropriately.");

            //The total number of opening parentheses must equal the total number of closing parentheses.
            if (closeParen != openParen)
                throw new FormulaFormatException("The total number of opening parentheses must equal the total number of closing parentheses.");



        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {

            if (lookup == null)
                throw new ArgumentNullException("Paramaters cannot be null!");

            // Initializing needed variables
            Stack<string> operatorStack = new Stack<string>();
            Stack<string> valueStack = new Stack<string>();

            double tokenNumber = 0.0;
            string previousToken = null;

            foreach (string token in tokens)
            {
                if (Double.TryParse(token, out tokenNumber) || Regex.IsMatch(token, @"^[a-zA-Z][0-9a-zA-Z]*$"))
                {
                    if (!Double.TryParse(token, out tokenNumber))
                    {
                        try
                        {
                            double varabileValue = lookup(token);
                            tokenNumber = varabileValue;
                        }
                        catch (UndefinedVariableException e)
                        {
                            throw new FormulaEvaluationException("This is undefined.");
                        }
                    }
                    // If* or / is at the top of the operator stack, pop the value stack, 
                    // pop the operator stack, and apply the popped operator to t and the 
                    // popped number. Push the result onto the value stack.Otherwise, push 
                    // t onto the value stack
                    if ((operatorStack.Count != 0) && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        string operators = operatorStack.Pop();
                        double poppedValue = 0.0;
                        Double.TryParse(valueStack.Pop(), out poppedValue);

                        if (operators == "*")
                            valueStack.Push((poppedValue * tokenNumber).ToString());
                        else if (operators == "/")
                        {
                            if (tokenNumber == 0)
                                throw new FormulaEvaluationException("Should not be 0.");
                            else
                                // Need a check
                                valueStack.Push((poppedValue / tokenNumber).ToString());
                        }

                    }
                    else
                    {
                        valueStack.Push(tokenNumber.ToString());
                    }
                }

                if (token == "+" || token == "-")
                {
                    // If + or - is at the top of the operator stack, 
                    // pop the value stack twice and the operator stack 
                    // once.  Apply the popped operator to the popped numbers.
                    // Push the result onto the value stack.Whether or not you 
                    // did the first step, push t onto the operator stack
                    if ((operatorStack.Count != 0) && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        string operators = operatorStack.Pop();
                        double firstPoppedValue = 0.0;
                        double secondPoppedValue = 0.0;
                        Double.TryParse(valueStack.Pop(), out firstPoppedValue);
                        Double.TryParse(valueStack.Pop(), out secondPoppedValue);

                        if (operators == "+")
                            valueStack.Push((firstPoppedValue + secondPoppedValue).ToString());
                        else if (operators == "-")
                            valueStack.Push((secondPoppedValue - firstPoppedValue).ToString());
                    }

                    operatorStack.Push(token);
                }

                if (token == "*" || token == "/")
                {
                    operatorStack.Push(token);
                }

                if (token == "(")
                {
                    operatorStack.Push(token);
                }

                if (token == ")")
                {
                    // If + or - is at the top of the operator stack, pop the value 
                    // stack twice and the operator stack once. Apply the popped 
                    // operator to the popped numbers. Push the result onto the value 
                    // stack. 
                    // Whether or not you did the first step, the top of the operator 
                    // stack will be a (. Pop it.
                    // After you have completed the previous step, if *or / is at the
                    // top of the operator stack, pop the value stack twice and the 
                    // operator stack once. Apply the popped operator to the popped numbers. 
                    // Push the result onto the value stack.
                    if ((valueStack.Count > 1) && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        string operators = operatorStack.Pop();
                        double firstPoppedValue = 0.0;
                        double secondPoppedValue = 0.0;
                        Double.TryParse(valueStack.Pop(), out firstPoppedValue);
                        Double.TryParse(valueStack.Pop(), out secondPoppedValue);

                        if (operators == "+")
                            valueStack.Push((firstPoppedValue + secondPoppedValue).ToString());
                        else if (operators == "-")
                            valueStack.Push((secondPoppedValue - firstPoppedValue).ToString());
                    }


                    operatorStack.Pop();

                    if ((valueStack.Count > 1) && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        string operators = operatorStack.Pop();
                        double firstPoppedValue = 0.0;
                        double secondPoppedValue = 0.0;
                        Double.TryParse(valueStack.Pop(), out firstPoppedValue);
                        Double.TryParse(valueStack.Pop(), out secondPoppedValue);

                        if (operators == "*")
                            valueStack.Push((firstPoppedValue * secondPoppedValue).ToString());
                        else if (operators == "/")
                        {
                            if (firstPoppedValue == 0)
                                throw new FormulaEvaluationException("Should not be 0.");
                            else
                                valueStack.Push((secondPoppedValue / firstPoppedValue).ToString());
                        }
                    }

                    //if ((operatorStack.Count > 0) && operatorStack.Peek() == "(")
                    //{
                    //    operatorStack.Pop();
                    //}
                }

                previousToken = token;
            }

            // Store the final result
            double result = 0.0;

            // Value stack will contain a single number.  Pop it and 
            // report as the value of the expression
            if (operatorStack.Count == 0)
            {
                Double.TryParse(valueStack.Peek(), out result);
            }
            else if (operatorStack.Count != 0)
            {
                // There will be exactly one operator on the operator stack, and it 
                // will be either + or -.There will be exactly two values on the value 
                // stack.Apply the operator to the two values and report the result as 
                // the value of the expression.
                if ((valueStack.Count > 1) && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                {
                    string operators = operatorStack.Pop();
                    double firstPoppedValue = 0.0;
                    double secondPoppedValue = 0.0;
                    Double.TryParse(valueStack.Pop(), out firstPoppedValue);
                    Double.TryParse(valueStack.Pop(), out secondPoppedValue);

                    if (operators == "+")
                        result = firstPoppedValue + secondPoppedValue;
                    else if (operators == "-")
                        result = secondPoppedValue - firstPoppedValue;
                }
            }
            return result;
        }

        /// <summary>
        /// Add a zero-parameter method GetVariables() to the Formula class.  
        /// It should return an ISet<string> that contains each distinct variable (in normalized form) that appears in the Formula.
        /// </summary>
        /// <returns></returns>
        public ISet<String> GetVariables()
        {
            HashSet<string> copy = new HashSet<string>(normalized_vars);
            return copy;
        }

        /// <summary>
        /// Override the ToString() method so that it returns a string version of the Formula (in normalized form).  
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Since tokens is a list, so I use for loop to get each item and put into a string.
            string formula = "";
            for (int i = 0; i < tokens.Count; i++)
            {
                formula += tokens[i];
            }
            return formula;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        public object Evaluate(Func<string, double> lookup)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    public delegate string Normalizer(string s);

    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}