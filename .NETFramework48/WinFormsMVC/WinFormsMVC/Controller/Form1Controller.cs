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
        private MementoManager _mementoManager;

        public Form1Controller(FormManager manager, MementoManager mementoManager)
        {
            _manager = manager;
            _mementoManager = mementoManager;
        }

        public void LaunchForm2(Form1 self_form)
        {
            var forms = new Form2();
            _manager.LaunchForm(self_form, forms, _mementoManager);
        }
    }
}
