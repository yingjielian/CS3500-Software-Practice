using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dependencies;
using Formulas;

/// <summary>
/// Author: Yingjie Lian    
/// UID: U1058784
/// Version: 2.15.2018
/// </summary>
namespace SS
{
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string s is a valid cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names.  On the other hand, 
    /// "Z", "X07", and "hello" are not valid cell names.
    /// 
    /// A spreadsheet contains a unique cell corresponding to each possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// The Dependency Graph class contains all the data for the spreadsheet,
        /// so we can just create a DG to store the needed data in the future.
        /// Invariant: no cells are allowed to have a circular dependency
        /// </summary>
        private DependencyGraph dg;

        /// <summary>
        /// Use Dictionary function to set the cells. The keys are the string which are the cell's
        /// names and the value are the cell's itself.
        /// The reason of using Dictionary is because it contains keys and values both at the same time.
        /// It will be much easier to write other method, e.g we can just return the keys in GetNamesOfAllNonemptyCells
        /// method since that method just need to return all the names which are keys in our dictionary.
        /// </summary>
        private Dictionary<string, Cell> ssCells;

        /// <summary>
        /// Use constructor to initialize the fields.
        /// </summary>
        public Spreadsheet()
        {
            this.dg = new DependencyGraph();
            this.ssCells = new Dictionary<string, Cell>();
        }


        /// <summary>
        /// Since in the instruction, professor said individual cells are an important abstraction in a spreadsheet.
        /// So I just defined an appropriate class of cell to use in the future.
        /// This class also contains a getter to get the content of cells.
        /// </summary>
        private class Cell
        {
            // only one of these will be initialized
            public Object contents { get; private set; }
            public Object value { get; private set; }

            string contents_type;
            string value_type;

            /// <summary>
            /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
            /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
            /// in the grid.)
            /// 
            /// If a cell's contents is a string, its value is that string.
            /// 
            /// If a cell's contents is a double, its value is that double.
            /// 
            /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
            /// The value of a Formula, of course, can depend on the values of variables.  The value 
            /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
            /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
            /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
            /// is a double, as specified in Formula.Evaluate.
            /// 
            /// So this is the constructor for strings.
            /// </summary>
            /// <param name="name"></param>
            public Cell(string name)
            {
                contents = name;
                value = name;
                contents_type = "string"; // Indicate the name
                value_type = contents_type;
            }

            /// <summary>
            /// So this is the constructor for doubles.
            /// </summary>
            /// <param name="name"></param>
            public Cell(double name)
            {
                contents = name;
                value = name;
                contents_type = "double";
                value_type = contents_type;
            }

            /// <summary>
            /// So this is the constructor for Formulas.
            /// </summary>
            /// <param name="name"></param>
            public Cell(Formula name)
            {
                contents = name;
                //value = name.Evaluate();
                contents_type = "Fromula";
                //value_type = value.GetType();
            }
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            // Use ReferenceEquals to check if name is null, and Regex.IsMatch that we used in Formula class
            // to check if name is a variable. If name is null or invalid, throws an InvalidNameException.
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$"))
            {
                throw new InvalidNameException();
            }

            // Initialize a value of type Cell because we are going to call TryGetValue to cetify the type of name
            Cell value;

            // Check if dictionary contains the key (name)
            if(ssCells.TryGetValue(name, out value))
                return value.contents;    // Returnt the value from the key position
            else
                return "";
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // Return all keys of the spreadsheet
            return ssCells.Keys;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            // If name is null or invalid, throws an InvalidNameException.
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$"))
            {
                throw new InvalidNameException();
            }

            // Declare and initialize a new cell. Otherwise, the contents of the named cell becomes number.
            // So we store the number into it.
            Cell cell = new Cell(number);

            // Check if ours cells already contains that key
            if (ssCells.ContainsKey(name))
                ssCells[name] = cell; // If yes, replace the key with the new value
            else
                ssCells.Add(name, cell); // Otherwise, add a new key for that value

            // Replace the dependents of 'name' in the dependency graph with an empty hash set
            dg.ReplaceDependees(name, new HashSet<String>());

            // Since Spreadsheets are never allowed to contain a combination of Formulas that establish 
            // a circular dependency. So we declare and initialize a hashset and call GetCellsToRecalculate
            // to get an Enumerable as a parameter and return the result.
            HashSet<string> result = new HashSet<string>(GetCellsToRecalculate(name));
            return result;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            //If the name is null or the name is invalid, then throw an InvalidNameException
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$"))
            {
                throw new InvalidNameException();
            }

            //If the text is null, then throw the ArgumentNullException
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException();
            }

            // Declare and initialize a new cell. Otherwise, the contents of the named cell becomes number.
            // So we store the number into it.
            Cell cell = new Cell(text);

            // Check if ours cells already contains that key
            if (ssCells.ContainsKey(name))
                ssCells[name] = cell; // If yes, replace the key with the new value
            else
                ssCells.Add(name, cell); // Otherwise, add a new key for that value

            // If the contents is an empty string, we don't want it in the dictionary
            if (ssCells[name].contents == "") 
                ssCells.Remove(name);

            // Replace the dependents of 'name' in the dependency graph with an empty hash set
            dg.ReplaceDependees(name, new HashSet<String>());

            // Since Spreadsheets are never allowed to contain a combination of Formulas that establish 
            // a circular dependency. So we declare and initialize a hashset and call GetCellsToRecalculate
            // to get an Enumerable as a parameter and return the result.
            HashSet<string> result = new HashSet<string>(GetCellsToRecalculate(name));
            return result;

        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            //If the name is null or the name is invalid, then throw an InvalidNameException
            if (ReferenceEquals(name, null) || !Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$"))
            {
                throw new InvalidNameException();
            }

            // Temp variable to hold old dependents 
            IEnumerable<String> old_dependees = dg.GetDependees(name);

            // Replace the dependents of 'name' in the dependency graph with the variables in formula
            dg.ReplaceDependees(name, formula.GetVariables());

            try // Check if the new depdendency graph creates a circular reference
            {
                // If there is no exception
                HashSet<String> all_dependees = new HashSet<String>(GetCellsToRecalculate(name));
                // Create a new cell
                Cell cell = new Cell(formula);
                if (ssCells.ContainsKey(name))    // If it already contains that key
                    ssCells[name] = cell;         // Replace the key with the new value
                else
                    ssCells.Add(name, cell);      // Otherwise add a new key for that value

                return all_dependees;
            }
            catch (CircularException e) // If an exception is caught, we want to keep the old dependents and not change the cell
            {
                dg.ReplaceDependees(name, old_dependees);
                throw new CircularException();
            }

        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (ReferenceEquals(name, null))
                throw new ArgumentNullException();

            if (!Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$"))
                throw new InvalidNameException();

            // GetDependents returns a HashSet ensuring there won't be duplicates
            // changed this from GetDependees to GetDependents and fixed most of my tests  
            return dg.GetDependents(name);
        }
    }


}
