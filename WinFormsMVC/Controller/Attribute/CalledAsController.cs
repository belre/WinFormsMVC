using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Controller.Attribute
{
    /// <summary>
    /// Controllerのコンストラクタを規定するための属性
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Constructor, AllowMultiple=false)]
    public class CalledAsController : System.Attribute
    {
    }
}
