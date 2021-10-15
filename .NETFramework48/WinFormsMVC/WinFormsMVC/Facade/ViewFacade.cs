﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Controller;
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
            // 想定しているクラスの追加
            var ctor = BaseController.GetRuntimeConstructor(typeof(T));
            var arguments = new object[ctor.GetParameters().Length];
            for (int i = 0 ; i < arguments.Length; i ++ )
            {
                if (ctor.GetParameters()[i].ParameterType == typeof(FormsManagement))
                {
                    arguments[i] = FormManager;
                }
            }
            

            // Controllerを動的に生成
            var inst = Activator.CreateInstance(typeof(T).Assembly.GetName().Name,
                typeof(T).FullName, false,
                BindingFlags.CreateInstance | BindingFlags.SetField, null,
                arguments, CultureInfo.CurrentCulture, null);

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
