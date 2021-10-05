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

        public Form2Controller(FormManager manager)
        {
            _manager = manager;
        }

        public void LaunchForm3(Form2 self_view)
        {
            var forms = new Form3();
            _manager.LaunchForm(self_view, forms);
        }

        public void SendMessage(Command command)
        {
            _manager.Operate<Form3>(command);
        }

        public void Redo()
        {
            _manager.OperatePrevious<Form3>();
        }
    }
}
