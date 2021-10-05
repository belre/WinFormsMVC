using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Model.Command;
using WinFormsMVC.Model.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    public class Form2Controller : Controller
    {
        private FormManager _manager;

        public Form2Controller(FormManager manager)
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

        public void SendMessage<TargetForm>(AbstractCommand abstractCommand) where TargetForm : BaseForm
        {
            _manager.Operate<TargetForm>(abstractCommand);
        }

        public void Redo()
        {
            _manager.OperatePrevious();
        }
    }
}
