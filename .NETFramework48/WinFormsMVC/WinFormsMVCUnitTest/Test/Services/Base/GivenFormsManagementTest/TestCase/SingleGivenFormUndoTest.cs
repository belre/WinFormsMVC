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

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertMemorableAction(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        public override void CalledBySelf()
        {
            base.CalledBySelf();

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertUndo(); });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledByNullInvoker()
        {
            base.CalledByNullInvoker();
            AssertUndo((commands, forms) => { CommonCommandStatus.AssertUndoButNotTarget(); });
        }

        [TestMethod, TestCategory("差分")]

        public override void ValidationNullCheck()
        {
            base.ValidationNullCheck();

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertNotValidating(); });
        }

        
        [TestMethod, TestCategory("差分")]

        public override void ValidationError()
        {
            base.ValidationError();

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertError(); });
        }
        
    }
}
