using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class SimplyConnectedFormsUndoTypeInheritedTest : SimplyConnectedFormsUndoTest
    {
        public SimplyConnectedFormsUndoTypeInheritedTest()
        {
            var command = CreateDefaultCommand<BaseForm>(BaseFormList.First(), DefaultValidationText(0));
            command.IsIncludingInheritedType = true;

            UpdateCommands(new List<Command>()
            {
                command
            });
        }
    }
}
