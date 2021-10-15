using WinFormsMVC.Controller;
using WinFormsMVC.Services;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form3Controller : BaseController
    {
        private FormsManagement _manager;

        public Form3Controller(FormsManagement manager)
        {
            _manager = manager;
        }

        public void RenewWindow(Form3 form3)
        {
            _manager.LaunchForm(form3, new Form2());
        }
    }
}
