using System;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    /// <summary>
    /// 抽象化されたコマンド
    /// </summary>
    public abstract class Command : IDisposable
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
        /// 親に対するコマンドか.
        /// </summary>
        public bool IsRecursiveForAncestor { get; set; }

        /// <summary>
        /// 自分自身に対するコマンドか.
        /// </summary>
        public bool IsForSelf { get; set; }

        /// <summary>
        /// 孫に再帰するか.
        /// コマンドはIsForSelfが先に優先されます.
        /// </summary>
        public bool IsRecursiveToChildren { get; set; }


        /// <summary>
        /// FormTypeで指定されたクラスが継承されているものも含むかどうかを表します.
        /// </summary>
        public bool IsIncludingInheritedType { get; set; }

        /// <summary>
        /// 全てのクラスを参照するか
        /// </summary>
        public bool IsAll { get; set; }


        /// <summary>
        /// データ反映
        /// </summary>
        /// <returns>成功時はtrue</returns>
        public abstract bool Validate();


        /// <summary>
        /// やり直し実行時のデータ反映
        /// </summary>
        /// <returns></returns>
        public abstract bool Restore(IMvcForm form);

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="form">対象のフォーム</param>
        public abstract void Prev(IMvcForm form);

        /// <summary>
        /// やり直し＆実行
        /// </summary>
        /// <param name="form">対象のフォーム</param>
        public abstract void Next(IMvcForm form);

        /// <summary>
        /// 元に戻す実行後に行なう処理
        /// </summary>
        /// <param name="form"></param>
        public abstract void Invalidate();

        /// <summary>
        /// データ反映時に失敗したら実行する処理
        /// </summary>
        protected abstract void HandleValidationError();


        protected bool _disposed = false;


        public void Dispose()
        {
            Dispose(disposing: true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {

            }
        }
    }
}
