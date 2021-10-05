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
        private MementoManager _mementoManager;

        public Form2Controller(FormManager manager, MementoManager mementoManager)
        {
            _manager = manager;
            _mementoManager = mementoManager;
        }

        public void LaunchForm3(Form2 self_view)
        {
            var forms = new Form3();
            _manager.LaunchForm(self_view, forms, _mementoManager);
        }

        public void SendMessage(Command command)
        {
            _manager.Operate<Form3>(command);
            _mementoManager.PushRedoCommand(command);
        }

        public void Redo()
        {
            var command = _mementoManager.PopUndoCommand();
            _manager.OperatePrevious<Form3>(command);
        }
    }
}
