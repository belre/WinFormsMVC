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

            var forms = BaseFormModel.CreateSimplyConnectedForms(DefaultBaseForm, BaseForm.MaxDepthTree, true);
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
            base.CalledBySelf_RootInvoker(null, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }


        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySelf_LastInvoker(null, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromRootInvoker(null, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
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
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationError(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.ValidationError((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm2, TextItem>))
                    {
                        ((GenericCommand<BaseFormModel.ChildForm2, TextItem>)command).Validation = (item) =>
                        {
                            item.Next = DefaultValidationText(0);
                            _was_validation = true;
                            return false;
                        };
                    }
                }
            }, null);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.ValidationNullCheck((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm2, TextItem>)list[0]).Validation = null;
            }, null);

        }

    }
}
