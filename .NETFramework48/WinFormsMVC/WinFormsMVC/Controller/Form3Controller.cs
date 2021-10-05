using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Main.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    public class Form3Controller : Controller
    {
        private FormManager _manager;
        private MementoManager _mementoManager;

        public Form3Controller(FormManager manager, MementoManager mementoManager)
        {
            _manager = manager;
            _mementoManager = mementoManager;
        }

        public void Test(Form3 form3)
        {
            _manager.LaunchForm(form3, new Form2(), _mementoManager);
        }
    }
}
