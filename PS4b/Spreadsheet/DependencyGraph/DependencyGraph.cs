// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Author: Yingjie Lian
/// Class: Class-3500
/// Version: 1.30.2018
/// </summary>
namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        // Declare variables to store needed values in future codes
        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;
        private int size;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            // Initialize variables in the constructor, this will simplize our sotre
            // in future code. When we need to call or use them, we can just type the name 
            // them.
            this.dependents = new Dictionary<string, HashSet<string>>();
            this.dependees = new Dictionary<string, HashSet<string>>();
            this.size = 0;
        }

        /// <summary>
        /// Add a new constructor that takes a DependencyGraph as its parameter.
        /// </summary>
        public DependencyGraph(DependencyGraph dg)
        {
            dg = new DependencyGraph();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            // Return the size
            get { return this.size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty. s must not be null.
        /// If s is null, then throw ArgumentNullException.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // Initialize a string type hashset named result to hold values
            HashSet<string> result;
            if(dependents.ContainsKey(s))
            {
                dependents.TryGetValue(s, out result);

                // If the result is not empty, then return true
                if (result.Count != 0)
                {
                    return true;
                }
                // Return false otherwise
                return false;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  s must not be null.
        /// If s is null, then throw ArgumentNullException.
        /// </summary>
        public bool HasDependees(string s)
        {

            if (s == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // Initialize a string type hashset named result to hold values
            HashSet<string> result;
            if (dependees.ContainsKey(s))
            {
                dependees.TryGetValue(s, out result);

                // If the result is not empty, then return true
                if (result.Count != 0)
                {
                    return true;
                }
                // Return false otherwise
                return false;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s). s must not be null.
        /// If s is null, then throw ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // If s is in the dependent, then return it, else will
            // be empty, but implements IEnumerable
            if (dependents.ContainsKey(s))
            {
                return new HashSet<String>(dependents[s]);
            }
            return new HashSet<String>();
        }

        /// <summary>
        /// Enumerates dependees(s).  s must not be null.
        /// If s is null, then throw ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // If s is in the dependee, then return it, else will
            // be empty, but implements IEnumerable
            if (dependees.ContainsKey(s))
            {
                return new HashSet<String>(dependees[s]);
            }
            return new HashSet<String>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// s and t must not be null. If s or t is null, then throw ArgumentNullException.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // Initialize a string type hashset named result to hold values
            HashSet<string> result;

            // If s contains dependents
            if(dependents.ContainsKey(s))
            {
                // Get all dependents of s and store it in result
                dependents.TryGetValue(s, out result);

                // If t is not the dependents
                if(!result.Contains(t))
                {
                    // Add t into the result
                    result.Add(t);

                    // Store the result back into the key position of dictionary
                    dependents[s] = result;

                    // If t does not have dependees
                    if(!dependees.ContainsKey(t))
                    {
                        // Initialize result again and add s into result
                        result = new HashSet<string>();
                        result.Add(s);

                        // Add t and result into the dependees dictionary
                        dependees.Add(t, result);
                    }
                    else
                    {
                        // Else, try to get all dependees of t
                        dependees.TryGetValue(t, out result);

                        // Add s into result
                        result.Add(s);

                        // Store result into key position of dependees dictionary
                        dependees[t] = result;
                    }
                    // Increse size
                    this.size++;
                }
            }
            else
            {
                // Else, initialize result again
                result = new HashSet<string>();

                // Add t into result
                result.Add(t);

                // Store s and result into the dependents dictionary
                dependents.Add(s, result);

                // If t does not have dependee
                if(!dependees.ContainsKey(t))
                {
                    // Initialize result again
                    result = new HashSet<string>();

                    // Add s into result
                    result.Add(s);

                    // Store into the dependees dictionary
                    dependees.Add(t, result);
                }
                else
                {
                    // Else, try to get all dependees of t
                    dependees.TryGetValue(t, out result);

                    // Add s into result
                    result.Add(s);

                    // Store result in key position of dependees dictionary
                    dependees[t] = result;
                }
                // Increse size
                this.size++;
            }

        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// s and t must not be null. If s or t is null, then throw ArgumentNullException.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // Set two string hashsets named dependentResult and dependeesResult
            HashSet<String> dependentResult;
            HashSet<String> dependeesResult;

            // If s has dependents
            if(dependents.ContainsKey(s))
            {
                // Try to get all dependets of s
                dependents.TryGetValue(s, out dependentResult);

                // Try to get all dependees of t
                dependees.TryGetValue(t, out dependeesResult);

            // If t has dependents
            if(dependentResult.Contains(t))
                {
                    // Remove t from dependentResult
                    dependentResult.Remove(t);

                    // Store dependentResult into key position of dependents dictionary
                    dependents[s] = dependentResult;

                    // Remove s from dependeesResult
                    dependeesResult.Remove(s);

                    // Store dependeesResult into key position of dependees dictionary
                    dependees[t] = dependeesResult;

                    // Decrease the size
                    this.size--;
                }

            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// s and t must not be null. If s or t is null, then throw ArgumentNullException.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null)
                throw new ArgumentNullException("Parameters must not be null!");

            // Initialize a string type hashset named result to hold values
            HashSet<string> result;

            // If s has dependent
            if(dependents.ContainsKey(s))
            { 
                // Try to get all dependents of s
                dependents.TryGetValue(s, out result);

                // Using for-each loop to handle all string in result
                foreach (string r in result.ToArray())
                {
                    // Call the RemoveDependency method to remove needed item
                    RemoveDependency(s, r);
                }

                // Using for-each loop to handle all string in newDependents
                foreach(string t in newDependents)
                {
                    // Call the AddDependency method to add needed item
                    AddDependency(s, t);
                }
            }
            else
            {
                // Using for-each loop to handle all string in result
                foreach (string t in newDependents)
                {
                    // Call the AddDependency method to add needed item
                    AddDependency(s, t);
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// s and t must not be null. If s or t is null, then throw ArgumentNullException.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null)
                throw new ArgumentNullException("Parameters must not be null!");
            // Initialize a string type hashset named result to hold values
            HashSet<string> result;

            // If t has dependees
            if(dependees.ContainsKey(t))
            {
                // Try to get all dependees of t
                dependees.TryGetValue(t, out result);

                // Using for-each loop to handle all string in result
                foreach (string r in result.ToArray())
                {
                    // Call the RemoveDependency method to remove needed item
                    RemoveDependency(r, t);
                }

                // Using for-each loop to handle all string in result
                foreach (string s in newDependees)
                {
                    // Call the AddDependency method to add needed item
                    AddDependency(s, t);
                }
            }
            else
            {
                // Using for-each loop to handle all string in result
                foreach (string s in newDependees)
                {
                    // Call the AddDependency method to add needed item
                    AddDependency(s, t);
                }
            }
        }
    }
}
