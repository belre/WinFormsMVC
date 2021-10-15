using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVC.Services;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form2Controller : BaseController
    {
        private FormsManagement _manager;

        public Form2Controller(FormsManagement manager)
        {
            _manager = manager;
        }

        public void LaunchForm3(Form2 self_view)
        {
            _manager.LaunchForm(self_view, new Form3());
        }

        public void LaunchForm4(Form2 self_view)
        {
            _manager.LaunchForm(self_view, new Form4());
        }

        public void SendMessage(AbstractCommand[] abstractCommand) 
        {
            _manager.Operate(abstractCommand);
        }

        public void Redo()
        {
            _manager.OperatePrevious();
        }
    }
}
