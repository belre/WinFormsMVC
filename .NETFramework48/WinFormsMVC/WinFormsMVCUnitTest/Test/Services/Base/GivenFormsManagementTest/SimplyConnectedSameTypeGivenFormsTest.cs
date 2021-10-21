using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;
using WinFormsMVCUnitTest.Test.View.BaseForm;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class SimplyConnectedSameTypeGivenFormsTest : GivenFormManagementTestFormat
    {
        private bool _was_validation = false;
        private bool _was_finalize = false;
        private bool _was_error = false;


        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
        }



        public SimplyConnectedSameTypeGivenFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreateSimplyConnectedForms(DefaultBaseForm, BaseForm.MaxDepthTree);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                new GenericCommand<BaseForm, TextItem>() {
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
        public void BeCalledBySelf_InvokerRoot_Test()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void BeCalledByInvokerRootTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void BeRetrievedByInvokerRootTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsRetrieved = true;
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                    }
                }
            });

        }

        [TestMethod, TestCategory("正常系")]
        public void BeCalledBySelf_InvokerLast_Test()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.Last();
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Last())
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });


        }                

        [TestMethod, TestCategory("正常系")]
        public void BeCalledByInvokerLastTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.Last();
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

        }

        [TestMethod, TestCategory("正常系")]
        public void BeRetrievedByInvokerLastTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsRetrieved = true;
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.Last();
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }


        [TestMethod, TestCategory("異常系")]
        public void BeCalledByNullInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {

                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        ((GenericCommand<BaseForm, TextItem>)command).Invoker = null;
                        ((GenericCommand<BaseForm, TextItem>)command).IsForSelf = false;
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation); // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }        
    }
}
