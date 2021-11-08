using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class SingleGivenFormTest : GivenFormManagementTestFormat
    {
        public string ValidationText
        {
            get
            {
                return "Validation Text";
            }
        }

        public string DefaultText
        {
            get
            {
                return "First Text";
            }
        }

        public SingleGivenFormTest()
        {
            var forms = new List<BaseForm>()
            {
                new BaseFormModel.ChildForm1() { Text = DefaultText }
            };
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.First(), ValidationText)
            });


            TestActionMode = ActionMode.SIMPLE_ACTION;
        }

        protected override void AssertAction(
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            AssertSimpleAction(modified, assert);
        }


        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) => { });
            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(ValidationText, forms.First().Text);
            });

            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByNullInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) => {
                (list.First()).Invoker = null;
                (list.First()).IsForSelf = false;
            });

            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);         // 該当データがいないのでテキストは同じ
            });

            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void ValidationError(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) => {
                ((CommandValidator<TextItem>)list.First()).Validation = (item) =>
                {
                    item.Next = ValidationText;
                    CommonCommandStatus.WasValidation = true;
                    return false;
                };
            });

            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidationError();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);
            });


            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) => {
                ((CommandValidator<TextItem>)list.First()).Validation = null;
            });

            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
                Assert.IsFalse((commands.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);
            });

            AssertAction(modified, assert);
        }
    }
}
