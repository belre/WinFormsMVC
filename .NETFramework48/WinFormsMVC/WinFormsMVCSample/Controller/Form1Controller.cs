using WinFormsMVC.Services;
using WinFormsMVC.Controller;
using WinFormsMVCSample.Controller;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    class Form1Controller : BaseController
    {
        private FormsManagement _manager;

        public Form1Controller(FormsManagement manager)
        {
            _manager = manager;
        }

        public void LaunchForm2(Form1 self_form)
        {
            var forms = new Form2();
            _manager.LaunchForm(self_form, forms);
        }
    }
}
