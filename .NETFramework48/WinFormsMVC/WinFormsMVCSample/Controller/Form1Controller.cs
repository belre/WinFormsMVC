using System;
using WinFormsMVC.Services;
using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVCSample.Controller;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form1Controller : CommandController
    {

        public Form1Controller(FormsManagement manager)
            : base(manager)
        {
        }

    }
}
