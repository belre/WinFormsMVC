using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Controller;
using WinFormsMVC.Services;

namespace WinFormsMVCUnitTest.Test.Facade.ViewFacade
{
    [TestClass]
    public class GetControllerTest
    {
        public class Test1Controller : BaseController
        {

        }
        public class Test2Controller : BaseController
        {

        }

        public class Test3Controller : BaseController
        {

        }

        protected FormsManagement Manager
        {
            get;
        }

        protected WinFormsMVC.Facade.ViewFacadeCore FacadeCore
        {
            get;
        }

        public GetControllerTest()
        {
            Manager = new FormsManagement();
            FacadeCore = new WinFormsMVC.Facade.ViewFacade(Manager);
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
