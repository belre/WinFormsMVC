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
            
        }

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertMemorableAction(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        public override void CalledBySelf()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();
                Assert.AreEqual(DefaultText, forms.First().Text);
            });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledByNullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker();
            AssertUndo((commands, forms) => { CommonCommandStatus.AssertUndoButNotTarget(); });
        }

        [TestMethod, TestCategory("差分")]

        public override void ValidationNullCheck()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationNullCheck();

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertNotValidating(); });
        }

        
        [TestMethod, TestCategory("差分")]

        public override void ValidationError()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError();

            AssertUndo((commands, forms) => { CommonCommandStatus.AssertValidationError(); });
        }
        
    }
}
