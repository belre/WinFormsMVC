using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.View;
using WinFormsMVCSample.Controller;
using WinFormsMVCSample.View;
using WinFormsMVC.Facade;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;

namespace WinFormsMVCSample
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

            public string RootMessage
            {
                get
                {
                    return label2.Text;
                }
                set
                {
                    label2.Text = value;
                }
            }
            
            public string FolderPath
            {
                get
                {
                    return label3.Text;
                }
                set
                {
                    label3.Text = value;
                }
            }

            public Form3()
            {
                InitializeComponent();
            }

            private void button1_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form3Controller>(this);
                controller.Launch<Form2>(this);
            }

            private void button2_Click(object sender, EventArgs e)
            {
                label1.Text = "Self";
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form3Controller>(this);
                controller.SendStoredMessage(new Command[]
                {
                    new GenericCommand<Form1, TextItem>()
                    {
                        Invoker = this,
                        NodeSearchMode = Command.NodeSearchMethod.RecursiveShallower,
                        Preservation = (item, status, form1) =>
                        {
                            item[form1] = form1.Label1;
                            item.Next = "Hello";
                        },
                        NextOperation = (item, status, form1) =>
                        {
                            form1.Label1 = item.Next;
                        },
                        PrevOperation = (item, status, form1) =>
                        {
                            form1.Label1 = item[form1];
                        }
                    }
                }, null);
            }

            private void button4_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form3Controller>(this);
                controller.SendStoredMessage(new Command[]
                {
                    new GenericCommand<IMvcForm1, TextItem>()
                    {
                        Invoker = this,
                        NodeSearchMode = Command.NodeSearchMethod.All,
                        Preservation = (item, status, form1) =>
                        {
                            item[form1] = form1.Label2;
                            item.Next = "Hello";
                        },
                        NextOperation = (item, status, form1) =>
                        {
                            form1.Label2 = item.Next;
                        },
                        PrevOperation = (item, status, form1) =>
                        {
                            form1.Label2 = item[form1];
                        }
                    }
                }, null);
            }
        }
    }

}
