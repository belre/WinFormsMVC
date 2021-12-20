using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Controller.CommandController
{
    [TestClass]
    public class SendSimpleMessageTest : FacadeAndFormManagementTestFormat
    {
        public SendSimpleMessageTest()
        {

        }

        [TestMethod]
        public void SendJustSimpleMessage()
        {
            var form_initiate = new BaseForm();
            (new BaseFormModel()).AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            (new BaseFormModel()).AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            controller.SendSimpleMessage(new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    NodeSearchMode = Command.NodeSearchMethod.OnlyMyChildren,
                    Validation = (item) =>
                    {
                        item.Next = form_initiate.Text;
                        return true;
                    },
                    NextOperation = (item, status, form) =>
                    {
                        form.Text = item.Next;
                    }
                }
            });

            Assert.AreEqual(form_initiate.Text, form_second.Text);
        }

        [TestMethod]
        public void SendNullAsSimpleMessage()
        {
            var form_initiate = new BaseForm();
            (new BaseFormModel()).AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            (new BaseFormModel()).AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            Assert.ThrowsException<NullReferenceException>( () => controller.SendSimpleMessage(null));

        }

    }
}
