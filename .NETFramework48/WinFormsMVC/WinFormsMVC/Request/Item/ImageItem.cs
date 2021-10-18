using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Request.Item
{
    /// <summary>
    /// Controllerとして画像を扱うアイテム
    /// </summary>
    public class ImageItem : CommandItem
    {
        /// <summary>
        /// 前回定義した画像
        /// </summary>
        public Image PrevImage
        {
            get;
            set;
        }

        /// <summary>
        /// 今回定義した画像
        /// </summary>
        public Image NextImage
        {
            get;
            set;
        }
    }
}
