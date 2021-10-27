using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;
using WinFormsMVCUnitTest.Test.View.BaseForm;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class PerfectTreeGivenFormsUndoTest : PerfectTreeGivenFormsTest
    {

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertMemorableAction(modified, assert);
        }


        public PerfectTreeGivenFormsUndoTest()
        {

        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf_RootInvoker(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledByRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByRootInvoker(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.RecursiveFromRootInvoker(modified, assert);


            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void RecursiveFromRootInvokerInSingleLevel(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromRootInvokerInSingleLevel(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf_LastInvoker(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledByLastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            
            base.CalledByLastInvoker(modified, assert);

            base.AssertUndo(((commands, forms) =>
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
        [DataRow(null, null, null)]
        public virtual void RecursiveFromLastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromLastInvoker(modified, assert);

            base.AssertUndo(((commands, forms) =>
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
        [DataRow(null, null, null)]
        public virtual void CalledByFirstAndLastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByFirstAndLastInvoker(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySecondLeftInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySecondLeftInvoker(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySecondRightInvoker(Action<List<Command>, List<BaseForm>> modified, 
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySecondRightInvoker(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void RecursiveFromSecondLeftRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.RecursiveFromSecondLeftRootInvoker(modified, assert);


            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void RecursiveFromSecondLeftRootInvokerInSingleLevel(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.RecursiveFromSecondLeftRootInvokerInSingleLevel(modified, assert);

            base.AssertUndo((commands, forms) =>
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
        [DataRow(null, null, null)]
        public virtual void CalledByAllLeftInvokers(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByAllLeftInvokers(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledByAllRightInvokers(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByAllRightInvokers(modified, assert);
            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null, null)]
        public virtual void CalledBySelf_AllLeftInvokers(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.CalledBySelf_AllLeftInvokers(modified, assert);

            base.AssertUndo((commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
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

            base.AssertUndo(((commands, forms) =>
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
        [DataRow(null, null, null)]
        public virtual void ValidationError(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            
            base.ValidationError(modified, assert);

            base.AssertUndo(((commands, forms) =>
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
        [DataRow(null, null, null)]
        public virtual void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationNullCheck(modified, assert);

            base.AssertUndo(((commands, forms) =>
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
