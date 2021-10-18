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
    public class FormsManagement
    {
        private List<BaseForm> _managed_baseform;
        private ViewFacade _facade;
        private CommandMementoManagement _memento_management;

        public ViewFacade Facade
        {
            get { return _facade; }
            set { _facade = value; }
        }

        public CommandMementoManagement MementoManager
        {
            get
            {
                return _memento_management;
            }
        }

        public FormsManagement()
        {
            _managed_baseform = new List<BaseForm>();
            _memento_management = new CommandMementoManagement();
        }

        public void LaunchForm<TargetForm>(BaseForm source, TargetForm target)
            where TargetForm : BaseForm
        {
            _managed_baseform.Add(target);
            target.Invoker = source;
            target.Facade = _facade;
            target.Closed += OnFormClosed;
            OperateFromInit(target);
            target.Show();
        }

        public void Operate(IEnumerable<Request.AbstractCommand> abstract_command, bool is_record_memento)
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
                _memento_management.PushCommand(abstract_command);
            }
        }

        public void OperateAsync(IEnumerable<Request.AbstractCommand> abstract_command)
        {
            foreach (var command in abstract_command)
            {
                var async_command = new AsyncCommand(command);

                async_command.NotifyingAsync += ReflectNext;
                Task.Run(async_command.ValidateAsync);
            }
        }


        public void OperateFromInit(BaseForm target)
        {
            foreach (var recent_commands in _memento_management.MememtoCommand)
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

        public void ReflectNext(AbstractCommand command)
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


        public void ReflectPrevious()
        {
            var recent_commands = _memento_management.PopCommand();

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
                        command.Finalize();
                    }
                }
            }
        }


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
