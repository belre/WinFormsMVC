using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class SingleFormTest : FormManagementTestFormat
    {
        public class ChildForm1 : BaseForm
        {

        }

        private bool _was_validation = false;
        private bool _was_finalize = false;
        private bool _was_error = false;

        public SingleFormTest()
        {
            FormList.Add(new ChildForm1() { Text = "First Text" });

            DefaultCommands.Add(new GenericCommand<ChildForm1, TextItem>() {
                Invoker = FormList.First(),
                IsForSelf = true,
                Validation = (item) =>
                {
                    item.Next = "Validation Text";
                    _was_validation = true;
                    return true;
                },
                NextOperation = ((item, form1) =>
                {
                    item[form1] = item.Next;
                    form1.Text = item.Next;
                }),
                PrevOperation = ((item, form1) => { form1.Text = item[form1]; }),
                FinalOperation = ((item) => { _was_finalize = true; }),
                ErrorOperation = ((item) => { _was_error = true; })
            });
        }


        [TestMethod, TestCategory("正常系")]
        public void BeCalledBySelfTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {

            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).WasThroughValidation);
                Assert.AreEqual("Validation Text", FormList.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void BeCalledByNullInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).Invoker = null;
                ((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).IsForSelf = false;
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).WasThroughValidation);
                Assert.AreEqual("First Text", FormList.First().Text);         // 該当データがいないのでテキストは同じ
            });

        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationErrorTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).Validation = (item) =>
                {
                    item.Next = "Validation Text";
                    _was_validation = true;
                    return false;
                };
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsTrue(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).WasThroughValidation);
                Assert.AreEqual("First Text", FormList.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationNullCheckTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).Validation = null;
            }, null, (list, forms) =>
            {

                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse(((GenericCommand<ChildForm1, TextItem>)DefaultCommands[0]).WasThroughValidation);
                Assert.AreEqual("First Text", FormList.First().Text);
            });

        }

    }
}
