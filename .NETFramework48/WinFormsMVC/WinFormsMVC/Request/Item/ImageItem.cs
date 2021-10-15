using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Request.Item
{
    public class ImageItem : CommandItem
    {
        public Image PrevImage
        {
            get;
            set;
        }

        public Image NextImage
        {
            get;
            set;
        }
    }
}
