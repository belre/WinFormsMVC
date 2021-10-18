using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVC.Services;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form2Controller : BaseController
    {
        public delegate void NotifyIsAvailableUndo(bool is_enable);

        private FormsManagement _manager;

        public bool IsAvailableUndo
        {
            get
            {
                return _manager.MementoManager.IsAvalableUndo();
            }
        }

        public Form2Controller(FormsManagement manager)
        {
            _manager = manager;
        }

        public void LaunchForm3(Form2 self_view)
        {
            _manager.LaunchForm(self_view, new Form3());
        }

        public void LaunchForm4(Form2 self_view)
        {
            _manager.LaunchForm(self_view, new Form4());
        }

        public void SendMessageWithRecord(AbstractCommand[] abstractCommand, NotifyIsAvailableUndo notify_undo_func) 
        {
            _manager.Operate(abstractCommand, true);
            ReflectMemento(notify_undo_func);
        }

        public void SendSimpleMessage(AbstractCommand[] abstractCommand, NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.Operate(abstractCommand, false);
            ReflectMemento(notify_undo_func);
        }

        public void SendAsyncMessage(AbstractCommand[] abstractCommand, NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.OperateAsync(abstractCommand);
            ReflectMemento(notify_undo_func);
        }

        public void NotifyAsyncMessage()
        {

        }

        public void Undo(NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.ReflectPrevious();
            ReflectMemento(notify_undo_func);
        }

        public void ReflectMemento(NotifyIsAvailableUndo notify_undo_func)
        {
            if (notify_undo_func != null)
            {
                notify_undo_func(IsAvailableUndo);
            }
        }

    }
}
