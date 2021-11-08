using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public sealed class SimplyConnectedGivenFormsUndoAndRedoTest : SimplyConnectedGivenFormsUndoTest
    {

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void CalledBySelf_RootInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf_RootInvoker(null, null, null);

            Define(ref assert_redo, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                CommonCommandStatus.AssertWasRedo();
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            AssertRedo(assert_redo);
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void CalledByRootInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByRootInvoker(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);

                }
            }));
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void CalledBySelf_LastInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf_LastInvoker(null, null);

            AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void CalledByLastInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByLastInvoker(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void CalledByFirstAndLastInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByFirstAndLastInvoker(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void CalledByNullInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker(null, null);


            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void RecursiveFromRootInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromRootInvoker(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void RecursiveFromLastInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromLastInvoker(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void ValidationError(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertValidationError();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));

        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public  void ValidationNullCheck(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.ValidationNullCheck(null, null);

            AssertUndo(((commands, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }));
        }
    }
}
