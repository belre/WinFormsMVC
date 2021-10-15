using System;
using WinFormsMVC.View;

namespace WinFormsMVC.Model.Command
{
    public abstract class AbstractCommand
    {

        public BaseForm Invoker { get; set; }

        public abstract Type FormType
        {
            get;
        }

        public abstract bool Initialize(BaseForm form);

        public abstract void Prev(BaseForm form);

        public abstract void Next(BaseForm form);

        public abstract void Finalize(BaseForm form);

        public abstract void HandleInitError(BaseForm form);
    }
}
