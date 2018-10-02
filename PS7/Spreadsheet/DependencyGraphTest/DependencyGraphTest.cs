using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace Dependencies
{
    [TestClass]
    public class UnitTest1
    {

        //Create a dictionary for the dependency graph 
        DependencyGraph newGraph = new DependencyGraph();



        /// <summary>
        ///  This test is used to test the dependency graph has the 
        ///  correct size, make sure the size adding and reducing is correct
        /// </summary>

        [TestMethod]
        public void TestSize()
        {
            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("apple", "cat");
            Assert.AreEqual(2, newGraph.Size);
        }

        /// <summary>
        ///  This test is used to test the dependency graph has the dependents 
        ///  or not, make it has correct value in the graph
        /// </summary>

        [TestMethod]
        public void TestHasdependents()
        {
            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("apple", "grape");
            newGraph.AddDependency("banana", "pear");

            Assert.IsTrue(newGraph.HasDependents("apple"));
            Assert.IsTrue(newGraph.HasDependents("banana"));
            Assert.IsFalse(newGraph.HasDependents("pear"));

            newGraph.RemoveDependency("banana", "pear");
            Assert.IsFalse(newGraph.HasDependents("banana"));
        }

        /// <summary>
        /// This test is used to test the dependency graph has the dependents 
        /// or not, make it has correct value in the graph
        /// </summary>
        [TestMethod]
        public void TestHasdependee()
        {
            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("apple", "grape");
            newGraph.AddDependency("banana", "pear");

            Assert.IsTrue(newGraph.HasDependees("banana"));
            Assert.IsTrue(newGraph.HasDependees("grape"));
            Assert.IsFalse(newGraph.HasDependees("apple"));

            newGraph.RemoveDependency("apple", "grape");
            Assert.IsFalse(newGraph.HasDependees("grape"));
        }
        [TestMethod]
        public void TestAddandGetDependentsAndDependees()
        {
            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("apple", "grape");

            newGraph.AddDependency("banana", "pear");
            newGraph.AddDependency("apple", "egg");

            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("coke", "water");


            Assert.IsTrue(newGraph.GetDependees("pear").Contains("banana"));

            Assert.IsTrue(newGraph.GetDependents("apple").Contains("egg"));
            Assert.IsTrue(newGraph.GetDependents("apple").Contains("grape"));
        }
        /// <summary>
        ///  This test is used to test the remove method, add some string into 
        /// the graph and use the remove method, one or two string, check the string
        /// is exist or not
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("egg", "grape");
            newGraph.AddDependency("banana", "pear");

            newGraph.RemoveDependency("apple", "banana");
            newGraph.RemoveDependency("grape", "banana");


            Assert.IsFalse(newGraph.GetDependents("apple").Contains("banana"));
            Assert.IsTrue(newGraph.GetDependents("egg").Contains("grape"));
        }
        /// <summary>
        ///  This test is used to test the replace method, add some string into
        ///  the graph and set up a string array and put something into it, then
        ///  use the replace method replace the string in the array, and check is
        ///  replace or not.
        /// </summary>
        [TestMethod]
        public void TestReplace()
        {
            newGraph.AddDependency("apple", "banana");
            newGraph.AddDependency("apple", "grape");
            newGraph.AddDependency("apple", "pear");
            newGraph.AddDependency("banana", "egg");
            newGraph.AddDependency("egg", "egg");

            string[] replacement = new string[] { "apple", "milk", "water" };

            newGraph.ReplaceDependents("apple", replacement);
            Assert.IsTrue(newGraph.GetDependents("apple").Contains("apple"));
            Assert.IsTrue(newGraph.GetDependents("apple").Contains("milk"));
            Assert.IsTrue(newGraph.GetDependents("apple").Contains("water"));


            newGraph.ReplaceDependees("egg", replacement);
            Assert.IsTrue(newGraph.GetDependees("egg").Contains("apple"));
            Assert.IsTrue(newGraph.GetDependees("egg").Contains("milk"));
            Assert.IsTrue(newGraph.GetDependees("egg").Contains("water"));
        }


    }
}
