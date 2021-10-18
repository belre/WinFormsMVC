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

        public void LaunchForm3(Form2 self_view)
        {
            _manager.LaunchForm(self_view, new Form3());
        }

        public void LaunchForm4(Form2 self_view)
        {
            _manager.LaunchForm(self_view, new Form4());
        }
        public void NotifyAsyncMessage()
        {

        }

        public void Undo(NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.ReflectPrevious();
            ReflectMemento(notify_undo_func);
        }

    }
}
