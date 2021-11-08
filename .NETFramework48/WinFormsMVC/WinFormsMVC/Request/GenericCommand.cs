﻿using System;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    /// <summary>
    /// コマンドを表します
    /// </summary>
    /// <typeparam name="TargetForm">対象とするフォーム</typeparam>
    /// <typeparam name="Item">何を送信するか(テキスト、画像など)</typeparam>
    public class GenericCommand<TargetForm, Item> : CommandValidator<Item> where TargetForm : BaseForm where Item : CommandItem
    {
        protected ValidationStatus.Operations CurrentOperations
        {
            get;
            set;
        }

        public override bool WasThroughValidation
        {
            get
            {
                return CurrentOperations != Request.ValidationStatus.Operations.NO_VALIDATION;
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

            CurrentOperations = Request.ValidationStatus.Operations.NO_VALIDATION;
            Status = new ValidationStatus();
        }



        /// <summary>
        /// 「実行」「やり直し」で行なわれる処理です。
        /// </summary>
        public Action<Item, TargetForm> NextOperation { get; set; }

        /// <summary>
        /// 「元に戻す」で行なわれる処理です。
        /// </summary>
        public Action<Item, TargetForm> PrevOperation { get; set; }


        /// <summary>
        /// データ検証を実行します。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (CurrentOperations == Request.ValidationStatus.Operations.NO_VALIDATION)
            {
                if (Validation == null)
                {
                    return false;
                }

                // Validation状態で処理を変える
                bool ret = Validation(StoredItem, Status);

                // Validateの結果によってStatusを変える
                if (ret)
                {
                    CurrentOperations = Request.ValidationStatus.Operations.VALIDATED;
                }
                else
                {
                    CurrentOperations = Request.ValidationStatus.Operations.ERROR_WITH_VALIDATING;
                    HandleValidationError();
                }

                Status.CommitValidation();

                return ret;
            }
            else
            {
                return false;
            }
        }

        public override bool RestoreValidation()
        {
            if (CurrentOperations == Request.ValidationStatus.Operations.DONE_FINALIZE || 
                CurrentOperations == Request.ValidationStatus.Operations.ERROR_WITH_VALIDATING )
            {
                Status.StageNextValidation(CurrentOperations);
                CurrentOperations = Request.ValidationStatus.Operations.NO_VALIDATION;

                return Validate();
            }

            return false;
        }

        /// <summary>
        /// 元に戻すを実行します。
        /// </summary>
        /// <param name="form"></param>
        public override void Prev(BaseForm form)
        {
            if (CurrentOperations == Request.ValidationStatus.Operations.VALIDATED)
            {
                if (PrevOperation != null)
                {
                    PrevOperation(StoredItem, (TargetForm) form);
                }
            }
        }

        /// <summary>
        /// 実行、やり直しを実行します。
        /// </summary>
        /// <param name="form"></param>
        public override void Next(BaseForm form)
        {
            if (CurrentOperations == Request.ValidationStatus.Operations.VALIDATED)
            {
                if (NextOperation != null)
                {
                    NextOperation(StoredItem, (TargetForm)form);
                }
            }
        }


        /// <summary>
        /// 元に戻すの後の処理を表します。
        /// </summary>
        /// <param name="form"></param>
        public override void Invalidate()
        {
            if (CurrentOperations == Request.ValidationStatus.Operations.VALIDATED)
            {
                if (FinalOperation != null)
                {
                    FinalOperation(StoredItem, Status);
                    CurrentOperations = Request.ValidationStatus.Operations.DONE_FINALIZE;
                }
            }
        }

        /// <summary>
        /// データ検証に失敗したときの処理を表します。
        /// </summary>
        protected override void HandleValidationError()
        {
            if (CurrentOperations != Request.ValidationStatus.Operations.NO_VALIDATION)
            {
                if (ErrorOperation != null)
                {
                    ErrorOperation(StoredItem, Status);
                }
            }
        }
    }
}
