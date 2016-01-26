using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebsiteService.Tests.Database;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WebsiteService.Tests.General
{
    public delegate void DoSomethingHandler(int num, string text);

    public class TestEventArgs : EventArgs
    {
        public TestEventArgs(int num, string text)
        {
            Num = num;
            Text = text;
        }

        public int Num { get; set; }
        public string Text { get; set; }
    }

    [TestClass]
    public class DelegatesAndEvents
    {
        private string m_delMessage = "";
        private DoSomethingHandler m_doSomething;
        private EventHandler<TestEventArgs> m_doSomethingToo;

        [TestMethod]
        public void TestDelegate()
        {
            DoSomethingHandler del1 = new DoSomethingHandler(DoSomethingForReal);
            DoSomethingHandler del2 = new DoSomethingHandler(DoSomethingForPretend);

            del1 += del2;

            del1(2, "fish");

            Assert.AreEqual("Something real done 2 times with fish\nSomething made up was done 2 times with fish\n", m_delMessage);
        }

        [TestMethod]
        public void TestEvent()
        {
            DoSomething += DoSomethingForReal;

            m_doSomething(10, "apples");

            Assert.AreEqual("Something real done 10 times with apples\n", m_delMessage);
        }

        [TestMethod]
        public void TestAnything()
        {
            char c = 'Þ';
            string s = "2Þ00006";
            
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (r.IsMatch(s))
            {
                string p = "blah";
            }

            bool isLetter =  char.IsLetterOrDigit(c);

            try
            {
                StaticConstructorTest test = new StaticConstructorTest();
            }
            catch (Exception)
            {

            }

            StaticConstructorTest test2 = new StaticConstructorTest();

            Console.WriteLine("dfd");
        }

        [TestMethod]
        public void TestEvent2()
        {
            DoSomethingToo += DoSomethingForPretendToo;

            DoSomethingToo(this, new TestEventArgs(15, "pears"));

            Assert.AreEqual("Something made up was done 15 times with pears\n", m_delMessage);
        }

        public event EventHandler<TestEventArgs> DoSomethingToo;
        //{
        //    add { m_doSomethingToo += value; }
        //    remove { m_doSomethingToo -= value; }
        //}

        public event DoSomethingHandler DoSomething
        {
            add { m_doSomething += value; }
            remove { m_doSomething -= value; }
        }

        private void DoSomethingForReal(int num, string text)
        {
            m_delMessage += string.Format("Something real done {0} times with {1}\n", num, text);
        }

        private void DoSomethingForPretend(int num, string text)
        {
            m_delMessage += string.Format("Something made up was done {0} times with {1}\n", num, text);
        }

        private void DoSomethingForPretendToo(object sender, TestEventArgs args)
        {
            m_delMessage += string.Format("Something made up was done {0} times with {1}\n", args.Num, args.Text);
        }
    }
}
