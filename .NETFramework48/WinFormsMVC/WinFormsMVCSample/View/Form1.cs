using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMVC.View;
using WinFormsMVC.Facade;
using WinFormsMVCSample.Controller;

namespace WinFormsMVCSample
{
    namespace View
    {
        public partial class Form1 : BaseForm
        {
            public Form1()
            {
                InitializeComponent();
            }


            private void button1_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form1Controller>(this);
                controller.LaunchForm2(this);
            }

            private void button2_Click(object sender, EventArgs e)
            {

            }
        }
    }
}
