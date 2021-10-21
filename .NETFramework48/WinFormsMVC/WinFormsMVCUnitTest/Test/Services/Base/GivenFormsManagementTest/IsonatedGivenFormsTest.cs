using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class IsonatedGivenFormsTest : GivenFormManagementTestFormat
    {
        public class ChildForm1 : BaseForm
        {

        }

        public class ChildForm2 : BaseForm
        {

        }

        public class ChildForm3 : BaseForm
        {

        }

        private bool _was_validation = false;
        private bool _was_finalize = false;
        private bool _was_error = false;

        public IsonatedGivenFormsTest()
        {
            var forms = new List<BaseForm>()
            {
                new ChildForm1() { Text = "First Text, ChildForm1" },
                new ChildForm2() { Text = "First Text, ChildForm2-1" },
                new ChildForm2() { Text = "First Text, ChildForm2-2" },
                new ChildForm3() { Text = "First Text, ChildForm3" }
            };
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                new GenericCommand<ChildForm1, TextItem>()
                {
                    Invoker = forms.First(),
                    IsForSelf = true,
                    Validation = (item) =>
                    {
                        item.Next = "Validation Text - ChildForm1";
                        _was_validation = true;
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
                    }),
                    FinalOperation = ((item) =>
                    {
                        _was_finalize = true;
                    }),
                    ErrorOperation = ((item) =>
                    {
                        _was_error = true;
                    })
                }
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
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)list.First()).WasThroughValidation);
                Assert.AreEqual("Validation Text - ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationErrorTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                    {
                        ((GenericCommand<ChildForm1, TextItem>)command).Validation = (item) =>
                        {
                            item.Next = "Validation Text";
                            _was_validation = true;
                            return false;
                        };
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsTrue(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)list.First()).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationNullCheckTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                    {
                        ((GenericCommand<ChildForm1, TextItem>)command).Validation = null;
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse(((GenericCommand<ChildForm1, TextItem>)list[0]).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void InvokerNullTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                    {
                        ((GenericCommand<ChildForm1, TextItem>)command).Invoker = null;
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)list[0]).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);         // 該当データがいないのでテキストは同じ
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void BeCalledByNullInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                    {
                        ((GenericCommand<ChildForm1, TextItem>)command).Invoker = null;
                        ((GenericCommand<ChildForm1, TextItem>)command).IsForSelf = false;
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)list[0]).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);         // 該当データがいないのでテキストは同じ
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void BeCalledByExistedInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                    {
                        ((GenericCommand<ChildForm1, TextItem>)command).Invoker = forms.Last();
                        ((GenericCommand<ChildForm1, TextItem>)command).IsForSelf = false;
                    }
                }


            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation); // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)list[0]).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text); // 該当データがいないのでテキストは同じ            });
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void BeRetrievedByExistedInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                    {
                        ((GenericCommand<ChildForm1, TextItem>)command).Invoker = forms.Last();
                        ((GenericCommand<ChildForm1, TextItem>)command).IsForSelf = false;
                        ((GenericCommand<ChildForm1, TextItem>)command).IsRetrieved = true;
                    }
                }
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<ChildForm1, TextItem>)list[0]).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);         // 該当データがいないのでテキストは同じ          });
            });

        }

    }
}
