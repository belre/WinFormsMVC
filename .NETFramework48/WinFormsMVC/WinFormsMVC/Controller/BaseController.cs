using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Controller.Attribute;

namespace WinFormsMVC.Controller
{
    public class BaseController
    {
        public static ConstructorInfo GetRuntimeConstructor(Type controller_type)
        {
            var ctors = controller_type.GetConstructors();
            var target_ctors = new List<ConstructorInfo>();

            // 型チェック
            // CalledAsController Attributeはどのようなクラスにも常に1個定義されている必要がある.
            foreach (var ctor in ctors)
            {
                if (ctor.CustomAttributes.Any(data => { return data.AttributeType == typeof(CalledAsController); }))
                {
                    target_ctors.Add(ctor);
                }
            }

            if (target_ctors.Count > 1)
            {
                return null;
            }
            else if(target_ctors.Count == 1)
            {
                return target_ctors.First();
            }
            else
            {
                return ctors.First();
            }
        }


        public BaseController()
        {
            var target_ctor = GetRuntimeConstructor(this.GetType());

            if (target_ctor == null)
            {
                throw new TypeInitializationException(this.GetType().Name, new Exception("Controllerの型指定異常です"));
            }
        }   
    }
}
