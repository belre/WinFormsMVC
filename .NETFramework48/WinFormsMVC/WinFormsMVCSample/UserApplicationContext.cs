using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMVC.Controller;
using WinFormsMVC.Facade;
using WinFormsMVC.Services;
using WinFormsMVC.View;
using WinFormsMVCSample.View;
using WinFormsMVCSample.Controller;


namespace WinFormsMVCSample
{
    public class UserApplicationContext<T> : ApplicationContext where T : BaseForm
    {
        protected FormsManagement _form_manager;
 
        protected ViewFacadeCore FacadeCore;
        protected T _root_form;

        public UserApplicationContext()
        {
            // 初期化
            try
            {
                _form_manager = new FormsManagement();
                FacadeCore = new ViewFacade(_form_manager);

                _root_form = (T)typeof(T).GetConstructor(new Type[0]).Invoke(new object[0]);
                _root_form.Closed += new EventHandler(OnFormClosed);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("例外が発生しました：\n{0}\n詳細は発注元に問い合わせください。\n", e.Message));
            }

            // フォーム生成
            _form_manager.LaunchForm(null, _root_form, false);
        }

        protected void OnFormClosed(object sender, EventArgs e)
        {
            ExitThread();
        }
    }
}
