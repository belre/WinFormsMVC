using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public sealed class SimplyConnectedGivenFormsUndoAndRedoTest : SimplyConnectedGivenFormsTest, IRedoTest
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
        public  void CalledBySelf_RootInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf_RootInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf_RootInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }


        [TestMethod, TestCategory("差分")]
        
       
        public void CalledByRootInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByRootInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByRootInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }


        [TestMethod, TestCategory("差分")]
        
       
        public  void CalledBySelf_LastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledBySelf_LastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledBySelf_LastInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }

        [TestMethod, TestCategory("差分")]
        
       
        public  void CalledByLastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByLastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByLastInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }

        [TestMethod, TestCategory("差分")]
        
       
        public  void CalledByFirstAndLastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByFirstAndLastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByFirstAndLastInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }

        [TestMethod, TestCategory("差分")]
        
       
        public  void CalledByNullInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.CalledByNullInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.CalledByNullInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }

        [TestMethod, TestCategory("差分")]
        
       
        public  void RecursiveFromRootInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromRootInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.RecursiveFromRootInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }


        [TestMethod, TestCategory("差分")]
        
       
        public  void RecursiveFromLastInvoker()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.RecursiveFromLastInvoker(null, null);
            AssertUndo(((commands, forms) => { }));

            base.RecursiveFromLastInvoker(null, null);
            AssertRedo(((commands, forms) => CommonCommandStatus.AssertWasRedo()));
            ParentAssertion(CommandList, BaseFormList);
        }

        [TestMethod, TestCategory("差分")]
        
       
        public  void ValidationError()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;

            base.ValidationError(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationError(null, null);
            AssertRedo(((commands, forms) => { }));
            ParentAssertion(CommandList, BaseFormList);
        }

        [TestMethod, TestCategory("差分")]
        
       
        public  void ValidationNullCheck()
        {
            TestActionMode = ActionMode.MEMORABLE_ACTION;


            base.ValidationNullCheck(null, null);
            AssertUndo(((commands, forms) => { }));

            base.ValidationNullCheck(null, null);
            AssertRedo(((commands, forms) => { }));
            ParentAssertion(CommandList, BaseFormList);

        }
    }
}
