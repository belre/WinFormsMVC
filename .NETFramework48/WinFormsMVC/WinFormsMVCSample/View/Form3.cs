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
        }
    }

}
