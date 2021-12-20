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
    public class GenericCommand<TargetForm, Item> : CommandValidator<Item> where TargetForm : IMvcForm where Item : CommandItem
    {
        public enum Operations
        {
            NO_VALIDATION,
            VALIDATED,
            ERROR_WITH_VALIDATING,
            DONE_FINALIZE
        };


        protected Operations CurrentOperations
        {
            get;
            set;
        }

        public override bool WasThroughValidation
        {
            get
            {
                return CurrentOperations != Operations.NO_VALIDATION;
            }
        }

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
        public GenericCommand()
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

            CurrentOperations = Operations.NO_VALIDATION;
            Status = new TransitionStatus();
        }

        /// <summary>
        /// 初期化で行なわれる処理です。
        /// </summary>
        public Action<Item, TransitionStatus, TargetForm> Preservation { get; set; }

        /// <summary>
        /// 「実行」「やり直し」で行なわれる処理です。
        /// </summary>
        public Action<Item, TransitionStatus, TargetForm> NextOperation { get; set; }

        /// <summary>
        /// 「元に戻す」で行なわれる処理です。
        /// </summary>
        public Action<Item, TransitionStatus, TargetForm> PrevOperation { get; set; }


        /// <summary>
        /// データ検証を実行します。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (CurrentOperations == Operations.NO_VALIDATION)
            {
                if (Validation == null)
                {
                    CurrentOperations = Operations.VALIDATED;
                    return true;
                }

                // Validation状態で処理を変える
                bool ret = Validation(StoredItem);

                // Validateの結果によってStatusを変える
                if (ret)
                {
                    CurrentOperations = Operations.VALIDATED;
                }
                else
                {
                    CurrentOperations = Operations.ERROR_WITH_VALIDATING;
                    HandleValidationError();
                }

                return ret;
            }
            else
            {
                return false;
            }
        }

        public override bool Restore(IMvcForm form)
        {
            if (CurrentOperations == Operations.DONE_FINALIZE || 
                CurrentOperations == Operations.ERROR_WITH_VALIDATING || 
                CurrentOperations == Operations.VALIDATED)
            {
                Status.StageNextValidation();
                Next(form);
                Status.CommitNextValidation();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 元に戻すを実行します。
        /// </summary>
        /// <param name="form"></param>
        public override void Prev(IMvcForm form)
        {
            if (CurrentOperations == Operations.VALIDATED)
            {
                if (PrevOperation != null)
                {
                    PrevOperation(StoredItem, Status, (TargetForm)form);
                }
            }
        }

        public override void Next(IMvcForm form)
        {
            if (CurrentOperations == Operations.VALIDATED)
            {
                if (NextOperation != null)
                {
                    // 初回実行時に値を読み込む
                    if (Status.ExecutedCount == 0 && Preservation != null)
                    {
                        Preservation(StoredItem, Status, (TargetForm)form);
                    }

                    NextOperation(StoredItem, Status, (TargetForm)form);
                }
            }
        }

        /// <summary>
        /// 元に戻すの後の処理を表します。
        /// </summary>
        /// <param name="form"></param>
        public override void Invalidate()
        {
            if (CurrentOperations == Operations.VALIDATED)
            {
                if (Finalization != null)
                {
                    Finalization(StoredItem);
                }

                CurrentOperations = Operations.DONE_FINALIZE;
            }
        }

        /// <summary>
        /// データ検証に失敗したときの処理を表します。
        /// </summary>
        protected override void HandleValidationError()
        {
            if (CurrentOperations != Operations.NO_VALIDATION)
            {
                if (ErrorHandle != null)
                {
                    ErrorHandle(StoredItem);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                if (disposing)
                {
                    StoredItem.Dispose();
                }
            }
        }
    }
}
