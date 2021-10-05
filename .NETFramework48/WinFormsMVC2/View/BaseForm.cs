using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MVCTraining.Controller;


namespace MVCTraining
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
