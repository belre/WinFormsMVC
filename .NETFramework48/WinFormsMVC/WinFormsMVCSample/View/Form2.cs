using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.View;
using WinFormsMVC.Request;
using WinFormsMVCSample.Controller;

namespace WinFormsMVCSample
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

                controller.SendMessage( new AbstractCommand[] {
                    new Command<Form3> {
                        Invoker=this,
                        Validation = (command) =>
                        {
                            //command.PrevTemporary = form3.Message;
                            command.NextTemporary = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, form3) =>
                        {
                            if (command.PrevTemporary != null)
                            {
                                form3.Message = command.PrevTemporary;
                            }
                        },
                        NextOperation = (command, form3) =>
                        {
                            if (command.NextTemporary != null)
                            {
                                command.PrevTemporary = form3.Message;
                                form3.Message = command.NextTemporary;
                            }
                        }
                    },
                    new Command<Form4>() {
                        Invoker = this,
                        Validation = (command) =>
                        {
                            //command.PrevTemporary = form4.Message;
                            command.NextTemporary = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, form4) =>
                        {
                            if (command.PrevTemporary != null)
                            {
                                form4.Message = command.PrevTemporary;
                            }
                        },
                        NextOperation = (command, form4) =>
                        {
                            if (command.NextTemporary != null)
                            {
                                command.PrevTemporary = form4.Message;
                                form4.Message = command.NextTemporary;
                            }
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

