using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVC.Services;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form2Controller : CommandController
    {

        public Form2Controller(FormsManagement manager)
            : base(manager)
        {
        }

        public void NotifyAsyncMessage()
        {

        }

    }
}
