using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.FormsManagement
{
    [TestClass]
    public class LaunchAndCloseTest
    {
        private WinFormsMVC.Services.FormsManagement _form_manager;

        protected BaseForm SingleModelessForm
        {
            get
            {
                var default_form = new BaseForm()
                {
                    Text = "Test Text"
                };
                default_form.Load += (sender, e) => { default_form.WindowState = FormWindowState.Minimized; };

                return default_form;
            }
        }

        protected BaseForm SingleModalForm
        {
            get
            {
                var default_form = SingleModelessForm;
                default_form.Load += (sender, args) => { default_form.Close(); };
                return default_form;
            }
        }

        public LaunchAndCloseTest()
        {
            _form_manager = new WinFormsMVC.Services.FormsManagement();
        }

 


        [TestMethod]
        public void LaunchInitFromUnknownFormTest()
        {
            _form_manager.LaunchForm(new BaseForm(), SingleModelessForm, false);
        }


        [TestMethod]
        public void LaunchInitModelessFormTest()
        {
            _form_manager.LaunchForm(null, SingleModelessForm, false);
        }

        [TestMethod]
        public void LaunchInitModalFormTest()
        {
            _form_manager.LaunchForm(null, SingleModalForm, true);
        }

    }
}
