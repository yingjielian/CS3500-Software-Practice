using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using Formulas;
/// <summary>
/// Author: Yingjie Lian
/// UID: U1058784
/// Version: 2.15.2018
/// </summary>
namespace SpreadSheetTests
{
    /// <summary>
    /// The class for testing the spreadsheet
    /// </summary>
    [TestClass]
    public class SpreadSheetTester
    {
        /// <summary>
        /// Initialize new sheet
        /// </summary>
        AbstractSpreadsheet sheet = new Spreadsheet();

        /// <summary>
        /// Test the constructor
        /// </summary>
        [TestMethod]
        public void constructorTest()
        {
            HashSet<string> set = new HashSet<string>(new Spreadsheet().GetNamesOfAllNonemptyCells());
            Assert.AreEqual(0, set.Count);
        }

        /// <summary>
        /// Test GetNamesOfAllNonemptyCells method
        /// </summary>
        [TestMethod]
        public void test01()
        {
            sheet.SetCellContents("A_1", 1.0);
            Assert.AreEqual("A_1", new List<string>(sheet.GetNamesOfAllNonemptyCells())[0]);
            Assert.AreEqual(1, new List<string>(sheet.GetNamesOfAllNonemptyCells()).Count);
        }

        /// <summary>
        /// Test SetCellContents(name, formula) method
        /// </summary>
        [TestMethod]
        public void test02()
        {
            sheet.SetCellContents("x_1", new Formula("x2+1"));
            Assert.AreEqual("x_1", new List<string>(sheet.GetNamesOfAllNonemptyCells())[0]);
            Assert.AreEqual(1, new List<string>(sheet.GetNamesOfAllNonemptyCells()).Count);
        }

        /// <summary>
        /// Test SetCellContents(name, text) method
        /// </summary>
        [TestMethod]
        public void test03()
        {
            sheet.SetCellContents("A2", "helloworld");
            Assert.AreEqual(1, new List<string>(sheet.GetNamesOfAllNonemptyCells()).Count);
            sheet.SetCellContents("A2", "");
            Assert.AreEqual(1, new List<string>(sheet.GetNamesOfAllNonemptyCells()).Count);
        }

        /// <summary>
        /// Test GetCellContents method
        /// </summary>
        [TestMethod]
        public void test04()
        {
            sheet.SetCellContents("A2", "helloworld");
            Assert.AreEqual("helloworld", sheet.GetCellContents("A2"));
        }

        /// <summary>
        /// Test GetCellContents when parameter is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test05()
        {
            sheet.SetCellContents("A2", "helloworld");
            Assert.AreEqual("helloworld", sheet.GetCellContents("A2"));
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Test GetCellContents when parameter is an empty string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test06()
        {
            sheet.SetCellContents("A2", "helloworld");
            Assert.AreEqual("helloworld", sheet.GetCellContents("A2"));
            sheet.GetCellContents("");
        }

        /// <summary>
        /// Test SetCellContents(name, text) when name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test07()
        {
            sheet.SetCellContents(null, "nba");

        }

        /// <summary>
        /// Test SetCellContents(name, formula) when name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test08()
        {
            Formula f = new Formula("3 + 5");
            sheet.SetCellContents(null, f);

        }


        /// <summary>
        /// Test SetCellContents(name, number) method
        /// </summary>
        [TestMethod]
        public void test09()
        {
            sheet.SetCellContents("y_15", 1.5);
            Assert.AreEqual(1.5, sheet.GetCellContents("y_15"));
        }

        /// <summary>
        /// exception test. 
        /// expected: InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test10()
        {
            sheet.SetCellContents("2x", 5.1);
        }

        /// <summary>
        /// exception test. 
        /// expected: InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test11()
        {
            sheet.SetCellContents("", 5.1);
        }

        /// <summary>
        /// exception test. 
        /// expected: InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test12()
        {
            sheet.SetCellContents("25", 5.1);
        }

        /// <summary>
        /// exception test. 
        /// expected: InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void test13()
        {
            sheet.SetCellContents(null, 5.1);
        }

        /// <summary>
        /// exception test. 
        /// expected: ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test14()
        {
            string test = null;
            sheet.SetCellContents("X5", test);
        }


        /// <summary>
        /// exception test. 
        /// expected: ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test15()
        {
            Formula f = new Formula();
            sheet.SetCellContents("X5", f);
        }

        /// <summary>
        /// Test SetCellContents(name, formula) and GetCellContents() when the name is the same with
        /// in SetCellContents() method
        /// </summary>
        [TestMethod]
        public void test16()
        {
            sheet.SetCellContents("x2", new Formula("x1 + y1"));
            Assert.AreEqual("x1+y1", sheet.GetCellContents("x2").ToString());
        }

        /// <summary>
        /// Tests for GetDirectDependents
        /// </summary>
        [TestMethod]
        public void test17()
        {
            PrivateObject accessor = new PrivateObject(new Spreadsheet());
            accessor.Invoke("GetDirectDependents", new Object[] { "A1" });

        }

        /// <summary>
        /// exception test. 
        /// expected: CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void test18()
        {
            sheet.SetCellContents("Y1", new Formula("X1"));
            sheet.SetCellContents("X1", new Formula("Z1"));
            sheet.SetCellContents("Z1", new Formula("Y1"));
        }

        /// <summary>
        /// test19
        /// stress test
        /// </summary>
        [TestMethod]
        public void test19()
        {
            sheet.SetCellContents("B1", new Formula("A1*2"));
            sheet.SetCellContents("C1", new Formula("B1+3"));
            List<string> list = new List<string>(sheet.SetCellContents("A1", 4));
            Assert.IsTrue(list.Contains("A1"));
        }

        /// <summary>
        /// test20
        /// stress test
        /// </summary>
        [TestMethod]
        public void test20()
        {
            sheet.SetCellContents("A1", new Formula("d2+d3"));
            sheet.SetCellContents("A1", new Formula("12+c3"));
            Assert.AreEqual("12+c3", sheet.GetCellContents("A1").ToString());
        }

        /// <summary>
        /// test21
        /// stress test
        /// </summary>
        [TestMethod]
        public void test21()
        {
            sheet.SetCellContents("A1", 1.5);
            sheet.SetCellContents("A1", 2.0);
            Assert.AreEqual(2.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// test22
        /// stress test
        /// </summary>
        [TestMethod]
        public void test22()
        {
            sheet.SetCellContents("B1", new Formula("A1*2"));
            sheet.SetCellContents("C1", new Formula("B1+3"));
            List<string> list = new List<string>(sheet.SetCellContents("A1", "nba"));
            Assert.IsTrue(list.Contains("A1"));
        }
    }
}