using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
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

        public class PrivateOnlyTestController : BaseController
        {
           
            private PrivateOnlyTestController()
            {
                
            }
        }

        public class SingleExplicitCtorTestController : BaseController
        {
            [CalledAsController]
            public SingleExplicitCtorTestController()
            {

            }
        }

        public class SingleExplicitAndImplicitCtorTestController : BaseController
        {
            [CalledAsController]
            public SingleExplicitAndImplicitCtorTestController()
            {

            }

            public SingleExplicitAndImplicitCtorTestController(int x)
            {
                
            }
        }

        public class DoubleExplicitCtorTestController : BaseController
        {
            [CalledAsController]
            public DoubleExplicitCtorTestController()
            {

            }


            [CalledAsController]
            public DoubleExplicitCtorTestController(int x)
            {

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
        public void LaunchFormAsTest2()
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

        [TestMethod]
        public void GetPrivateOnlyTestController()
        {
            var initiated_form = new BaseForm();
            initiated_form.Load += (sender, args) =>
            {
                initiated_form.WindowState = FormWindowState.Minimized;
            };

            Assert.ThrowsException<NotImplementedException>( () => 
            {
                var controller = Facade.GetController<PrivateOnlyTestController>(initiated_form);
            });
        }

        [TestMethod]
        public void GetSingleExplicitCtorTestController()
        {
            var initiated_form = new BaseForm();
            initiated_form.Load += (sender, args) =>
            {
                initiated_form.WindowState = FormWindowState.Minimized;
            };

            var controller = Facade.GetController<SingleExplicitCtorTestController>(initiated_form);
        }

        [TestMethod]

        public void GetSingleExplicitAndImplicitCtorTestController()
        {
            var initiated_form = new BaseForm();
            initiated_form.Load += (sender, args) =>
            {
                initiated_form.WindowState = FormWindowState.Minimized;
            };

            var controller = Facade.GetController<SingleExplicitAndImplicitCtorTestController>(initiated_form);

        }

        [TestMethod]

        public void GetDoubleExplicitCtorTestController()
        {
            var initiated_form = new BaseForm();
            initiated_form.Load += (sender, args) =>
            {
                initiated_form.WindowState = FormWindowState.Minimized;
            };

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                var controller = Facade.GetController<DoubleExplicitCtorTestController>(initiated_form);
            });
        }

    }
}
