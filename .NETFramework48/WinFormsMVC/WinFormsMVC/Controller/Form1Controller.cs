using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Main.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    class Form1Controller : Controller
    {
        private FormManager _manager;
        private OperationManager _operation_manager;

        public Form1Controller(FormManager manager, OperationManager operation_manager)
        {
            _manager = manager;
            _operation_manager = operation_manager;
        }

        public void LaunchForm2(Form1 self_form)
        {
            var forms = new Form2();
            _manager.LaunchForm(self_form, forms, _operation_manager);
        }
    }
}
