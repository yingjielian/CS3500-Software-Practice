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
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        private Dictionary<string, HashSet<string>> dependent;
        private Dictionary<string, HashSet<string>> dependee;
        private int size;

        public DependencyGraph(DependencyGraph dg)
        {
            if(dg == null)
            {
                throw new ArgumentNullException("Dependency Graph is null");
            }
            dependent = new Dictionary<string, HashSet<string>>();
            dependee = new Dictionary<string, HashSet<string>>();
            foreach(KeyValuePair<string,HashSet<string>> newSet in dg.dependee)
            {
                dependee.Add(newSet.Key, new HashSet<string>(newSet.Value));
            }
            foreach (KeyValuePair<string, HashSet<string>> newSet in dg.dependent)
            {
                dependent.Add(newSet.Key, new HashSet<string>(newSet.Value));
            }
            size = dg.size;
        }

        public DependencyGraph()
        {
            
            dependent = new Dictionary<string, HashSet<string>>();

            dependee  = new Dictionary<string, HashSet<string>>();

            size = 0;
    
        }


        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  
        /// If s is equal to null, it will throw to argument null exception
        /// </summary>
        public bool HasDependents(string s)
        {
            if(s == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
           if(dependent.ContainsKey(s) == true)
            {
                if(dependent[s].Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
               
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  
        /// If s is equal to null, it will throw arugument null exception
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
            if (dependee.ContainsKey(s)== true)
            {
                if (dependee[s].Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerates dependents(s). 
        /// If s equal null, it will throw arugument null exception
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
            HashSet<string> getD = new HashSet<string>();
            if(dependent.ContainsKey(s))
            {
                getD = dependent[s];

                return getD;
            }
            else
            {
                return getD;
            }


            

        }

        /// <summary>
        /// Enumerates dependees(s).
        /// If s is equal to null, it will throw arugument null exception
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
            HashSet<string> getD = new HashSet<string>();
            if(dependee.ContainsKey(s))
            {
                getD = dependee[s];
                return getD;
            }

            else
            {
                return getD;
            }


            
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// If s or t are equal to null, it will throw arugument null exception
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
            if (dependent.ContainsKey(s) && dependee.ContainsKey(t))
            {
                if (!dependent[s].Contains(t) && (dependee[t].Contains(s)))
                {
                    dependent[s].Add(t);
                    size++;
                }
                else
                {
                    if (dependent[s].Contains(t) && !dependee[t].Contains(s))
                    {
                        dependee[t].Add(s);
                        size++;
                    }
                    else
                    {
                        if (!dependent[s].Contains(t) && !dependee[t].Contains(s))
                        {
                            dependent[s].Add(t);
                            dependee[t].Add(s);
                            size++;
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                if (dependent.ContainsKey(s) && !(dependee.ContainsKey(t)))
                {
                    dependent[s].Add(t);

                    HashSet<string> newdependee = new HashSet<string>();
                    newdependee.Add(s);
                    dependee.Add(t, newdependee);
                    size++;
                }
                else
                {
                    if (!(dependent.ContainsKey(s)) && dependee.ContainsKey(t))
                    {
                        HashSet<string> newdenpendent = new HashSet<string>();
                        newdenpendent.Add(t);
                        dependent.Add(s, newdenpendent);
                        dependee[t].Add(s);
                        size++;
                    }
                    else
                    {
                        HashSet<string> newdependent = new HashSet<string>();
                        newdependent.Add(t);
                        dependent.Add(s, newdependent);

                        HashSet<string> newdependee = new HashSet<string>();
                        newdependee.Add(s);
                        dependee.Add(t, newdependee);

                        size++;
                    }
                }
            }
        }
        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        ///  If s or t are equal to null, it will throw arugument null exception
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
            if (!(dependent.ContainsKey(s) && dependee[t].Contains(s)))
            {

            }
            else
            {
                dependent[s].Remove(t);
                dependee[t].Remove(s);
                size--;
            }
        }


        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// If s or t are equal to null, it will throw arugument null exception.
        /// they throw an ArgumentNullException when a null string is encountered in their IEnumerable parameters.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null ||newDependents == null)
            {
                throw new ArgumentNullException("parameters are null");
            }
            if (dependent.ContainsKey(s))
            {
                size -= dependent[s].Count;
                HashSet<string> newDependent = new HashSet<string>();
                newDependent = dependent[s];

                foreach(string str in newDependent)
                {
                    if (str == null)
                    {
                        throw new ArgumentNullException("parameters are null");
                    }
                    dependee[str].Remove(s);

                  
                }
                dependent[s].Clear();
                foreach (string newS in newDependents)
                {
                    if (newS == null)
                    {
                        throw new ArgumentNullException("parameters are null");
                    }
                    AddDependency(s, newS);
                }
            }

            else
            {
                foreach (string newS in newDependents)
                {
                    if (newS == null)
                    {
                        throw new ArgumentNullException("parameters are null");
                    }
                    AddDependency(s, newS);
                }
            }

           
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// If s or t are equal to null, it will throw arugument null exception
        /// they throw an ArgumentNullException when a null string is encountered in their IEnumerable parameters.
        ///  
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null)
            {
                throw new ArgumentNullException("parameters are null");
            }

            if (dependee.ContainsKey(t))
            {
                size -= dependee[t].Count;
                HashSet<string> newDependee = new HashSet<string>();
                newDependee = dependee[t];
                foreach (string str in newDependee)
                {
                    if (str == null)
                    {
                        throw new ArgumentNullException("parameters are null");
                    }

                    dependent[str].Remove(t);


                }
                dependee[t].Clear();
                foreach (string newT in newDependees)
                {
                    if (newT == null)
                    {
                        throw new ArgumentNullException("parameters are null");
                    }
                    AddDependency(newT, t);
                }
            }

            else
            {
                foreach (string newT in newDependees)
                {
                    if (newT == null)
                    {
                        throw new ArgumentNullException("parameters are null");
                    }
                    AddDependency(newT,t);
                }
            }
           
        }
    }
}
