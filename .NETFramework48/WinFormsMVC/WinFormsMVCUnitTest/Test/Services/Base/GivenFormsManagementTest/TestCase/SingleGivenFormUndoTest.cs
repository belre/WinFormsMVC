using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class SingleGivenFormUndoTest : SingleGivenFormTest
    {
        public SingleGivenFormUndoTest()
        {
            var forms = new List<BaseForm>()
            {
                new BaseFormModel.ChildForm1() { Text = DefaultText }
            };
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                new GenericCommand<BaseFormModel.ChildForm1, TextItem>() {
                    Invoker = forms.First(),
                    IsForSelf = true,
                    Validation = (item) =>
                    {
                        item.Next = ValidationText;
                        CommonCommandStatus.WasValidation = true;
                        return true;
                    },
                    NextOperation = ((item, form1) =>
                    {
                        item[form1] = form1.Text;
                        form1.Text = item.Next;
                        CommonCommandStatus.WasNext = true;
                    }),
                    PrevOperation = ((item, form1) =>
                    {
                        form1.Text = item[form1];
                        CommonCommandStatus.WasPrev = true;
                    }),
                    FinalOperation = ((item) => { CommonCommandStatus.WasFinalized = true; }),
                    ErrorOperation = ((item) => { CommonCommandStatus.WasError = true; })
                }
            });

            
            TestActionMode = ActionMode.MEMORABLE_ACTION;
        }

        [TestMethod, TestCategory("差分")]
        public override void CalledBySelf()
        {
            AssertMemorableAction((list, forms) =>
            {

            }, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(ValidationText, forms.First().Text);
            });

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertUndo(); });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledByNullInvoker()
        {
            AssertMemorableAction((list, forms) =>
            {
                (list[0]).Invoker = null;
                (list[0]).IsForSelf = false;
            }, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((commands.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);         // 該当データがいないのでテキストは同じ
            });

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertUndoButNotTarget(); });
        }

        [TestMethod, TestCategory("差分")]

        public override void ValidationNullCheck()
        {
            AssertMemorableAction((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)list[0]).Validation = null;
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
                Assert.IsFalse((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);
            });

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertNotValidating(); });
        }

        [TestMethod, TestCategory("差分")]

        public override void ValidationError()
        {
            AssertMemorableAction((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)list[0]).Validation = (item) =>
                {
                    item.Next = ValidationText;
                    CommonCommandStatus.WasValidation = true;
                    return false;
                };
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertError();
                Assert.IsTrue((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultText, forms.First().Text);
            });

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertError(); });
        }
    }
}
