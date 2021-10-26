using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
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
        public void LaunchInitFromUnknownForm()
        {
            var form = SingleModelessForm;
            _form_manager.LaunchForm(new BaseForm(), form, false);
            Assert.IsTrue(_form_manager.IsExistForm(form));
            Assert.IsFalse(_form_manager.IsExistForm(SingleModelessForm));
        }


        [TestMethod]
        public void LaunchInit()
        {
            var form = SingleModelessForm;
            _form_manager.LaunchForm(null, form, false);
            Assert.IsTrue(_form_manager.IsExistForm(form));
            Assert.IsFalse(_form_manager.IsExistForm(SingleModelessForm));
        }

        [TestMethod]
        public void LaunchInitModalForm()
        {
            var form = SingleModalForm;
            _form_manager.LaunchForm(null, SingleModalForm, true);
            Assert.IsFalse(_form_manager.IsExistForm(form));        // ModalFormの場合、閉じるまで、スレッドがロックされるため
            Assert.IsFalse(_form_manager.IsExistForm(SingleModalForm));
        }

        [TestMethod]
        public void LaunchModalFormOnAnotherThread()
        {
            var form = SingleModelessForm;
            Task.Run(() =>
            {
                _form_manager.LaunchForm(null, form, true);

            });

            
            Thread.Sleep(20);

            Assert.IsTrue(_form_manager.IsExistForm(form));        // ModalFormの場合、閉じるまで、スレッドがロックされるため
            Assert.IsTrue(_form_manager.IsLoadForms);
            Assert.IsFalse(_form_manager.IsExistForm(SingleModalForm));

            form.Close();
            Thread.Sleep(20);
            Assert.IsFalse(_form_manager.IsLoadForms);
            Assert.IsFalse(_form_manager.IsExistForm(form));

        }


    }
}
