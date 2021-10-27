using System;
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
    /// <summary>
    /// Viewから、Controllerの操作を行ないます。
    /// </summary>
    public class ViewFacadeCore
    {
        /// <summary>
        /// 現在管理しているオブジェクトの一覧です.
        /// </summary>
        protected Dictionary<Type, object> ConstructorArgsTemplate;


        /// <summary>
        /// コンストラクタの引数で該当しているものを
        /// あてはめます。
        /// </summary>
        /// <param name="constructor_info"></param>
        /// <returns></returns>
        protected object[] BindArguments(ConstructorInfo constructor_info)
        {
            var arguments = new object[constructor_info.GetParameters().Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                var param_type = constructor_info.GetParameters()[i].ParameterType;

                if (ConstructorArgsTemplate.ContainsKey(param_type))
                {
                    arguments[i] = ConstructorArgsTemplate[param_type];
                }
            }

            return arguments;
        }

        /// <summary>
        /// コントローラを取得します。
        /// </summary>
        /// <typeparam name="T">Controllerの型</typeparam>
        /// <param name="form">Form(通常は自分自身 *this*などを返す)</param>
        /// <returns>Controllerのオブジェクト</returns>
        public T GetController<T>(BaseForm form) where T : Controller.BaseController
        {
            // 想定しているクラスの追加
            var ctor = BaseController.GetRuntimeConstructor(typeof(T));

            // Controllerを動的に生成
            var inst = Activator.CreateInstance(typeof(T).Assembly.GetName().Name,
                typeof(T).FullName, false,
                BindingFlags.CreateInstance | BindingFlags.SetField, null,
                BindArguments(ctor), CultureInfo.CurrentCulture, null);

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
