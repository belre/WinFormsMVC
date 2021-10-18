using System;
using WinFormsMVC.Services;
using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVCSample.Controller;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    class Form1Controller : CommandController
    {

        public Form1Controller(FormsManagement manager)
            : base(manager)
        {
        }


        public void LaunchForm2(Form1 self_form)
        {
            var forms = new Form2();
            _manager.LaunchForm(self_form, forms);
        }
    }
}
