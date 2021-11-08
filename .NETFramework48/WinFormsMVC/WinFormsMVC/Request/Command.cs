using System;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    /// <summary>
    /// 抽象化されたコマンド
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// コマンドを発生させたフォーム
        /// </summary>
        public virtual BaseForm Invoker { get; set; }

        /// <summary>
        /// フォームの型
        /// </summary>
        public abstract Type FormType
        {
            get;
        }

        public abstract bool WasThroughValidation
        {
            get;
        }


        /// <summary>
        /// 自分自身に対するコマンドか.
        /// </summary>
        public bool IsForSelf { get; set; }

        /// <summary>
        /// 孫に再帰するか.
        /// コマンドはIsForSelfが先に優先されます.
        /// </summary>
        public bool IsRecursive { get; set; }


        /// <summary>
        /// FormTypeで指定されたクラスが継承されているものも含むかどうかを表します.
        /// </summary>
        public bool IsIncludingInheritedType { get; set; }



        /// <summary>
        /// データ反映
        /// </summary>
        /// <returns>成功時はtrue</returns>
        public abstract bool Validate();


        /// <summary>
        /// やり直し実行時のデータ反映
        /// </summary>
        /// <returns></returns>
        public abstract bool Restore(BaseForm form);

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="form">対象のフォーム</param>
        public abstract void Prev(BaseForm form);

        /// <summary>
        /// やり直し＆実行
        /// </summary>
        /// <param name="form">対象のフォーム</param>
        public abstract void Next(BaseForm form);

        /// <summary>
        /// 元に戻す実行後に行なう処理
        /// </summary>
        /// <param name="form"></param>
        public abstract void Invalidate();

        /// <summary>
        /// データ反映時に失敗したら実行する処理
        /// </summary>
        protected abstract void HandleValidationError();
    }
}
