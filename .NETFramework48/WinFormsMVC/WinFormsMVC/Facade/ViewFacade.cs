using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Model.Services;
using WinFormsMVC.View;

namespace WinFormsMVC
{
    namespace Facade
    {
        public class ViewFacade
        {
            public IEnumerable<BaseForm> Forms { get; set; }

            public IEnumerable<Controller.Controller> Controllers { get; set; }

            public FormManager Manager { get; set; }

            public ViewFacade(FormManager manager)
            {
                Manager = manager;
            }

            public T GetController<T>(BaseForm form) where T : Controller.Controller
            {
                var inst = Activator.CreateInstance(typeof(T).Assembly.GetName().Name,
                    typeof(T).FullName, false,
                    BindingFlags.CreateInstance | BindingFlags.SetField, null,
                    new object[] {Manager}, CultureInfo.CurrentCulture, null);

                if (inst != null)
                {
                    return (T) inst.Unwrap();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
