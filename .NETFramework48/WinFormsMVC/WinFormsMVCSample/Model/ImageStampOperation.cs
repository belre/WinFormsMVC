using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVCSample.Model
{
    public class ImageStampOperation
    {
        public Image GetNewImage(Image org)
        {
            Graphics g = Graphics.FromImage(org);
            g.DrawString("Assigned", new Font("Arial", 16.0F), Brushes.Blue, 20, 20);


            System.Threading.Thread.Sleep(5000);
            return org;
        }


    }
}
