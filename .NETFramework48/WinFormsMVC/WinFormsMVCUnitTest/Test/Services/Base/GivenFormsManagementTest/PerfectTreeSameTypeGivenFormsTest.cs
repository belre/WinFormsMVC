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
    public class PerfectTreeSameTypeGivenFormsTest : GivenFormManagementTestFormat
    {
        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
        }



        public PerfectTreeSameTypeGivenFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreatePerfectTreeForms(DefaultBaseForm, BaseForm.MaxDepthTree);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseForm>(forms.First(), "Validation Text")
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void CalledBySelf_RootInvoker_Test()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

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
        public void CalledByRootInvokerTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (forms.First() == form.Invoker)
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2, throw_count);
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void RetrievedByRootInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRetrieved = true;
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                }
                Assert.AreEqual(forms.Count-1, throw_count);
            });

        }

        [TestMethod, TestCategory("正常系")]
        public void CalledBySelf_LastInvoker_Test()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).Invoker = forms.Last();
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

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
        public void CalledByLastInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).Invoker = forms.Last();
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }





        [TestMethod, TestCategory("正常系")]
        public void CalledByFirstAndLastInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.First();
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;

                list.Add(CreateDefaultCommand<BaseForm>(forms.Last(), "Validation Text - 2"));
                list.Last().IsForSelf = false;
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker == forms.First())
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
                Assert.AreEqual(2, throw_count);
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void CalledByLeftSecondaryInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.First().Children.First();
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker == forms.First().Children.First())
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2, throw_count);
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void RetrievedByLastInvokerTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRetrieved = true;
                (list.First()).Invoker = forms.Last();
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }


        [TestMethod, TestCategory("異常系")]
        public void CalledByNullInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {

                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        (command).Invoker = null;
                        (command).IsForSelf = false;
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation); // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });
        }


        [TestMethod, TestCategory("異常系")]
        public void ValidationErrorTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        ((GenericCommand<BaseForm, TextItem>)command).Validation = (item) =>
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
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
                }
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationNullCheckTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        ((GenericCommand<BaseForm, TextItem>)command).Validation = null;
                    }
                }

            }, null, (list, forms) =>
            {
                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse((list[0]).WasThroughValidation);
                Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
            });
        }
    }
}
