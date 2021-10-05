using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMVC.Controller;
using WinFormsMVC.Facade;


namespace WinFormsMVC
{
    namespace View
    {
        public partial class BaseForm : Form
        {
            public ViewFacade Facade
            {
                get;
                set;
            }

            public BaseForm Invoker
            {
                get;
                set;
            }

            public BaseForm()
            {
                InitializeComponent();
            }

        }
    }
}
