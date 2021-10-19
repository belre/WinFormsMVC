using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WinFormsMVCUnitTest.Test.Controller.BaseController
{
    [TestClass]
    public class ConstructorTest
    {
        [TestMethod]
        public void Ctor()
        {
            var controller = new WinFormsMVC.Controller.BaseController();
        }
    }
}
