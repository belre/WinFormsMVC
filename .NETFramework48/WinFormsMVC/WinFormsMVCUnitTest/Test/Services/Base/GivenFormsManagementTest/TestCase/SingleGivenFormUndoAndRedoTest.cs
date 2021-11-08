using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public sealed class SingleGivenFormUndoAndRedoTest : SingleGivenFormTest, IRedoTest
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

        public SingleGivenFormUndoAndRedoTest()
        {
            
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
        [DataRow(null)]
        public void CalledBySelf(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            CalledBySelf(null, null);
            AssertUndo(((commands, forms) => { }));

            CalledBySelf(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public void CalledByNullInvoker(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByNullInvoker(null, null);
            AssertRedo((commands, forms) => CommonCommandStatus.AssertWasRedo());
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public void ValidationNullCheck(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationNullCheck(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationNullCheck(null, null);
            AssertRedo((commands, forms) => { });
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null)]
        public void ValidationError(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert_redo)
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationError(null, null);
            AssertRedo((commands, forms) => { });
            ParentAssertion(CommandList.ToList(), BaseFormList.ToList());
        }



    }
}
