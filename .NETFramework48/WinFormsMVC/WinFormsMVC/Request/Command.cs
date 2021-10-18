using System;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    /// <summary>
    /// コマンドを表します
    /// </summary>
    /// <typeparam name="TargetForm">対象とするフォーム</typeparam>
    /// <typeparam name="Item">何を送信するか(テキスト、画像など)</typeparam>
    public class Command<TargetForm, Item> : AbstractCommand where TargetForm : BaseForm where Item : CommandItem
    {
        /// <summary>
        /// 確保されているデータを表します。
        /// </summary>
        public Item StoredItem
        {
            get;
        }

        /// <summary>
        /// フォームの型を表します。
        /// </summary>
        public override Type FormType
        {
            get
            {
                return typeof(TargetForm);
            }
        }

        /// <summary>
        /// コマンドを生成します。
        /// </summary>
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

        /// <summary>
        /// データ検証を行うときに実行される処理です。
        /// </summary>
        public Func<Command<TargetForm, Item>, Item, bool> Validation { get; set; }

        /// <summary>
        /// 「実行」「やり直し」で行なわれる処理です。
        /// </summary>
        public Action<Command<TargetForm, Item>, Item, TargetForm> NextOperation { get; set; }

        /// <summary>
        /// 「元に戻す」で行なわれる処理です。
        /// </summary>
        public Action<Command<TargetForm, Item>, Item, TargetForm> PrevOperation { get; set; }

        /// <summary>
        /// 「元に戻す」の後に行なわれる処理です。
        /// </summary>
        public Action<Command<TargetForm, Item>, Item> FinalOperation { get; set; }

        /// <summary>
        /// データ検証に失敗したときに実行される処理です。
        /// </summary>
        public Action<Command<TargetForm, Item>, Item>  ErrorOperation { get; set; }

        /// <summary>
        /// データ検証を実行します。
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 元に戻すを実行します。
        /// </summary>
        /// <param name="form"></param>
        public override void Prev(BaseForm form)
        {
            if (PrevOperation != null)
            {
                PrevOperation(this, StoredItem, (TargetForm)form);
            }
        }

        /// <summary>
        /// 実行、やり直しを実行します。
        /// </summary>
        /// <param name="form"></param>
        public override void Next(BaseForm form)
        {
            if (NextOperation != null)
            {
                NextOperation(this, StoredItem, (TargetForm)form);
            }
        }


        /// <summary>
        /// 元に戻すの後の処理を表します。
        /// </summary>
        /// <param name="form"></param>
        public override void Finalize()
        {
            if (FinalOperation != null)
            {
                FinalOperation(this, StoredItem);
            }
        }

        /// <summary>
        /// データ検証に失敗したときの処理を表します。
        /// </summary>
        public override void HandleValidationError()
        {
            if (ErrorOperation != null)
            {
                ErrorOperation(this, StoredItem);
            }
        }
    }
}
