using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Request.Item
{
    /// <summary>
    /// ControllerとしてTextを扱うアイテム
    /// </summary>
    public class TextItem : CommandItem
    {
        /// <summary>
        /// 前回定義したテキスト
        /// </summary>
        public string PrevText
        {
            get;
            set;
        }

        /// <summary>
        /// 今回定義したテキスト
        /// </summary>
        public string NextText
        {
            get;
            set;
        }

    }
}
