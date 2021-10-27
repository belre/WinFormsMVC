using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Services;

namespace WinFormsMVC.Facade
{
    public class ViewFacade : ViewFacadeCore
    {
        /// <summary>
        /// 現在管理しているFormの管理オブジェクトを返します。
        /// </summary>
        protected FormsManagement FormManager { get; }

        public ViewFacade(FormsManagement form_manager)
        {
            FormManager = form_manager;
            FormManager.FacadeCore = this;

            ConstructorArgsTemplate = new Dictionary<Type, object>();
            ConstructorArgsTemplate[typeof(FormsManagement)] = FormManager;
        }

    }
}
