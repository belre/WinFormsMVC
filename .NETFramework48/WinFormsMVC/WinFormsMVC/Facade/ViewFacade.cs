using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Facade
{
    public class ViewFacade
    {
        public IEnumerable<BaseForm> Forms { get; set; }

        public IEnumerable<Controller.BaseController> Controllers { get; set; }

        public FormsManagement FormManager { get; }

        public ViewFacade(FormsManagement form_manager)
        {
            FormManager = form_manager;
            FormManager.Facade = this;
        }

        public T GetController<T>(BaseForm form) where T : Controller.BaseController
        {
            var inst = Activator.CreateInstance(typeof(T).Assembly.GetName().Name,
                typeof(T).FullName, false,
                BindingFlags.CreateInstance | BindingFlags.SetField, null,
                new object[] { FormManager }, CultureInfo.CurrentCulture, null);

            if (inst != null)
            {
                return (T)inst.Unwrap();
            }
            else
            {
                return null;
            }
        }
    }
}
