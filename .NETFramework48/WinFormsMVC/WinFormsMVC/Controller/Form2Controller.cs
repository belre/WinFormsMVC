using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Main.Command;
using WinFormsMVC.Main.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    public class Form2Controller : Controller
    {
        private FormManager _manager;
        private OperationManager _operation_manager;

        public Form2Controller(FormManager manager, OperationManager operation_manager)
        {
            _manager = manager;
            _operation_manager = operation_manager;
        }

        public void LaunchForm3(Form2 self_view)
        {
            var forms = new Form3();
            _manager.LaunchForm(self_view, forms);
        }

        public void SendMessage(Command command)
        {
            _manager.Operate<Form3>(command);
            _operation_manager.MememtoCommand.Push(command);

        }

        public void Redo()
        {
            var command = _operation_manager.PopRedoCommand();
            _manager.OperatePrevious<Form3>(command);
        }
    }
}
