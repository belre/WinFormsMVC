using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.Controller;
using WinFormsMVC.Model.Command;

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

                controller.SendMessage<Form3>(new Command<Form3> {
                    Invoker=this,
                    InitOperation = (command, form3) =>
                    {
                        command.PrevTemporary = ((Form3)form3).Message;
                        command.NextTemporary = textBox1.Text;
                        return true;
                    },
                    PrevOperation = (command, form3) =>
                    {
                        if (command.PrevTemporary != null)
                        {
                            ((Form3)form3).Message = command.PrevTemporary;
                        }
                    },
                    NextOperation = (command, form3) =>
                    {
                        if (command.NextTemporary != null)
                        {
                            ((Form3)form3).Message = command.NextTemporary;
                        }
                    }
                });


                controller.SendMessage<Form4>(new Command<Form4>()
                {
                    Invoker = this,
                    InitOperation = (command, form4) =>
                    {
                        command.PrevTemporary = ((Form4)form4).Message;
                        command.NextTemporary = textBox1.Text;
                        return true;
                    },
                    PrevOperation = (command, form4) =>
                    {
                        if (command.PrevTemporary != null)
                        {
                            ((Form4)form4).Message = command.PrevTemporary;
                        }
                    },
                    NextOperation = (command, form4) =>
                    {
                        if (command.NextTemporary != null)
                        {
                            ((Form4) form4).Message = command.NextTemporary;
                        }
                    }
                });
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.Redo();
            }

            private void button4_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.LaunchForm4(this);
            }
        }
    }
}

