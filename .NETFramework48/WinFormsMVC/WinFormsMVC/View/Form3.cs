using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.Controller;

namespace WinFormsMVC
{
    namespace View
    {
        public partial class Form3 : BaseForm
        {
            public string Message
            {
                get
                {
                    return label1.Text;
                }

                set
                {
                    label1.Text = value;
                }
            }

            public Form3()
            {
                InitializeComponent();
            }

            private void button1_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form3Controller>(this);
                controller.RenewWindow(this);
            }
        }
    }

}
