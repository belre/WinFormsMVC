using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Services;

namespace WinFormsMVCUnitTest.Test.Controller
{
    public class FacadeAndFormManagementTestFormat
    {
        protected FormsManagement Manager
        {
            get;
        }

        protected WinFormsMVC.Facade.ViewFacade Facade
        {
            get;
        }

        public FacadeAndFormManagementTestFormat()
        {
            Manager = new FormsManagement();
            Facade = new WinFormsMVC.Facade.ViewFacade(Manager);
        }
    }
}
