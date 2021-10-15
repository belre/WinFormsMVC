using System;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    public class Command<TargetForm, Item> : AbstractCommand where TargetForm : BaseForm where Item : CommandItem
    {
        public Item StoredItem
        {
            get;
        }

        public override Type FormType
        {
            get
            {
                return typeof(TargetForm);
            }
        }

        public Command()
        {
            var temporary_item = typeof(Item).GetConstructor(new Type[0]);
            if (temporary_item != null)
            {
                StoredItem = (Item)temporary_item.Invoke(new object[0]);
            }
            else
            {
                throw new TypeInitializationException(typeof(Item).Name, new Exception("コマンドアイテムが異常です"));
            }
        }

        public Func<Command<TargetForm, Item>, Item, bool> Validation { get; set; }

        public Action<Command<TargetForm, Item>, Item, TargetForm> NextOperation { get; set; }

        public Action<Command<TargetForm, Item>, Item, TargetForm> PrevOperation { get; set; }

        public Action<Command<TargetForm, Item>, Item, TargetForm> FinalOperation { get; set; }

        public Action<Command<TargetForm, Item>, Item>  ErrorOperation { get; set; }

        public override bool Validate()
        {
            if (Validation != null)
            {
                return Validation(this, StoredItem);
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
                PrevOperation(this, StoredItem, (TargetForm)form);
            }
        }

        public override void Next(BaseForm form)
        {
            if (NextOperation != null)
            {
                NextOperation(this, StoredItem, (TargetForm)form);
            }
        }

        public override void Finalize(BaseForm form)
        {
            if (FinalOperation != null)
            {
                FinalOperation(this, StoredItem, (TargetForm)form);
            }
        }

        public override void HandleValidationError()
        {
            if (ErrorOperation != null)
            {
                ErrorOperation(this, StoredItem);
            }
        }
    }
}
