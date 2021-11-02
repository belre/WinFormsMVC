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
    /// <summary>
    /// 全てのControllerクラスのベースクラスを表します。
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// リフレクションを使って、Controllerのコンストラクタを返します。
        /// ・CalledAsController属性が指定されているコンストラクタを優先して生成する
        /// ・CalledAsController属性がコンストラクタに複数指定されている場合は、nullを返却する
        /// ・CalledAsController属性が指定されていない場合は、最初に指定されたコンストラクタを生成する。
        /// </summary>
        /// <param name="controller_type">Controllerの型</param>
        /// <returns>コンストラクタの型</returns>
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
            
            // 抽出したオブジェクト一覧に従ってRuntime用のコンストラクタを出力
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
                if (ctors.Length == 0)
                {
                    return null;
                }
                else
                {
                    return ctors.First();
                }
            }
        }

        /// <summary>
        /// コンストラクタを表します。
        /// </summary>
        public BaseController()
        {
            var target_ctor = GetRuntimeConstructor(this.GetType());

            if (target_ctor == null)
            {
                throw new TypeInitializationException(this.GetType().Name, new Exception("Controllerの型指定が異常です."));
            }
        }   
    }
}
