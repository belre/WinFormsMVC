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

namespace WinFormsMVCUnitTest.Test.Services.Base
{
    public class GivenFormManagementTestFormat
    {
        protected bool _was_validation = false;
        protected bool _was_finalize = false;
        protected bool _was_error = false;

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



        public GivenFormManagementTestFormat()
        {
            ManagedFormList = new List<BaseForm>();
            OrderingCommands = new List<Command>();
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
                    _was_validation = true;
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
                    _was_finalize = true;
                }),
                ErrorOperation = ((item) =>
                {
                    _was_error = true;
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

        public T ConstructFormsManagement<T>() where T : GivenFormsManagement
        {
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
        }


        protected void AssertAction<T> ( 
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert) where T : GivenFormsManagement
        {
            modified( OrderingCommands, ManagedFormList);

            var form_management = ConstructFormsManagement<T>();
            form_management.Run(OrderingCommands);

            assert(OrderingCommands, ManagedFormList);
        }

        protected void AssertMemorableAction<T>(
            Action<List<Command>, List<BaseForm>> modified,
            Action<GivenFormsManagement, IEnumerable<Command>, IEnumerable<BaseForm>> assert) where T : GivenFormsManagement
        {
            modified(OrderingCommands, ManagedFormList);

            var form_management = ConstructFormsManagement<T>();
            form_management.RunAndRecord(OrderingCommands);

            assert(form_management, OrderingCommands, ManagedFormList);
        }

        protected void AssertUndo<T>(Action<GivenFormsManagement, IEnumerable<Command>, IEnumerable<BaseForm>> assert) where T : GivenFormsManagement
        {
            var form_management = ConstructFormsManagement<T>();
            form_management.Undo();

            assert(form_management, OrderingCommands, ManagedFormList);
        }

    }
}
