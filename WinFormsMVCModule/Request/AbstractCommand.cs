using System;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    public abstract class AbstractCommand
    {

        public BaseForm Invoker { get; set; }

        public abstract Type FormType
        {
            get;
        }

        public abstract bool Validate();

        public abstract void Prev(BaseForm form);

        public abstract void Next(BaseForm form);

        public abstract void Finalize(BaseForm form);

        public abstract void HandleValidationError();
    }
}
