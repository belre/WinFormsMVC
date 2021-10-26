using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class IsolatedGivenFormsUndoTest : IsolatedGivenFormsTest
    {

        public IsolatedGivenFormsUndoTest()
        {
            var forms = new List<BaseForm>()
            {
                new BaseFormModel.ChildForm1() { Text = "First Text, ChildForm1" },
                new BaseFormModel.ChildForm2() { Text = "First Text, ChildForm2-1" },
                new BaseFormModel.ChildForm2() { Text = "First Text, ChildForm2-2" },
                new BaseFormModel.ChildForm3() { Text = "First Text, ChildForm3" }
            };
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                new GenericCommand<BaseFormModel.ChildForm1, TextItem>()
                {
                    Invoker = forms.First(),
                    IsForSelf = true,
                    Validation = (item) =>
                    {
                        item.Next = "Validation Text - ChildForm1";
                        CommonCommandStatus.WasValidation = true;
                        return true;
                    },
                    NextOperation = ((item, form1) =>
                    {
                        CommonCommandStatus.WasNext = true;
                        item[form1] = form1.Text;
                        form1.Text = item.Next;
                    }),
                    PrevOperation = ((item, form1) =>
                    {
                        CommonCommandStatus.WasPrev = true;
                        form1.Text = item[form1];
                    }),
                    FinalOperation = ((item) =>
                    {
                        CommonCommandStatus.WasFinalized = true;
                    }),
                    ErrorOperation = ((item) =>
                    {
                        CommonCommandStatus.WasError = true;
                    })
                }
            });

            TestActionMode = ActionMode.SIMPLE_ACTION;
        }

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            AssertMemorableAction(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        public override void CalledBySelf()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledBy2Invokers()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBy2Invokers();

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            }));

        }

        [TestMethod, TestCategory("差分")]
        public override void CalledByExistedInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledByExistedInvoker();

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            }));

        }
    }
}
