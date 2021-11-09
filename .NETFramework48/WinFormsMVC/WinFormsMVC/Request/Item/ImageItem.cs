using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Request.Item
{
    /// <summary>
    /// Controllerとして画像を扱うアイテム
    /// </summary>
    public class ImageItem : GenericCommandItem<Image>, IDisposable
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
                _temporary_image = (Image)value.Clone();
            }
        }

        public override Image this[BaseForm form]
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
                PrevItem[form] = (Image)value.Clone();
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                if (disposing)
                {
                    _temporary_image.Dispose();
                    _temporary_image = null;
                }
            }
        }
    }
}
