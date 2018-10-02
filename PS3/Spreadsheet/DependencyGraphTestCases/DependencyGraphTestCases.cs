using System;
using System.Collections.Generic;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
/// <summary>
/// Author: Yingjie Lian
/// Class: Class-3500
/// Version: 2.2.2018
/// </summary>
namespace DependencyGraphTestCases
{
    [TestClass]
    public class DependencyGraphTestCases
    {

        /// <summary>
        /// This test is going to test empty DependencyGraph that should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest01()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///This test is going to remove from an empty DependencyGraph and it shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest02()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("a", "b");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// This test is going to test the HasDependents method.
        /// Reports whether dependents(s) is non-empty. In this test, 
        /// the DependencyGraph's dependent is empty, so it should return false
        ///</summary>
        [TestMethod()]
        public void EmptyTest03()
        {
            DependencyGraph t = new DependencyGraph();
            string s = "s";
            Assert.IsFalse(t.HasDependents(s));
        }

        /// <summary>
        /// This test is going to test the HasDependents method.
        /// Reports whether dependee(s) is non-empty. In this test, 
        /// the DependencyGraph's dependee is empty, so it should return false
        ///</summary>
        [TestMethod()]
        public void EmptyTest04()
        {
            DependencyGraph t = new DependencyGraph();
            string s = "s";
            Assert.IsFalse(t.HasDependees(s));
        }

        /// <summary>
        /// This test is going to replace on an empty DependencyGraph and
        /// it shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest05()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// This test is going to test the HasDependents method.
        /// Reports whether dependent(s) is non-empty. In this test, 
        /// the DependencyGraph's dependee is non-empty, so it should return true
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest01()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "s");
            string s = "s";
            Assert.IsTrue(t.HasDependents(s));
        }

        /// <summary>
        /// This test is going to Remove the Dependency from a
        /// non-empty DependencyGraph.
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest02()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.RemoveDependency("a", "b");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        /// This test is going to Replace the Dependent from a
        /// non-empty DependencyGraph.
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest03()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> aPends = new HashSet<string>(t.GetDependents("a"));
            Assert.IsTrue(aPends.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        /// <summary>
        /// This test is going to Replace the Dependee from a
        /// non-empty DependencyGraph.
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest04()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependees("c", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> cDees = new HashSet<string>(t.GetDependees("c"));
            Assert.IsTrue(cDees.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        /// <summary>
        /// This test is going to set the DependencyGraph = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
        /// And show all the dependents and dependees:
        ///     dependents("a") = {"b", "c"}
        ///     dependents("b") = {"d"}
        ///     dependents("c") = {}
        ///     dependents("d") = {"d"}
        ///     dependees("a") = {}
        ///     dependees("b") = {"a"}
        ///     dependees("c") = {"a"}
        ///     dependees("d") = {"b", "d"}
        /// </summary>
        [TestMethod]
        public void NonEmptyTest05()
        {
            DependencyGraph test01 = new DependencyGraph();
            test01.AddDependency("a", "b");
            test01.AddDependency("a", "c");
            test01.AddDependency("b", "d");
            test01.AddDependency("d", "d");
            HashSet<String> aDents = new HashSet<String>(test01.GetDependents("a"));
            HashSet<String> bDents = new HashSet<String>(test01.GetDependents("b"));
            HashSet<String> cDents = new HashSet<String>(test01.GetDependents("c"));
            HashSet<String> dDents = new HashSet<String>(test01.GetDependents("d"));
            HashSet<String> eDents = new HashSet<String>(test01.GetDependents("e"));
            HashSet<String> aDees = new HashSet<String>(test01.GetDependees("a"));
            HashSet<String> bDees = new HashSet<String>(test01.GetDependees("b"));
            HashSet<String> cDees = new HashSet<String>(test01.GetDependees("c"));
            HashSet<String> dDees = new HashSet<String>(test01.GetDependees("d"));
            HashSet<String> eDees = new HashSet<String>(test01.GetDependees("e"));
            Assert.IsTrue(aDents.Count == 2 && aDents.Contains("b") && aDents.Contains("c"));
            Assert.IsTrue(bDents.Count == 1 && bDents.Contains("d"));
            Assert.IsTrue(cDents.Count == 0);
            Assert.IsTrue(dDents.Count == 1 && dDents.Contains("d"));
            Assert.IsTrue(eDents.Count == 0);
            Assert.IsTrue(aDees.Count == 0);
            Assert.IsTrue(bDees.Count == 1 && bDees.Contains("a"));
            Assert.IsTrue(cDees.Count == 1 && cDees.Contains("a"));
            Assert.IsTrue(dDees.Count == 2 && dDees.Contains("b") && dDees.Contains("d"));
        }

        /// <summary>
        /// This test is going to test when the DependencyGraph objects contains 100,000 
        /// dependencies, all of its methods should appear (to a human observer) to run instantly.
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100000;
            string[] letters = new string[SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                t.AddDependency("a", "b");
                t.HasDependees("b");
                t.HasDependents("a");
                t.GetDependees("b");
                t.GetDependents("a");
                t.RemoveDependency("a", "b");
                t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
                t.ReplaceDependees("c", new HashSet<string>() { "x", "y", "z" });

            }
        }
    }
}
