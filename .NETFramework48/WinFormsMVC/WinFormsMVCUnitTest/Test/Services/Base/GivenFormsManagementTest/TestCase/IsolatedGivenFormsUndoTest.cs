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
        public override void CalledBySelf()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledBy2Invokers()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBy2Invokers();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

        }

        [TestMethod, TestCategory("差分")]
        public override void CalledByExistedInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledByExistedInvoker();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledBySelf_NullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf_NullInvoker();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]

        public override void CalledByNullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }


        [TestMethod, TestCategory("差分")]
        public override void RecursiveFromExistedInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromExistedInvoker();


            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        public override void ValidationError()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertValidationError();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        public override void ValidationNullCheck()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationNullCheck();

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }
    }
}
