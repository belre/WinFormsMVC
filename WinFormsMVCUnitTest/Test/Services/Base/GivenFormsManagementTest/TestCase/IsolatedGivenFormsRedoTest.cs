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
    public sealed class IsolatedGivenFormsRedoTest : IsolatedGivenFormsTest
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
        
        
        public void CalledBySelf()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        public void CalledBy2Invokers()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBy2Invokers(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBy2Invokers(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]

        public void CalledByExistedInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledByExistedInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByExistedInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        
        
        public void CalledBySelf_NullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;
            base.CalledBySelf_NullInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf_NullInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        
        
        public void CalledByNullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByNullInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }


        [TestMethod, TestCategory("差分")]
        
        
        public void RecursiveFromExistedInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromExistedInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.RecursiveFromExistedInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        
        
        public void ValidationError()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationError(null, null);
            AssertRedo((commands, forms) => { });
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        
        
        public void ValidationNullCheck()
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
