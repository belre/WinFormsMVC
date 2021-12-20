using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class IsolatedGivenFormsTypeInheritedTest : IsolatedGivenFormsTest
    {
        public IsolatedGivenFormsTypeInheritedTest()
        {
            var command = CreateDefaultCommand<BaseForm>(BaseFormList.First(), ValidationText);
            command.IsIncludingInheritedSubclass = true;

            UpdateCommands(new List<Command>()
            {
                command
            });
        }
    }
}
