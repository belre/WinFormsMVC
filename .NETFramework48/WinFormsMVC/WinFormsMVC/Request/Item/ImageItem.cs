using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request.Item
{
    /// <summary>
    /// Controllerとして画像を扱うアイテム
    /// </summary>
    public class ImageItem : GenericCommandItem<Image>
    {
        private Image _temporary_image = null;

        public override Image Next
        {
            get
            {
                return (Image)_temporary_image.Clone();
            }
            set
            {
                _temporary_image = (Image)value;
            }
        }
    }
}
