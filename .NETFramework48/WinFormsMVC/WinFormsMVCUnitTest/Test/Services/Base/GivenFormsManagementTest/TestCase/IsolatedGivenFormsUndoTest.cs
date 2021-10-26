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

        [TestMethod, TestCategory("差分")]

        public override void CalledBySelf_NullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf_NullInvoker();

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            }));
        }
    }
}
