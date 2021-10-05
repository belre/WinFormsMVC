using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Facade;
using WinFormsMVC.View;

namespace WinFormsMVC.Main.Services
{
    public class FormManager
    {
        private List<BaseForm> _managed_baseform;
        private ViewFacade _facade;

        public ViewFacade Facade
        {
            get { return _facade; }
            set { _facade = value; }
        }

        public FormManager()
        {
            _managed_baseform = new List<BaseForm>();
        }

        public void LaunchForm<TargetForm>(BaseForm source, TargetForm target, MementoManager mementoes)
            where TargetForm : BaseForm
        {
            _managed_baseform.Add(target);
            target.Invoker = source;
            target.Facade = _facade;
            target.Closed += OnFormClosed;
            OperateFromInit(target, mementoes);
            target.Show();
        }

        public void Operate<TargetForm>(Command.Command command)
            where TargetForm : BaseForm
        {
            var target_forms = new List<BaseForm>();
            foreach (var form in _managed_baseform)
            {
                if (form.Invoker == command.Invoker && form.GetType() == typeof(TargetForm))
                {
                    target_forms.Add((TargetForm) form);
                }
            }

            bool was_done = true;
            foreach (var target in target_forms)
            {
                if (command.InitOperation(command, target))
                {
                    command.NextOperation(command, target);
                }
                else
                {
                    was_done = false;
                    break;
                }
            }

            if (!was_done)
            {
                foreach (var target in target_forms)
                {
                    if (command.ErrorOperation != null)
                    {
                        command.ErrorOperation(command, target);
                    }
                }
            }
        }

        public void OperateFromInit(BaseForm target, MementoManager mementoes)
        {
            foreach (var operation in mementoes.MememtoCommand)
            {
                if (target.Invoker == operation.Invoker)
                {
                    operation.NextOperation(operation, target);
                }
            }
        }

        public void OperatePrevious<TargetForm>(Command.Command command)
            where TargetForm : BaseForm
        {
            if (command == null)
            {
                return;
            }

            foreach (var form in _managed_baseform)
            {
                if (form.Invoker == command.Invoker && form.GetType() == typeof(TargetForm))
                {
                    command.PrevOperation(command, form);

                    if (command.FinalOperation != null)
                    {
                        command.FinalOperation(command, form);
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
