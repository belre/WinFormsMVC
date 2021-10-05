using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Func<Command<TargetForm>, BaseForm, bool> InitOperation { get; set; }

        public Action<Command<TargetForm>, BaseForm> NextOperation { get; set; }

        public Action<Command<TargetForm>, BaseForm> PrevOperation { get; set; }

        public Action<Command<TargetForm>, BaseForm> FinalOperation { get; set; }

        public Action<Command<TargetForm>, BaseForm> ErrorOperation { get; set; }


        public override bool Initialize(BaseForm form)
        {
            if (InitOperation != null)
            {
                return InitOperation(this, form);
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
                PrevOperation(this, form);
            }
        }

        public override void Next(BaseForm form)
        {
            if (NextOperation != null)
            {
                NextOperation(this, form);
            }
        }

        public override void Finalize(BaseForm form)
        {
            if (FinalOperation != null)
            {
                FinalOperation(this, form);
            }
        }

        public override void HandleInitError(BaseForm form)
        {
            if (ErrorOperation != null)
            {
                ErrorOperation(this, form);
            }
        }



    }
}
