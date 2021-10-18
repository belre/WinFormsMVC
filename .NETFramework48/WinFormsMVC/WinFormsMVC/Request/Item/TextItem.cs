using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

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
        protected Dictionary<BaseForm, string> PrevText
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

        public TextItem()
        {
            PrevText = new Dictionary<BaseForm, string>();
        }

        public void SetPreviousState(BaseForm form, string previous_text)
        {
            PrevText[form] = previous_text;
        }

        public string GetPrevious(BaseForm form)
        {
            if (form != null && PrevText.Keys.Contains(form))
            {
                return PrevText[form];
            }
            else
            {
                return null;
            }
        }

    }
}
