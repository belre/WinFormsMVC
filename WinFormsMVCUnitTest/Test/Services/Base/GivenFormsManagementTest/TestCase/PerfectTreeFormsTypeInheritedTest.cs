using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class PerfectTreeFormsTypeInheritedTest : PerfectTreeFormsTest
    {
        public PerfectTreeFormsTypeInheritedTest()
        {
            var command = CreateDefaultCommand<BaseForm>(BaseFormList.First(), DefaultValidationText(0));
            command.IsIncludingInheritedSubclass = true;

            UpdateCommands(new List<Command>()
            {
                command
            });
        }
    }
}
