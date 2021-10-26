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
            public bool WasFinalized = false;
            public bool WasError = false;
            public bool WasNext = false;
            public bool WasPrev = false;

            public void Clear()
            {
                WasValidation = false;
                WasFinalized = false;
                WasError = false;
                WasNext = false;
                WasPrev = false;
            }

            public void AssertNotValidating()
            {
                Assert.IsFalse(WasValidation);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsFalse(WasNext);
                Assert.IsFalse(WasPrev);
            }


            public void AssertError()
            {
                Assert.IsTrue(WasValidation);
                Assert.IsTrue(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsFalse(WasNext);
                Assert.IsFalse(WasPrev);
            }

            public void AssertValidated()
            {
                Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsTrue(WasNext);
                Assert.IsFalse(WasPrev);
            }


            public void AssertValidatedButNotTarget()
            {
                Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasError);
                Assert.IsFalse(WasFinalized);
                Assert.IsFalse(WasNext);
                Assert.IsFalse(WasPrev);
            }

            public void AssertUndo()
            {
                //Assert.IsTrue(WasValidation);
                Assert.IsFalse(WasError);
                Assert.IsTrue(WasFinalized);
                //Assert.IsTrue(WasNext);
                Assert.IsTrue(WasPrev);
            }
        }

        protected CommandExecutionStatus CommonCommandStatus
        {
            get;
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
        }

        protected Command CreateDefaultCommand<T>(BaseForm invoker, string validation_text) where T : BaseForm
        {
            return new GenericCommand<T, TextItem>()
            {
                Invoker = invoker,
                IsForSelf = true,
                Validation = (item) =>
                {
                    item.Next = validation_text;
                    CommonCommandStatus.WasValidation = true;
                    return true;
                },
                NextOperation = ((item, form1) =>
                {
                    item[form1] = item.Next;
                    form1.Text = item.Next;
                }),
                PrevOperation = ((item, form1) =>
                {
                    form1.Text = item[form1];
                }),
                FinalOperation = ((item) =>
                {
                    CommonCommandStatus.WasFinalized = true;
                }),
                ErrorOperation = ((item) =>
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


        protected void AssertAction( 
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            modified( OrderingCommands, ManagedFormList);

            var form_management = UseFormsManagement();
            form_management.Run(OrderingCommands);

            assert(OrderingCommands, ManagedFormList);
        }

        protected void AssertMemorableAction(
            Action<List<Command>, List<BaseForm>> modified,
            Action<GivenFormsManagement, IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            CommonCommandStatus.Clear();

            modified(OrderingCommands, ManagedFormList);

            var form_management = UseFormsManagement();
            form_management.RunAndRecord(OrderingCommands);

            assert(form_management, OrderingCommands, ManagedFormList);
        }

        protected void AssertUndo(Action<GivenFormsManagement, IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            var form_management = UseFormsManagement();
            form_management.Undo();

            assert(form_management, OrderingCommands, ManagedFormList);
        }

    }
}
