using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.Services;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;

namespace WinFormsMVC.Controller
{
    /// <summary>
    /// コマンドを取り扱うController
    /// </summary>
    public class CommandController : BaseController
    {
        /// <summary>
        /// フォームの管理オブジェクトです。
        /// </summary>
        protected FormsManagement _manager;

        /// <summary>
        /// Undoの処理を通知するためのDelagateです。
        /// </summary>
        /// <param name="is_enable"></param>
        public delegate void AsNotifiedAfterSomeAction(bool is_available_undo, bool is_available_redo);

        /// <summary>
        /// いま「元に戻す」の動作が可能であるかを表します.
        /// </summary>
        public bool IsAvailableUndo
        {
            get
            {
                return _manager.ManagedMemento.IsAvalableUndo();
            }
        }

        public bool IsAvailableRedo
        {
            get
            {
                return _manager.ManagedMemento.IsAvailableRedo();
            }
        }

        /// <summary>
        /// 既定のmanagerを入力してControllerを生成します。
        /// </summary>
        /// <param name="manager"></param>
        public CommandController(FormsManagement manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// 履歴を記録してコマンドを送信します。
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="notifyAfterSomeAction"></param>
        public void SendStoredMessage(Command[] commands, AsNotifiedAfterSomeAction notifyAfterSomeAction)
        {
            _manager.RunAndRecord(commands);
            ReflectMemento(notifyAfterSomeAction);
        }

        /// <summary>
        /// 履歴を記録せずにコマンドを送信します。
        /// </summary>
        /// <param name="commands"></param>
        public void SendSimpleMessage(Command[] commands)
        {
            _manager.Run(commands);
        }

        public void Undo(AsNotifiedAfterSomeAction asNotifiedUndoFunc)
        {
            _manager.Undo();
            ReflectMemento(asNotifiedUndoFunc);
        }


        public void Redo(AsNotifiedAfterSomeAction asNotifiedUndoFunc)
        {
            _manager.Redo();
            ReflectMemento(asNotifiedUndoFunc);
        }

        public void Launch<T>(BaseForm self_form) where T : BaseForm
        {
            Launch<T>(self_form, null, null);
        }

        public void Launch<T>(BaseForm self_form, Action<T> preload) where T : BaseForm
        {
            Launch<T>(self_form, null, preload);
        }

        public void Launch<T>(BaseForm self_form, FormClosedEventHandler on_closed, Action<T> preload) where T : BaseForm
        {
            var create_instance = (T)typeof(T).InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
            if (preload != null)
            {
                preload(create_instance);
            }
            create_instance.FormClosed += on_closed;

            _manager.LaunchForm(self_form, create_instance, false);
        }

        public void LaunchWithLock<T>(BaseForm self_form) where T : BaseForm
        {
            LaunchWithLock<T>(self_form, null, null);
        }

        public void LaunchWithLock<T>(BaseForm self_form, Action<T> preload) where T : BaseForm
        {
            LaunchWithLock<T>(self_form, null, preload);
        }

        public void LaunchWithLock<T>(BaseForm self_form, FormClosedEventHandler on_closed, Action<T> preload) where T : BaseForm
        {
            var create_instance = (T)typeof(T).InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
            if (preload != null)
            {
                preload(create_instance);
            }
            create_instance.FormClosed += on_closed;

            _manager.LaunchForm(self_form, create_instance, true);
        }

        private void ReflectMemento(AsNotifiedAfterSomeAction asNotifiedUndoFunc)
        {
            if (asNotifiedUndoFunc != null)
            {
                asNotifiedUndoFunc(IsAvailableUndo, IsAvailableRedo);
            }
        }
    }
}
