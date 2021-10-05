using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Main.Services;
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

            public MementoManager Mementoes { get; set; }

            public ViewFacade(FormManager manager, MementoManager mementoes)
            {
                Manager = manager;
                Mementoes = mementoes;
            }

            public T GetController<T>(BaseForm form) where T : Controller.Controller
            {
                var inst = Activator.CreateInstance(typeof(T).Assembly.GetName().Name,
                        typeof(T).FullName, false,
                        BindingFlags.CreateInstance | BindingFlags.SetField, null,
                        new object[] {Manager, Mementoes}, CultureInfo.CurrentCulture, new object[] { })
                    .Unwrap();

                if (inst != null)
                {
                    return (T) inst;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
