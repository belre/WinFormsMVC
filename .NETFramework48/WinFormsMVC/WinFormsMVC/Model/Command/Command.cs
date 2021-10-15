using System;
using WinFormsMVC.View;

namespace WinFormsMVC.Model.Command
{
    public class Command<TargetForm> : AbstractCommand where TargetForm : BaseForm
    {
        public string NextTemporary
        {
            get;
            set;
        }

        public string PrevTemporary
        {
            get;
            set;
        }

        public override Type FormType
        {
            get
            {
                return typeof(TargetForm);
            }
        }

        public Func<Command<TargetForm>, TargetForm, bool> InitOperation { get; set; }

        public Action<Command<TargetForm>, TargetForm> NextOperation { get; set; }

        public Action<Command<TargetForm>, TargetForm> PrevOperation { get; set; }

        public Action<Command<TargetForm>, TargetForm> FinalOperation { get; set; }

        public Action<Command<TargetForm>, TargetForm> ErrorOperation { get; set; }


        public override bool Initialize(BaseForm form)
        {
            if (InitOperation != null)
            {
                return InitOperation(this, (TargetForm)form);
            }
            else
            {
                return true;
            }
        }

        public override void Prev(BaseForm form)
        {
            if (PrevOperation != null)
            {
                PrevOperation(this, (TargetForm)form);
            }
        }

        public override void Next(BaseForm form)
        {
            if (NextOperation != null)
            {
                NextOperation(this, (TargetForm)form);
            }
        }

        public override void Finalize(BaseForm form)
        {
            if (FinalOperation != null)
            {
                FinalOperation(this, (TargetForm)form);
            }
        }

        public override void HandleInitError(BaseForm form)
        {
            if (ErrorOperation != null)
            {
                ErrorOperation(this, (TargetForm)form);
            }
        }
    }
}
