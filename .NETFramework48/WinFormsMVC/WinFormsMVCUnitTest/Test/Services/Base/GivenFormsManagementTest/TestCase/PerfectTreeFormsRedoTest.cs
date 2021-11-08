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
    public class PerfectTreeFormsRedoTest : PerfectTreeFormsTest
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


        public virtual void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified, 
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_undo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf_RootInvoker(modified, assert);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf_RootInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        public virtual void CalledByRootInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByRootInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByRootInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        public virtual void RecursiveFromRootInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.RecursiveFromRootInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.RecursiveFromRootInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void CalledBySelf_LastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf_LastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf_LastInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void CalledByLastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            
            base.CalledByLastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByLastInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());

        }

        [TestMethod, TestCategory("差分")]


        public virtual void RecursiveFromLastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromLastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.RecursiveFromLastInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void CalledByFirstAndLastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByFirstAndLastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByFirstAndLastInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void CalledBySecondLeftInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySecondLeftInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySecondLeftInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());

        }


        [TestMethod, TestCategory("差分")]


        public virtual void CalledBySecondRightInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySecondRightInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySecondRightInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void RecursiveFromSecondLeftRootInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.RecursiveFromSecondLeftRootInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.RecursiveFromSecondLeftRootInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void CalledByAllLeftInvokers()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByAllLeftInvokers(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByAllLeftInvokers(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]


        public virtual void CalledByAllRightInvokers()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByAllRightInvokers(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByAllRightInvokers(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }


        [TestMethod, TestCategory("差分")]
        public virtual void CalledBySelf_AllLeftInvokers()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            
            base.CalledBySelf_AllLeftInvokers(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf_AllLeftInvokers(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }


        [TestMethod, TestCategory("差分")]
        public virtual void CalledByNullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByNullInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        public virtual void ValidationError()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            
            base.ValidationError(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationError(null, null);
            AssertRedo((commands, forms) => { });
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());

        }

        [TestMethod, TestCategory("差分")]
        public virtual void ValidationNullCheck()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationNullCheck(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationNullCheck(null, null);
            AssertRedo((commands, forms) => { });
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }
    }
}
