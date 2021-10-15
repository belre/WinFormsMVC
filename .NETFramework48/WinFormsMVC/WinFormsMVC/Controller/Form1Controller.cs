using WinFormsMVC.Model.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    class Form1Controller : Controller
    {
        private FormManager _manager;

        public Form1Controller(FormManager manager)
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
