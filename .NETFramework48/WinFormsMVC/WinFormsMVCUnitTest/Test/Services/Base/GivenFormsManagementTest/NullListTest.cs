using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass, TestCategory("異常系")]
    public class NullListTest
    {

        private Command[] _default_commands;

        public NullListTest()
        {
            _default_commands = new Command[]
            {
                new GenericCommand<IsonatedGivenFormsTest.ChildForm1, TextItem>()
                {
                    Invoker = new BaseForm(),
                    IsForSelf = true,
                    Validation = (item) =>
                    {
                        item.Next = "Validation Text - ChildForm1";
                        return true;
                    },
                    NextOperation = ((item, form1) =>
                    {
                        item[form1] = item.Next;
                        form1.Text = item.Next;
                    }),
                    PrevOperation = ((item, form1) =>
                    {
                        form1.Text = item[form1];
                    })
                }
            };
        }

        [TestMethod]
        public void CalledBySelfTest()
        {
            var given_form_obj = new GivenFormsManagement(null);

            Assert.ThrowsException<NullReferenceException>((() =>
            {
                given_form_obj.Run(_default_commands);
            }));
        }
    }
}
