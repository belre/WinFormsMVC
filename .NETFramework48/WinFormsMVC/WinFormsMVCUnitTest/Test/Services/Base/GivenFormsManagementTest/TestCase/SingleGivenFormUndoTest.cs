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

        protected override void AssertUndo(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertUndo(assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySelf(Action<List<Command>, List<BaseForm>> modified, 
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            CalledBySelf(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();
                Assert.AreEqual(DefaultText, forms.First().Text);
            });

            AssertUndo(assert_undo);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledByNullInvoker(Action<List<Command>, List<BaseForm>> modified, 
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
            });

            AssertUndo(assert_undo);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, 
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationNullCheck(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
            });
            
            AssertUndo(assert_undo);
        }

        
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void ValidationError(
            Action<List<Command>, List<BaseForm>> modified, 
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidationError();
            });

            AssertUndo(assert_undo);
        }
        
    }
}
