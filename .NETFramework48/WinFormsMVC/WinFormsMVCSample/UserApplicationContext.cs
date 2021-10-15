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
 
        protected ViewFacade _facade;
        protected T _root_form;

        public UserApplicationContext()
        {
            try
            {
                _form_manager = new FormsManagement();
                _facade = new ViewFacade(_form_manager);

                _facade.Forms = new List<BaseForm>()
                {
                    new Form1(),
                    new Form2(),
                    new Form3()
                };
                _facade.Controllers = new List<BaseController>()
                {
                    new Form1Controller(_form_manager),
                    new Form2Controller(_form_manager),
                    new Form3Controller(_form_manager)
                };

                _root_form = (T)typeof(T).GetConstructor(new Type[0]).Invoke(new object[0]);
                _root_form.Closed += new EventHandler(OnFormClosed);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("例外が発生しました：\n{0}\n詳細は発注元に問い合わせください。\n", e.Message));
            }

            _form_manager.LaunchForm(null, _root_form);
        }

        protected void OnFormClosed(object sender, EventArgs e)
        {
            ExitThread();
        }
    }
}
