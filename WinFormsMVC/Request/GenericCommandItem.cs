using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request
{
    /// <summary>
    /// コマンドのアイテムの基本クラスを表します。
    /// </summary>
    public class GenericCommandItem<T> : CommandItem where T : class
    {
        /// <summary>
        /// 以前のデータを記録する
        /// </summary>
        protected Dictionary<IMvcForm, T> PrevItem
        {
            get;
        }

        /// <summary>
        /// 実行時のデータ
        /// </summary>
        public virtual T Next
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// バックアップデータの保存
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public virtual T this[IMvcForm form]
        {
            get
            {
                if (form != null && PrevItem.Keys.Contains(form))
                {
                    return PrevItem[form];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                PrevItem[form] = value;
            }
        }


        public GenericCommandItem()
        {
            PrevItem = new Dictionary<IMvcForm, T>();
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                if (disposing)
                {

                }
            }
        }
    }
}
