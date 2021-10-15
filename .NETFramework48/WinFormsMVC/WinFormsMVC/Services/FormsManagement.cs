using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Facade;
using WinFormsMVC.View;

namespace WinFormsMVC.Services
{
    public class FormsManagement
    {
        private List<BaseForm> _managed_baseform;
        private ViewFacade _facade;
        private CommandMementoManagement _mementManagement;

        public ViewFacade Facade
        {
            get { return _facade; }
            set { _facade = value; }
        }

        public FormsManagement()
        {
            _managed_baseform = new List<BaseForm>();
            _mementManagement = new CommandMementoManagement();
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

        public void Operate(IEnumerable<Request.AbstractCommand> abstract_command)
        {
            foreach (var command in abstract_command)
            {
                if (command.Validate())
                {
                    var target_forms = new List<BaseForm>();
                    foreach (var form in _managed_baseform)
                    {
                        BaseForm invoker = command.IsForSelf ? command.Invoker : form.Invoker;

                        if (invoker == command.Invoker && form.GetType() == command.FormType)
                        {
                            target_forms.Add(form);
                        }
                    }

                    foreach (var target in target_forms)
                    {
                        command.Next(target);
                    }
                }
                else
                {
                    command.HandleValidationError();
                }
            }


            _mementManagement.PushCommand(abstract_command);
        }

        public void OperateFromInit(BaseForm target)
        {
            foreach (var recent_commands in _mementManagement.MememtoCommand)
            {
                foreach (var command in recent_commands)
                {
                    BaseForm invoker = command.IsForSelf ? command.Invoker : target.Invoker;

                    if (invoker == command.Invoker && target.GetType() == command.FormType)
                    {
                        command.Next(target);
                    }
                }
            }
        }

        public void OperatePrevious()
        {
            var recent_commands = _mementManagement.PopCommand();

            if (recent_commands == null)
            {
                return;
            }

            foreach (var command in recent_commands)
            {
                foreach (var form in _managed_baseform)
                {
                    BaseForm invoker = command.IsForSelf ? command.Invoker : form.Invoker;

                    if (invoker == command.Invoker && form.GetType() == command.FormType)
                    {
                        command.Prev(form);
                        command.Finalize(form);
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
