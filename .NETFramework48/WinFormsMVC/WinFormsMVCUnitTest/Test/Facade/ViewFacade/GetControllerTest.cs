using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using WinFormsMVC.Controller;
using WinFormsMVC.Services;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Facade.ViewFacade
{
    [TestClass]
    public class GetControllerTest
    {
        public class Test1Controller : BaseController
        {
            public static string DefaultText
            {
                get
                {
                    return "Test1Controller";
                }
            }

            public string GetText()
            {
                return DefaultText;
            }
        }
        public class Test2Controller : CommandController
        {

            public Test2Controller(FormsManagement form_manager) : base(form_manager)
            {
            }

            public static string DefaultText
            {
                get
                {
                    return "Test2Controller";
                }
            }

            public string GetText()
            {
                return DefaultText;
            }
        }

        public class Test3Controller : BaseController
        {
            public static string DefaultText
            {
                get
                {
                    return "Test3Controller";
                }
            }

            public string GetText()
            {
                return DefaultText;
            }
        }

        protected FormsManagement Manager
        {
            get;
        }

        protected WinFormsMVC.Facade.ViewFacade Facade
        {
            get;
        }

        public GetControllerTest()
        {
            Manager = new FormsManagement();
            Facade = new WinFormsMVC.Facade.ViewFacade(Manager);
        }

        [TestMethod]
        public void GetTest1Controller()
        {
            var controller = Facade.GetController<Test1Controller>(new BaseForm());
            Assert.AreEqual(Test1Controller.DefaultText, controller.GetText());   
        }

        [TestMethod]
        public void GetTest2Controller()
        {
            var initiated_form = new BaseForm();
            initiated_form.Load += (sender, args) =>
            {
                initiated_form.WindowState = FormWindowState.Minimized;
            };

            var target_form = new BaseForm();
            
            var controller = Facade.GetController<Test2Controller>(initiated_form);
            controller.Launch<BaseForm>(initiated_form, null, form =>
            {
                form.WindowState = FormWindowState.Minimized;
            });
            
            Assert.AreEqual(Test2Controller.DefaultText, controller.GetText());

        }

    }
}
