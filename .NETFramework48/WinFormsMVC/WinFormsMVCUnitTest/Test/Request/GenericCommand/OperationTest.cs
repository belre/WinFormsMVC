using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Request.GenericCommand
{
    [TestClass]
    public class OperationTest
    {
        [TestMethod]
        public void ValidationTest()
        {
            var new_command = new GenericCommand<BaseForm, TextItem>()
            {
                Validation = ((item) =>
                {
                    return true;
                })
            };



        }
    }
}
