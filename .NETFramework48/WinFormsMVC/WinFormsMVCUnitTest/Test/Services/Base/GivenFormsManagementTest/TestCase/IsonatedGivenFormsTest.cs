﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class IsonatedGivenFormsTest : GivenFormManagementTestFormat
    {

        public IsonatedGivenFormsTest()
        {
            var forms = new List<BaseForm>()
            {
                new BaseFormModel.ChildForm1() { Text = "First Text, ChildForm1" },
                new BaseFormModel.ChildForm2() { Text = "First Text, ChildForm2-1" },
                new BaseFormModel.ChildForm2() { Text = "First Text, ChildForm2-2" },
                new BaseFormModel.ChildForm3() { Text = "First Text, ChildForm3" }
            };
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                new GenericCommand<BaseFormModel.ChildForm1, TextItem>()
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
        public void CalledBySelf()
        {
            AssertAction((list, forms) =>
            {

            }, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(list.First().WasThroughValidation);
                Assert.AreEqual("Validation Text - ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void CalledBy2Invokers()
        {
            AssertAction((list, forms) =>
            {
                
                list.First().Invoker = forms.First();
                list.First().IsForSelf = false;

                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.Last(), "Validation Text - 2"));
                list.Last().IsForSelf = false;
            }, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreNotEqual("Validation Text", form.Text);
                    }
                }
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationError()
        {
            AssertAction((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm1, TextItem>))
                    {
                        ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)command).Validation = (item) =>
                        {
                            item.Next = "Validation Text";
                            _was_validation = true;
                            return false;
                        };
                    }
                }

            }, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsTrue(_was_error);
                Assert.IsTrue(list.First().WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void ValidationNullCheck()
        {
            AssertAction((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm1, TextItem>))
                    {
                        ((GenericCommand<BaseFormModel.ChildForm1, TextItem>)command).Validation = null;
                    }
                }

            }, (list, forms) =>
            {
                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse(list.First().WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void InvokerBySelf_Null()
        {
            AssertAction((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm1, TextItem>))
                    {
                        command.Invoker = null;
                    }
                }

            }, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue(list.First().WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);         // 該当データがいないのでテキストは同じ
            });
        }

        [TestMethod, TestCategory("異常系")]
        public void CalledByNullInvoker()
        {
            AssertAction((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm1, TextItem>))
                    {
                        (command).Invoker = null;
                        (command).IsForSelf = false;
                    }
                }

            }, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);         // 該当データがいないのでテキストは同じ
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void CalledByExistedInvoker()
        {
            AssertAction((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm1, TextItem>))
                    {
                        (command).Invoker = forms.Last();
                        (command).IsForSelf = false;
                    }
                }


            }, (list, forms) =>
            {
                Assert.IsTrue(_was_validation); // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text); // 該当データがいないのでテキストは同じ            });
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void RecursiveFromExistedInvoker()
        {
            AssertAction((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm1, TextItem>))
                    {
                        (command).Invoker = forms.Last();
                        (command).IsForSelf = false;
                        (command).IsRecursive = true;
                    }
                }
            }, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);         // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);
                Assert.AreEqual("First Text, ChildForm1", forms.First().Text);         // 該当データがいないのでテキストは同じ          });
            });

        }

    }
}
