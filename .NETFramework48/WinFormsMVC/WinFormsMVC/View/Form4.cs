using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsMVC.View
{
    public partial class Form4 : WinFormsMVC.View.BaseForm
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
