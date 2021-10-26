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
    public class PerfectTreeFormsUndoTest : PerfectTreeFormsTest
    {

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertMemorableAction(modified, assert);
        }


        public PerfectTreeFormsUndoTest()
        {

        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledByRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledByLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void RecursiveFromLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledByFirstAndLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledBySecondLeftInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledBySecondRightInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void RecursiveFromSecondLeftRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledByAllLeftInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledByAllRightInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledBySelf_AllLeftInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void CalledByNullInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void ValidationError(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
        [DataRow(null, null)]
        public override void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
