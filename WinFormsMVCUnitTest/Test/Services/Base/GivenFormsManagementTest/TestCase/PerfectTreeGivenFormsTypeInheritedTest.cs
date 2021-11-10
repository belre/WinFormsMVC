using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class PerfectTreeGivenFormsTypeInheritedTest : PerfectTreeGivenFormsTest
    {
        public PerfectTreeGivenFormsTypeInheritedTest()
        {
            var command = CreateDefaultCommand<BaseForm>(BaseFormList.First(), DefaultValidationText(0));
            command.IsIncludingInheritedType = true;

            UpdateCommands(new List<Command>()
            {
                command
            });
        }


        /// <summary>
        /// 処理が消える
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="assert"></param>
        public override void RecursiveFromRootInvokerInSingleLevel(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            
        }

        /// <summary>
        /// 処理が消える
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="assert"></param>
        public override void RecursiveFromSecondLeftRootInvokerInSingleLevel(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

        }
    }
}
