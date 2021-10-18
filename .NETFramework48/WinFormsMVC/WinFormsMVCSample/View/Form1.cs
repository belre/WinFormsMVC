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
                controller.Launch<Form2>(this);
                //controller.LaunchForm2(this);

            }

            private void button2_Click(object sender, EventArgs e)
            {

            }

            private void button2_Click_1(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form1Controller>(this);
                controller.SendStoredMessage( new Command[]
                {
                    new GenericCommand<Form3, TextItem>()
                    {
                        Invoker = this,
                        IsRetrieved = true,
                        Validation = (command, item) =>
                        {
                            item.Next = "Hello World";
                            return true;
                        },
                        PrevOperation = (command, item, form3) =>
                        {
                            form3.RootMessage = item[form3];
                        },
                        NextOperation = (command, item, form3) =>
                        {
                            item[form3] = form3.RootMessage;
                            form3.RootMessage = item.Next;
                        }
                    }
                }, null);
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form1Controller>(this);
                controller.LaunchWithLock<SPWFolderForm>(this, OnClosedFolderBrowser<SPWFolderForm>, (spw_form) =>
                {
                    spw_form.RootDrive = @"F:\usrdata";
                });
            }
            
            private void OnClosedFolderBrowser<T>(object sender, FormClosedEventArgs e) where T : SPWFolderForm
            {
                var form = (T) sender;

                if (form.DialogResult == DialogResult.OK)
                {
                    var controller = Facade.GetController<Form1Controller>(this);
                    controller.SendStoredMessage(new Command[]
                    {
                        new GenericCommand<Form3, TextItem>()
                        {
                            Invoker = this,
                            IsRetrieved = true,
                            Validation = (command, item) =>
                            {
                                item.Next = form.FilePath;
                                return true;
                            },
                            PrevOperation = (command, item, form3) =>
                            {
                                form3.FolderPath = item[form3];
                            },
                            NextOperation = (command, item, form3) =>
                            {
                                item[form3] = form3.FolderPath;
                                form3.FolderPath = item.Next;
                            }
                        }
                    }, null);
                }
            }
        }
    }
}
