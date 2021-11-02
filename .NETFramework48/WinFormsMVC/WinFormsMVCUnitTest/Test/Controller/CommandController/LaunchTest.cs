using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Controller.CommandController
{
    [TestClass]
    public class LaunchTest : FacadeAndFormManagementTestFormat
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
            BaseFormModel.AddInitialAttributes(form1, false);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form1);
            controller.Launch<Form2>(form1, form2 =>
            {
                BaseFormModel.AddInitialAttributes(form2, false);
            });
        }
    }
}
