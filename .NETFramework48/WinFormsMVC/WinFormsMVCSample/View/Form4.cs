using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.View;
using WinFormsMVC.Facade;
using WinFormsMVCSample.Controller;

namespace WinFormsMVCSample.View
{
    public partial class Form4 : BaseForm
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
        public Form4()
        {
            InitializeComponent();
        }
    }
}
