using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class SingleGivenFormTest : GivenFormManagementTestFormat
    {
        public SingleGivenFormTest()
        {
            var forms = new List<BaseForm>()
            {
                new BaseFormModel.ChildForm1() { Text = "First Text" }
            };
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                new GenericCommand<BaseFormModel.ChildForm1, TextItem>() {
                    Invoker = forms.First(),
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
                }
            });
        }


        [TestMethod, TestCategory("正常系")]
        public void CalledBySelfTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {

            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);
                Assert.AreEqual("Validation Text", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void CalledByNullInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list[0]).Invoker = null;
                (list[0]).IsForSelf = false;
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);
                Assert.AreEqual("First Text", forms.First().Text);         // 該当データがいないのでテキストは同じ
            });

        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationErrorTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)list[0]).Validation = (item) =>
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
                Assert.IsTrue((list[0]).WasThroughValidation);
                Assert.AreEqual("First Text", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationNullCheckTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)list[0]).Validation = null;
            }, null, (list, forms) =>
            {

                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse((list[0]).WasThroughValidation);
                Assert.AreEqual("First Text", forms.First().Text);
            });

        }

    }
}
