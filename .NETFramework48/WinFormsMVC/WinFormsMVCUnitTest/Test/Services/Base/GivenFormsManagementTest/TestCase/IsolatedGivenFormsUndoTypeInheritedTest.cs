﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class IsolatedGivenFormsUndoTypeInheritedTest : IsolatedGivenFormsUndoTest
    {
        public IsolatedGivenFormsUndoTypeInheritedTest()
        {
            var command = CreateDefaultCommand<BaseForm>(BaseFormList.First(), ValidationText);
            command.IsIncludingInheritedType = true;

            UpdateCommands(new List<Command>()
            {
                command
            });
        }
    }
}