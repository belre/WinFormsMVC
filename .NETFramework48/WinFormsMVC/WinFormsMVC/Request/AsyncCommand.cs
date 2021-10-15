using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    public class AsyncCommand : AbstractCommand
    {
        public delegate void NotifyAsync(AbstractCommand command);

        public override BaseForm Invoker
        {
            get
            {
                return Command.Invoker;
            }
            set
            {

            }
        }



        public AbstractCommand Command
        {
            get;
            set;
        }

        public NotifyAsync NotifyingAsync { get; set; }

        public AsyncCommand(AbstractCommand command)
        {
            Command = command;
        }


        public override Type FormType
        {
            get
            {
                return Command.FormType;
            }
        }


        public override bool Validate()
        {
            return Command.Validate();
        }

        public void ValidateAsync()
        {
            Validate();

            if (NotifyingAsync != null)
            {
                NotifyingAsync(this);
            }
        }

        public override void Prev(BaseForm form)
        {
            Command.Prev(form);
        }

        public override void Next(BaseForm form)
        {
            Command.Next(form);
        }

        public override void Finalize(BaseForm form)
        {
            Command.Finalize(form);
        }

        public override void HandleValidationError()
        {
            Command.HandleValidationError();
        }



    }
}
