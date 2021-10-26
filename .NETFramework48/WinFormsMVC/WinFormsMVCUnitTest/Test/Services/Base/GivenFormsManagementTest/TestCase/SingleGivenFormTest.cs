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
        public virtual void CalledBySelf()
        {
            AssertAction((list, forms) =>
            {

            }, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(ValidationText, forms.First().Text);
            });
        }

        /*
        [TestMethod, TestCategory("正常系")]
        public void CalledBySelfAndUndo()
        {
            AssertMemorableAction((list, forms) =>
            {

            }, ( commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(ValidationText, forms.First().Text);
            });
            
            AssertUndo(( commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();
                Assert.AreEqual(DefaultText, forms.First().Text);
            });
            
        }
        */

        [TestMethod, TestCategory("異常系")]
        public virtual void CalledByNullInvoker()
        {
            AssertAction((list, forms) =>
            {
                (list[0]).Invoker = null;
                (list[0]).IsForSelf = false;
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);         // 該当データがいないのでテキストは同じ
            });

        }

        [TestMethod, TestCategory("異常系")]
        public virtual void ValidationError()
        {
            AssertAction((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)list[0]).Validation = (item) =>
                {
                    item.Next = ValidationText;
                    CommonCommandStatus.WasValidation = true;
                    return false;
                };
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidationError();
                Assert.IsTrue((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public virtual void ValidationNullCheck()
        {
            AssertAction((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)list[0]).Validation = null;
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
                Assert.IsFalse((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);
            });

        }

    }
}
