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
