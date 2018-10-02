using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

/// <summary>
/// 
/// Class: CS-3500
/// Author: Yingjie Lian & Xiaochuang Huang
/// Team: KungfuPanda 
/// Version: 4.2.2018
/// </summary>
namespace UnitTestsForSpreadsheetGUI
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SpreadsheetGUIViewStub stub = new SpreadsheetGUIViewStub();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }

        [TestMethod]
        public void TestMethod2()
        {
            SpreadsheetGUIViewStub stub = new SpreadsheetGUIViewStub();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledOpenNew);
        }

        [TestMethod]
        public void TestMethod3()
        {
            SpreadsheetGUIViewStub stub = new SpreadsheetGUIViewStub();
            Controller controller = new Controller(stub);
            stub.FireHelpEvent();
            Assert.IsTrue(stub.CalledDoHelp);
        }

        [TestMethod]
        public void TestMethod4()
        {
            SpreadsheetGUIViewStub stub = new SpreadsheetGUIViewStub();
            Controller controller = new Controller(stub);
            stub.FireValueEvent();
            Assert.IsTrue(stub.CalledDoValue);
        }

    }
}
