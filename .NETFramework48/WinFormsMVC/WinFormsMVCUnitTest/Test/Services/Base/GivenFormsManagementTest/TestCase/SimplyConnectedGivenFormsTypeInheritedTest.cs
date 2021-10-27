using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class SimplyConnectedGivenFormsTypeInheritedTest : SimplyConnectedGivenFormsTest
    {
        public SimplyConnectedGivenFormsTypeInheritedTest()
        {
            var command = CreateDefaultCommand<BaseForm>(BaseFormList.First(), DefaultValidationText(0));
            command.IsIncludingInheritedType = true;

            UpdateCommands(new List<Command>()
            {
                command
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            base.CalledBySelf_RootInvoker(modified,  assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                foreach (var form in forms)
                {
                    if (form == forms.Last())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            base.CalledBySelf_LastInvoker(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]

        public override void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();

                foreach (var form in forms)
                {
                    if (forms.First() == form)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                }
            });

            base.RecursiveFromRootInvoker(modified, assert);
        }
    }
}
