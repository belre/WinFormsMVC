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

        public void LaunchForm(BaseForm source, BaseForm target)
        {
            _managed_baseform.Add(target);
            target.Invoker = source;
            target.Facade = _facade;
            target.Closed += OnFormClosed;
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
                    command.NextOperation(command, (TargetForm) form);
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
                    command.PrevOperation(command, (TargetForm) form);
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
