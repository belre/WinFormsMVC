using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.Controller;
using WinFormsMVC.Main.Command;

namespace WinFormsMVC
{
    namespace View
    {
        public partial class Form2 : BaseForm
        {
            public Form2()
            {
                InitializeComponent();
            }

            private void button1_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.LaunchForm3(this);
            }

            private void button2_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.SendMessage(new Command{Invoker=this, NextOperation = (command, form3) =>
                    {
                        command._temporary = ((Form3)form3).Message;
                        ((Form3)form3).Message = textBox1.Text;
                        return true;
                    }, PrevOperation = (command, form3) =>
                    {
                        ((Form3)form3).Message = command._temporary;
                        return true;
                    }
                });
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.Redo();
            }
        }
    }
}

