using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    /// <summary>
    /// 非同期処理を表すコマンド
    /// </summary>
    public class AsyncCommand : Command
    {
        /// <summary>
        /// デリゲート
        /// 非同期処理通知
        /// </summary>
        /// <param name="command"></param>
        public delegate void NotifyAsync(Command command);

        /// <summary>
        /// コマンドを発生させたフォーム
        /// (※Commandを参照するので、setterは何もしない)
        /// </summary>
        public override BaseForm Invoker
        {
            get
            {
                return Command.Invoker;
            }
            set
            {
                throw new InvalidOperationException("Async Commandでは、元々定義されたInvoker以外使用できません");
            }
        }


        /// <summary>
        /// ラップさせたコマンド
        /// </summary>
        public Command Command
        {
            get;
            set;
        }

        public override bool WasThroughValidation
        {
            get
            {
                return Command.WasThroughValidation;
            }
        }

        /// <summary>
        /// 非同期処理が終わったら実行される処理
        /// </summary>
        public NotifyAsync NotifyingAsync { get; set; }


        public AsyncCommand(Command command)
        {
            Command = command;
        }

        /// <summary>
        /// フォームの型
        /// </summary>
        public override Type FormType
        {
            get
            {
                return Command.FormType;
            }
        }

        /// <summary>
        /// データ検証(Commandと処理同じ)
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return Command.Validate();
        }

        /// <summary>
        /// 非同期の処理として実行
        /// </summary>
        public void ValidateAsync()
        {
            Validate();

            if (NotifyingAsync != null)
            {
                NotifyingAsync(this);
            }
        }

        /// <summary>
        /// 元に戻す(Commandと処理同じ)
        /// </summary>
        /// <param name="form"></param>
        public override void Prev(BaseForm form)
        {
            Command.Prev(form);
        }

        /// <summary>
        /// やり直し＆実行（Commandと処理同じ)
        /// </summary>
        /// <param name="form"></param>
        public override void Next(BaseForm form)
        {
            Command.Next(form);
        }

        /// <summary>
        /// 元に戻した後に実行される処理(Commandと同じ）
        /// </summary>
        /// <param name="form"></param>
        public override void Invalidate()
        {
            Command.Invalidate();
        }

        /// <summary>
        /// データ検証に失敗したときに実行(Commandと同じ）
        /// </summary>
        protected override void HandleValidationError()
        {
            // Validateにエラー処理をゆだねる
            // Command.HandleValidationError();
        }



    }
}
