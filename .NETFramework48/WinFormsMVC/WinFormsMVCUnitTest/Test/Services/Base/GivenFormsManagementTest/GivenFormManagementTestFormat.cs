using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    public class GivenFormManagementTestFormat
    {

        protected class CommandExecutionStatus
        {
            public bool WasValidation  = false;
            public bool WasInitialized = false;
            public bool WasFinalized = false;
            public bool WasError = false;
            public bool WasNext = false;
            public bool WasPrev = false;
            public bool WasRedo = false;
            private int execution_number = -1;

            public void Clear(int number)
            {
                if (execution_number != number)
                {
                    WasValidation = false;
                    WasInitialized = false;
                    WasFinalized = false;
                    WasError = false;
                    WasNext = false;
                    WasPrev = false;
                    WasRedo = false;
                    execution_number = number;
                }
            }

            public void AssertNotValidating()
            {
                Assert.IsFalse(WasValidation);
                Assert.IsFalse(WasInitialized);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsFalse(WasNext);
                Assert.IsFalse(WasPrev);
            }


            public void AssertValidationError()
            {
                Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasInitialized);
                Assert.IsTrue(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsFalse(WasNext);
                Assert.IsFalse(WasPrev);
            }

            public void AssertValidated()
            {
                Assert.IsTrue(WasValidation);
                Assert.IsTrue(WasInitialized);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsTrue(WasNext);
                Assert.IsFalse(WasPrev);
            }

            public void AssertWasRedo()
            {
                if (WasNext)
                {
                    Assert.IsTrue(WasRedo);
                }
                else
                {
                    Assert.IsFalse(WasRedo);
                }
            }

            public void AssertValidatedButNotTarget()
            {
                Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasInitialized);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsFalse(WasNext);
                Assert.IsFalse(WasPrev);
            }

            public void AssertUndo()
            {
                //Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                //Assert.IsTrue(WasNext);
                Assert.IsTrue(WasPrev);
            }

            public void AssertUndoButNotTarget()
            {
                //Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                //Assert.IsTrue(WasNext);
                Assert.IsFalse(WasPrev);
            }
        }


        protected CommandExecutionStatus CommonCommandStatus
        {
            get;
        }

        protected enum ActionMode
        {
            UNDEFINED,
            SIMPLE_ACTION,
            MEMORABLE_ACTION
        };

        protected ActionMode TestActionMode
        {
            get;
            set;
        }

        private List<BaseForm> ManagedFormList
        {
            get;
        }

        protected IEnumerable<BaseForm> BaseFormList
        {
            get
            {
                return ManagedFormList;
            }
        }

        private List<Command> OrderingCommands
        {
            get;
        }

        protected IEnumerable<Command> CommandList
        {
            get
            {
                return OrderingCommands;
            }
        }

        private GivenFormsManagement FormManager
        {
            get;
            set;
        }

        public GivenFormManagementTestFormat()
        {
            ManagedFormList = new List<BaseForm>();
            OrderingCommands = new List<Command>();
            CommonCommandStatus = new CommandExecutionStatus();
            TestActionMode = ActionMode.UNDEFINED;
        }

        protected void Define<T>(ref T instance, T default_instance) where T : class
        {
            if (instance == null)
            {
                instance = default_instance;
            }
        }

        protected Command CreateDefaultCommand<T>(BaseForm invoker, string validation_text) where T : BaseForm
        {
            return new GenericCommand<T, TextItem>()
            {
                Invoker = invoker,
                IsForSelf = true,
                Validation = (item) =>
                {
                    CommonCommandStatus.WasValidation = true;
                    item.Next = validation_text;
                    return true;
                },
                Preservation = (item, status, form1) =>
                {
                    CommonCommandStatus.WasInitialized = true;
                },
                NextOperation = ((item, status, form1) =>
                {
                    if (status.ExecutedCount != status.PreviousExecutedCount)
                    {
                        CommonCommandStatus.Clear(status.ExecutedCount);
                        CommonCommandStatus.WasValidation = true;
                        CommonCommandStatus.WasRedo = true;
                        CommonCommandStatus.WasInitialized = true;
                    }

                    CommonCommandStatus.WasNext = true;
                    item[form1] = form1.Text;
                    form1.Text = item.Next;
                }),
                PrevOperation = ((item, status, form1) =>
                {
                    CommonCommandStatus.WasPrev = true;
                    form1.Text = item[form1];
                }),
                Finalization = ((item) =>
                {
                    CommonCommandStatus.WasFinalized = true;
                }),
                ErrorHandle = ((item) =>
                {
                    CommonCommandStatus.WasError = true;
                })
            };
        }

        protected void UpdateForms(IEnumerable<BaseForm> forms)
        {
            ManagedFormList.Clear();
            ManagedFormList.AddRange(forms);
        }

        protected void UpdateCommands(IEnumerable<Command> commands)
        {
            OrderingCommands.Clear();
            OrderingCommands.AddRange(commands);

        }

        public GivenFormsManagement UseFormsManagement() 
        {
            if (FormManager == null)
            {
                FormManager = new GivenFormsManagement(ManagedFormList);
            }

            return FormManager;
            
            /*
            if (typeof(T) == typeof(GivenFormsManagement))
            {
                return (T)new GivenFormsManagement(ManagedFormList);
            } 
            else if (typeof(T) == typeof(WinFormsMVC.Services.FormsManagement))
            {
                return (T)(GivenFormsManagement)new WinFormsMVC.Services.FormsManagement();
            }
            else
            {
                throw new NotImplementedException();
            }
            */
        }

        protected virtual void AssertAction(
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

        }

        protected void AssertSimpleAction( 
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Assert.AreEqual(ActionMode.SIMPLE_ACTION, TestActionMode);

            modified( OrderingCommands, ManagedFormList);

            var form_management = UseFormsManagement();
            form_management.Run(OrderingCommands);

            assert(OrderingCommands, ManagedFormList);
        }

        protected void AssertMemorableAction(
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Assert.AreEqual(ActionMode.MEMORABLE_ACTION, TestActionMode);

            CommonCommandStatus.Clear(0);

            modified(OrderingCommands, ManagedFormList);

            var form_management = UseFormsManagement();
            form_management.RunAndRecord(OrderingCommands);

            assert(OrderingCommands, ManagedFormList);
        }

        protected virtual void AssertUndo(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Assert.AreEqual(ActionMode.MEMORABLE_ACTION, TestActionMode);

            var form_management = UseFormsManagement();
            form_management.Undo();

            assert(OrderingCommands, ManagedFormList);
        }

        protected virtual void AssertRedo(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Assert.AreEqual(ActionMode.MEMORABLE_ACTION, TestActionMode);

            var form_management = UseFormsManagement();
            form_management.Redo();

            assert(OrderingCommands, ManagedFormList);
        }

    }
}
