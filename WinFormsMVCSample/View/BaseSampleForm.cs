using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsMVCSample.View
{
    public partial class BaseSampleForm : WinFormsMVC.View.BaseForm
    {
        public virtual string CommonText
        {
            get;
            set;
        }

        public BaseSampleForm()
        {
            InitializeComponent();
        }
    }
}
