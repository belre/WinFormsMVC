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
        public class ChildForm1 : BaseForm
        {

        }

        public class ChildForm2 : BaseForm
        {

        }

        public class ChildForm3 : BaseForm
        {

        }

        protected bool _was_validation = false;
        protected bool _was_finalize = false;
        protected bool _was_error = false;

        private List<BaseForm> FormList
        {
            get;
        }

        private List<Command> DefaultCommands
        {
            get;
        }

        public GivenFormManagementTestFormat()
        {
            FormList = new List<BaseForm>();
            DefaultCommands = new List<Command>();
        }

        protected Command CreateDefaultCommand(BaseForm invoker, string validation_text)
        {
            return new GenericCommand<BaseForm, TextItem>()
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
            FormList.Clear();
            FormList.AddRange(forms);
        }

        protected void UpdateCommands(IEnumerable<Command> commands)
        {
            DefaultCommands.Clear();
            DefaultCommands.AddRange(commands);
        }

        public T ConstructFormsManagement<T>() where T : GivenFormsManagement
        {
            if (typeof(T) == typeof(GivenFormsManagement))
            {
                return (T)new GivenFormsManagement(FormList);
            } 
            else if (typeof(T) == typeof(FormsManagement))
            {
                return (T)(GivenFormsManagement)new FormsManagement();
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        protected void AssertForms<T> ( 
            Action<List<Command>, List<BaseForm>> input, 
            Action<T, List<BaseForm>> launcher,
            Action<List<Command>, List<BaseForm>> assert) where T : GivenFormsManagement
        {
            input( DefaultCommands, FormList);

            var form_management = ConstructFormsManagement<T>();
            if (launcher != null)
            {
                launcher(form_management, FormList);
            }
            form_management.Run(DefaultCommands);

            assert(DefaultCommands, FormList);
        }
    }
}
