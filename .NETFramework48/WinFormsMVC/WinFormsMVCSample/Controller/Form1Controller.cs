using System;
using WinFormsMVC.Services;
using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVCSample.Controller;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    class Form1Controller : BaseController
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

        public Form1Controller(FormsManagement manager)
        {
            _manager = manager;
        }


        public void LaunchForm2(Form1 self_form)
        {
            var forms = new Form2();
            _manager.LaunchForm(self_form, forms);
        }

        public void SendMessageWithRecord(AbstractCommand[] abstractCommand, NotifyIsAvailableUndo notify_undo_func)
        {
            _manager.Operate(abstractCommand, true);
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
