using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Controller.CommandController
{
    [TestClass]
    public class LaunchWithLockTest : FacadeAndFormManagementTestFormat
    {
        class Form1 : BaseForm
        {

        }

        class Form2 : BaseForm
        {

        }

        [TestMethod]
        public void LaunchOneForm()
        {
            var form1 = new Form1();
            (new BaseFormModel()).AddInitialAttributes(form1, true);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form1);
            controller.LaunchWithLock<Form2>(form1, form2 =>
            {
                (new BaseFormModel()).AddInitialAttributes(form2, true);
            });
        }
    }
}
