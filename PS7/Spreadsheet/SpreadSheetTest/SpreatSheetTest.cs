using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;

namespace SS
{
    [TestClass]
    public class SpreadSheetTest
 
    {
        [TestMethod]
        public void TestMethod1()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "Bob");
            Assert.AreEqual("Bob", sheet.GetCellContents("A1"));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod2()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("AA", "Bob");
           
        }
        

        [TestMethod]
        public void TestMethod4()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 4.0);
            Assert.AreEqual(4.0, sheet.GetCellContents("A1"));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod5()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, "Bob");
           
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod6()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, 4.0);
          
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod7()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("AA", 4.0);
          
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod8()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod9()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("AA");

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod10()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, new Formula("A1+A1"));
        
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod11()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("AA", new Formula("A1+A1"));

        }
        [TestMethod]
        public void TestMethod12()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 4.0);
            Assert.AreEqual("A1",new List<string>(sheet.GetNamesOfAllNonemptyCells())[0]);
            Assert.AreEqual(1, new List<string>(sheet.GetNamesOfAllNonemptyCells()).Count);
        }
        [TestMethod]
        public void TestMethod13()
        {

            HashSet<string> set = new HashSet<string>(new Spreadsheet().GetNamesOfAllNonemptyCells());
            Assert.AreEqual(0, set.Count);
        }
        [TestMethod]
        public void SetCellContents5()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", new Formula("A2 + B1"));
            spreadsheet.SetCellContents("B1", new Formula("A3 + 2"));
            spreadsheet.SetCellContents("C1", new Formula("A1 + B1"));
            spreadsheet.SetCellContents("D1", new Formula("C1 + 1"));
            spreadsheet.SetCellContents("E1", new Formula("D1 + 2"));
            spreadsheet.SetCellContents("G1", new Formula("E1 + 3"));

            List<string> set = new List<string>();
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                set.Add(name);
            }
            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
            Assert.IsTrue(set.Contains("D1"));
            Assert.IsTrue(set.Contains("E1"));
            Assert.IsTrue(set.Contains("G1"));
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularException()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", new Formula("A2 + B1"));
            spreadsheet.SetCellContents("B1", new Formula("A1 + C1"));
        }

        [TestMethod]
        public void GetCellContents()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", "Test");
            Assert.AreEqual("Test", spreadsheet.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullText()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            string test = null;
            spreadsheet.SetCellContents("A1", test);
        }
        AbstractSpreadsheet spreadsheet;

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullName1()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents(null, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullName2()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullName3()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents(null, new Formula("X1 + Y2"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidName1()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("XX", "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidName2()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("XX", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidName3()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("XX", new Formula("X1+Y1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidName4()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("X1", new Formula("X2+Y1"));
            spreadsheet.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidName5()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("X1", new Formula("X2+Y1"));
            spreadsheet.GetCellContents("X0");
        }

        [TestMethod]
        public void SetCellContents()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", "Test");
            spreadsheet.SetCellContents("A2", "Test2");
            List<string> set = new List<string>();
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                set.Add(name);
            }
            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("A2"));
        }

        [TestMethod]
        public void SetCellContents2()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", "Test");
            spreadsheet.SetCellContents("A1", "Test2");
            List<string> set = new List<string>();
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                set.Add(name);
            }
            Assert.IsTrue(set.Contains("A1"));
        }

        [TestMethod]
        public void SetCellContents3()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", 2);
            spreadsheet.SetCellContents("A1", 3);
            List<string> set = new List<string>();
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                set.Add(name);
            }
            Assert.IsTrue(set.Contains("A1"));
        }

        [TestMethod]
        public void SetCellContents4()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", new Formula("A2 + B1"));
            spreadsheet.SetCellContents("A1", new Formula("A3 + C1"));
            List<string> set = new List<string>();
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                set.Add(name);
            }
            Assert.IsTrue(set.Contains("A1"));
        }

        [TestMethod]
        public void SetCellContents13()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", new Formula("A2 + B1"));
            spreadsheet.SetCellContents("B1", new Formula("A3 + 2"));
            spreadsheet.SetCellContents("C1", new Formula("A1 + B1"));
            spreadsheet.SetCellContents("D1", new Formula("C1 + 1"));
            spreadsheet.SetCellContents("E1", new Formula("D1 + 2"));
            spreadsheet.SetCellContents("G1", new Formula("E1 + 3"));

            List<string> set = new List<string>();
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                set.Add(name);
            }
            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
            Assert.IsTrue(set.Contains("D1"));
            Assert.IsTrue(set.Contains("E1"));
            Assert.IsTrue(set.Contains("G1"));
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", new Formula("A2 + B1"));
            spreadsheet.SetCellContents("B1", new Formula("A1 + C1"));
        }

        [TestMethod]
        public void TestGetCellContents()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", "Test");
            Assert.AreEqual("Test", spreadsheet.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testNullText()
        {
            spreadsheet = new Spreadsheet();
            string test = null;
            spreadsheet.SetCellContents("A1", test);
        }


        [TestMethod]
        public void testGetAllNonEmptyCellsName()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", "");
            int count = 0;
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                count++;
            }
            //Assert.AreEqual("", spreadsheet.GetCellContents("A1"));
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void GetAllNonEmptyCellsName2()
        {
            spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("A1", "");
            spreadsheet.SetCellContents("A2", "test");
            spreadsheet.SetCellContents("A3", "test2");
            int count = 0;
            foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                count++;
            }
            //Assert.AreEqual("", spreadsheet.GetCellContents("A1"));
            Assert.AreEqual(2, count);
        }

    }
}
