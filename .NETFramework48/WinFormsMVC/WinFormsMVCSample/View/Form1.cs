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
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
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

            private void button2_Click_1(object sender, EventArgs e)
            {
                /*
                var controller = Facade.GetController<Form1Controller>(this);
                controller.SendMessageWithRecord( new AbstractCommand[]
                {
                    new Command<Form3, TextItem>()
                    {
                        Validation = (command, item) =>
                        {
                            item.NextText = "Hello World";
                            return true;
                        },
                        NextOperation = (command, item, form3) =>
                        {
                            form3.Message = item.NextText;
                        }
                    }
                }, null);
                */
            }
        }
    }
}
