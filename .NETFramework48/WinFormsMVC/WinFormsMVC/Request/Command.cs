using System;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
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

        public Func<Command<TargetForm>, bool> Validation { get; set; }

        public Action<Command<TargetForm>, TargetForm> NextOperation { get; set; }

        public Action<Command<TargetForm>, TargetForm> PrevOperation { get; set; }

        public Action<Command<TargetForm>, TargetForm> FinalOperation { get; set; }

        public Action<Command<TargetForm>> ErrorOperation { get; set; }


        public override bool Validate()
        {
            if (Validation != null)
            {
                return Validation(this);
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

        public override void HandleValidationError()
        {
            if (ErrorOperation != null)
            {
                ErrorOperation(this);
            }
        }
    }
}
