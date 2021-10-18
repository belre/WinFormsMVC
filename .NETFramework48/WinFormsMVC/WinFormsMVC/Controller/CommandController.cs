using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.Services;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    public class CommandController : BaseController
    {
        protected FormsManagement _manager;
        public delegate void NotifyIsAvailableUndo(bool is_enable);

        public bool IsAvailableUndo
        {
            get
            {
                return _manager.MementoManager.IsAvalableUndo();
            }
        }


        public CommandController(FormsManagement manager)
        {
            _manager = manager;
        }

        public void SendMessageWithRecord(Command[] abstractCommand, NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.Operate(abstractCommand, true);
            ReflectMemento(notify_undo_func);
        }

        public void SendSimpleMessage(Command[] abstractCommand, NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.Operate(abstractCommand, false);
            ReflectMemento(notify_undo_func);
        }

        public void SendAsyncMessage(Command[] abstractCommand, NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.OperateAsync(abstractCommand);
            ReflectMemento(notify_undo_func);
        }

        public void Undo(NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.ReflectPrevious();
            ReflectMemento(notify_undo_func);
        }

        public void Launch<T>(BaseForm self_form) where T : BaseForm
        {
            var create_instance = (T)typeof(T).InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
            _manager.LaunchForm(self_form, create_instance);
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
