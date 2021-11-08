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
    public class PerfectTreeGivenFormsRedoTest : PerfectTreeGivenFormsTest
    {

        public bool IsUndoLock
        {
            get;
            set;
        }

        public Action<IEnumerable<Command>, IEnumerable<BaseForm>> ParentAssertion
        {
            get;
            set;
        }

        /// <summary>
        /// 元に戻した後のやり直しは親と同じ定義にする必要があるので
        /// AssertActionを実行した後にアサーションを記録して
        /// そのアサーション結果を使用する
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="assert"></param>
        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (!IsUndoLock)
            {
                base.AssertMemorableAction(modified, assert);
            }
            else
            {
                ParentAssertion = assert;
            }
        }

        protected override void AssertUndo(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertUndo(assert);
            IsUndoLock = true;
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);

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


            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);

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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);

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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });


            base.AssertUndo(assert_undo);

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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndoButNotTarget();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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


            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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

            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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


            Define(ref assert_undo, (commands, forms) =>
            {
                CommonCommandStatus.AssertUndo();

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);
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
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            base.AssertUndo(assert_undo);

        }
    }
}
