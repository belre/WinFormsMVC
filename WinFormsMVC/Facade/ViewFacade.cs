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

        /// <summary>
        /// モデルを追加する場合、ここで
        /// 宣言を追加し、プロパティを追加するなど対処します.
        /// </summary>
        /// <param name="form_manager"></param>
        public ViewFacade(FormsManagement form_manager)
        {
            FormManager = form_manager;
            FormManager.FacadeCore = this;


            // FormManager as Constructor Arguments
            ConstructorArgsTemplate = new Dictionary<Type, object>();
            ConstructorArgsTemplate[typeof(FormsManagement)] = FormManager;
        }
    }
}
