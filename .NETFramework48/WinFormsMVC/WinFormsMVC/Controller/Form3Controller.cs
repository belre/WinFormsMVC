using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Model.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    public class Form3Controller : Controller
    {
        private FormManager _manager;

        public Form3Controller(FormManager manager)
        {
            _manager = manager;
        }

        public void RenewWindow(Form3 form3)
        {
            _manager.LaunchForm(form3, new Form2());
        }
    }
}
