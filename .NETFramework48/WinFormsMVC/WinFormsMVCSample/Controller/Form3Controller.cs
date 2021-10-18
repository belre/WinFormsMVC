using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Services;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form3Controller : CommandController
    {
        public Form3Controller(FormsManagement manager)
            : base(manager)
        {

        }


        public void RenewWindow(Form3 form3)
        {
            _manager.LaunchForm(form3, new Form2());
        }

    }
}
