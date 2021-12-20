using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class SimplyConnectedGivenFormsTest : SimplyConnectedFormsTest
    {

        public SimplyConnectedGivenFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = (new BaseFormModel()).CreateSimplyConnectedForms(DefaultBaseForm, BaseForm.MaxDepthTree, true);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseFormModel.ChildForm2>(forms.First(), DefaultValidationText(0))
            });
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.CalledBySelf_RootInvoker(modified, assert);
        }


        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                Assert.IsTrue(CommonCommandStatus.WasValidation);
                Assert.IsFalse(CommonCommandStatus.WasFinalized);
                Assert.IsFalse(CommonCommandStatus.WasError);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.CalledBySelf_LastInvoker(modified, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                Assert.IsTrue(CommonCommandStatus.WasValidation);
                Assert.IsFalse(CommonCommandStatus.WasFinalized);
                Assert.IsFalse(CommonCommandStatus.WasError);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form.GetType() != typeof(BaseFormModel.ChildForm2))
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

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationError(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = (item) =>
                {
                    item.Next = DefaultValidationText(0);
                    CommonCommandStatus.WasValidation = true;
                    return false;
                };
            });

            base.ValidationError(modified, assert);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = null;
            });

            base.ValidationNullCheck(modified, assert);
        }

    }
}
