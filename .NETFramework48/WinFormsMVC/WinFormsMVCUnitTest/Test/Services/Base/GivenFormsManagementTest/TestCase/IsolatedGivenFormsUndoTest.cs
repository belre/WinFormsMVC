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
            TestActionMode = ActionMode.SIMPLE_ACTION;
        }

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            AssertMemorableAction(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySelf(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

            AssertUndo(assert_undo);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBy2Invokers(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBy2Invokers(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

            AssertUndo(assert_undo);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledByExistedInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledByExistedInvoker(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

            AssertUndo(assert_undo);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySelf_NullInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf_NullInvoker(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
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
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

            AssertUndo(assert_undo);
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void RecursiveFromExistedInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromExistedInvoker(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

            AssertUndo(assert_undo);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void ValidationError(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError(modified, assert);

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidationError();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
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
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

            AssertUndo(assert_undo);
        }
    }
}
