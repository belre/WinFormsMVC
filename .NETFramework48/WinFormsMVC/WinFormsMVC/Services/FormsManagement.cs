using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Facade;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVC.Services
{
    /// <summary>
    /// BaseFormを管理します。
    /// </summary>
    public class FormsManagement
    {
        /// <summary>
        /// 管理されているBaseFormの一覧です。
        /// </summary>
        private List<BaseForm> _managed_baseform;

        /// <summary>
        /// BaseFormとControllerを接続する窓口役(Facade)を表します。
        /// </summary>
        private ViewFacade _facade;

        /// <summary>
        /// 管理しているコマンド履歴です。
        /// </summary>
        private CommandMemento _memento;

        /// <summary>
        /// 窓口を表すクラスです。
        /// </summary>
        public ViewFacade Facade
        {
            get { return _facade; }
            set { _facade = value; }
        }

        /// <summary>
        /// コマンド履歴のオブジェクトです。
        /// </summary>
        public CommandMemento MementoManager
        {
            get
            {
                return _memento;
            }
        }

        /// <summary>
        /// Form管理を生成します。
        /// </summary>
        public FormsManagement()
        {
            _managed_baseform = new List<BaseForm>();
            _memento = new CommandMemento();
        }

        /// <summary>
        /// 新しいフォームを立ち上げます。
        /// </summary>
        /// <typeparam name="TargetForm"></typeparam>
        /// <param name="source">どのフォームが立ち上げるか</param>
        /// <param name="target">なんのフォームを立ち上げるか</param>
        public void LaunchForm<TargetForm>(BaseForm source, TargetForm target)
            where TargetForm : BaseForm
        {
            _managed_baseform.Add(target);
            target.Invoker = source;
            target.Facade = _facade;
            target.Closed += OnFormClosed;
            //OperateFromInit(target);
            target.Show();
        }

        /// <summary>
        /// コマンド履歴に従って、フォームを更新します。
        /// </summary>
        /// <param name="abstract_command"></param>
        /// <param name="is_record_memento"></param>
        public void Operate(IEnumerable<Request.Command> abstract_command, bool is_record_memento)
        {
            foreach (var command in abstract_command)
            {
                if (command.Validate())
                {
                    ReflectNext(command);
                }
                else
                {
                    command.HandleValidationError();
                }
            }

            if (is_record_memento)
            {
                _memento.PushCommand(abstract_command);
            }
        }


        /// <summary>
        /// コマンド履歴に従って、フォームを更新します。
        /// なお、非同期で処理を実行します。
        /// </summary>
        /// <param name="abstract_command"></param>
        public void OperateAsync(IEnumerable<Request.Command> abstract_command)
        {
            foreach (var command in abstract_command)
            {
                var async_command = new AsyncCommand(command);

                async_command.NotifyingAsync += ReflectNext;
                Task.Run(async_command.ValidateAsync);
            }
        }

        /// <summary>
        /// 新たに立ち上げたフォームを、履歴に従って更新します。
        /// </summary>
        /// <param name="target"></param>
        public void OperateFromInit(BaseForm target)
        {
            foreach (var recent_commands in _memento.Mememtoes)
            {
                foreach (var command in recent_commands)
                {
                    bool is_match_invoker = false;
                    if (command.IsForSelf)
                    {
                        is_match_invoker = target == command.Invoker;
                    }
                    else if (command.IsRetrieved)
                    {
                        is_match_invoker = target.IsOriginatingFromTarget(command.Invoker);
                    }
                    else
                    {
                        is_match_invoker = target.Invoker == command.Invoker;
                    }

                    if (is_match_invoker && target.GetType() == command.FormType)
                    {
                        command.Next(target);
                    }
                }
            }
        }

        /// <summary>
        /// Memento一覧に従ってフォームを更新します。
        /// </summary>
        /// <param name="command"></param>
        public void ReflectNext(Command command)
        {
            var target_forms = new List<BaseForm>();
            foreach (var form in _managed_baseform)
            {
                bool is_match_invoker = false;
                if (command.IsForSelf)
                {
                    is_match_invoker = form == command.Invoker;
                }
                else if (command.IsRetrieved)
                {
                    is_match_invoker = form.IsOriginatingFromTarget(command.Invoker);
                }
                else
                {
                    is_match_invoker = form.Invoker == command.Invoker;
                }

                if (is_match_invoker && form.GetType() == command.FormType)
                {
                    target_forms.Add(form);
                }
            }

            foreach (var target in target_forms)
            {
                command.Next(target);
            }
        }

        /// <summary>
        /// Memento一覧に従って、フォームを戻します。
        /// </summary>
        public void ReflectPrevious()
        {
            var recent_commands = _memento.PopCommand();

            if (recent_commands == null)
            {
                return;
            }

            foreach (var command in recent_commands)
            {
                foreach (var form in _managed_baseform)
                {
                    bool is_match_invoker = false;
                    if (command.IsForSelf)
                    {
                        is_match_invoker = form == command.Invoker;
                    }
                    else if (command.IsRetrieved)
                    {
                        is_match_invoker = form.IsOriginatingFromTarget(command.Invoker);
                    }
                    else
                    {
                        is_match_invoker = form.Invoker == command.Invoker;
                    }

                    if (is_match_invoker && form.GetType() == command.FormType)
                    {
                        command.Prev(form);
                        command.Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// フォームが閉じられた時に自動的に実行されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnFormClosed(object sender, EventArgs e)
        {
            // 自分自身
            BaseForm form = (BaseForm) sender;

            // 子フォームを探す
            var children_form = new List<BaseForm>();
            foreach (var any_form in _managed_baseform)
            {
                if (any_form.Invoker == form)
                {
                    children_form.Add(any_form);
                }
            }

            // 削除
            _managed_baseform.Remove(form);
            foreach (var child in children_form)
            {
                child.Close();
                _managed_baseform.Remove(child);
            }
        }
    }
}
