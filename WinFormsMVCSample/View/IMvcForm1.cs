using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVCSample.View
{
    interface IMvcForm1 : IMvcForm
    {
        string Label2
        {
            get;
            set;
        }
    }
}
