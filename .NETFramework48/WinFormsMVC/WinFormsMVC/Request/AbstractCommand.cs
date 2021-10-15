using System;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    public abstract class AbstractCommand
    {
        public virtual BaseForm Invoker { get; set; }

        public abstract Type FormType
        {
            get;
        }

        public bool IsForSelf { get; set; }

        public abstract bool Validate();


        public abstract void Prev(BaseForm form);

        public abstract void Next(BaseForm form);

        public abstract void Finalize(BaseForm form);

        public abstract void HandleValidationError();
    }
}
