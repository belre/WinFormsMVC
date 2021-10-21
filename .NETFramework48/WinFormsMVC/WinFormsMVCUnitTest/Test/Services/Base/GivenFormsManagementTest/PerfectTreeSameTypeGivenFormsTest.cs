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
        public void RecursiveFromRootInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
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
        public void RecursiveFromSecondRootInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).Invoker = forms.First().Children.First();
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            }, null, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                var is_ancestor_target = new Dictionary<BaseForm, bool>();
                is_ancestor_target[forms.First()] = false;
                is_ancestor_target[forms.First().Children.First()] = true;

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form == forms.First() || form == forms.First().Children.First()) 
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else if (is_ancestor_target[form.Invoker])
                    {
                        is_ancestor_target[form] = true;
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        is_ancestor_target[form] = false;
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
                Assert.AreEqual(2 * (BaseForm.MaxDepthTree - 2), throw_count);
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
                (list.First()).Invoker = forms.First();
                (list.First()).IsForSelf = false;

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
                (list.First()).Invoker = forms.First().Children.First();
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
        public void CalledBySelf_AllLeftInvokers_Test()
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();


            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                list.First().IsForSelf = true;
                was_searched_left_method[forms.First()] = true;

                foreach (var form in forms)
                {
                    if (form != forms.First())
                    {
                        if ( form.Children.Count() != 0 && form.Invoker.Children.First() == form
                            && was_searched_left_method[form.Invoker])
                        {
                            var com = CreateDefaultCommand<BaseForm>(form, "Validation Text");
                            com.IsForSelf = true;
                            list.Add(com);
                            was_searched_left_method[form] = true;
                        }
                        else
                        {
                            was_searched_left_method[form] = false;
                        }
                    }
                }
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (was_searched_left_method[form])
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(BaseForm.MaxDepthTree-1, throw_count);
            });
        }


        [TestMethod, TestCategory("正常系")]
        public void CalledByAllLeftInvokersTest()
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();


            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                list.First().IsForSelf = false;
                was_searched_left_method[forms.First()] = true;

                foreach (var form in forms)
                {
                    if (form != forms.First())
                    {
                        if (form.Children.Count() != 0 && form.Invoker.Children.First() == form
                            && was_searched_left_method[form.Invoker])
                        {
                            var com = CreateDefaultCommand<BaseForm>(form, "Validation Text");
                            com.IsForSelf = false;
                            list.Add(com);
                            was_searched_left_method[form] = true;
                        }
                        else
                        {
                            was_searched_left_method[form] = false;
                        }
                    }
                }
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker != null && was_searched_left_method[form.Invoker])
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2 * (BaseForm.MaxDepthTree - 2), throw_count);
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void CalledByAllRightInvokersTest()
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();


            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                list.First().IsForSelf = false;
                was_searched_left_method[forms.First()] = true;

                foreach (var form in forms)
                {
                    if (form != forms.First())
                    {
                        if (form.Children.Count() != 0 && form.Invoker.Children.Last() == form
                            && was_searched_left_method[form.Invoker])
                        {
                            var com = CreateDefaultCommand<BaseForm>(form, "Validation Text");
                            com.IsForSelf = false;
                            list.Add(com);
                            was_searched_left_method[form] = true;
                        }
                        else
                        {
                            was_searched_left_method[form] = false;
                        }
                    }
                }
            }, null, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list[0]).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker != null && was_searched_left_method[form.Invoker])
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2 * (BaseForm.MaxDepthTree - 2), throw_count);
            });
        }

        [TestMethod, TestCategory("正常系")]
        public void CalledByRightSecondaryInvokerTest()
        {
            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.First().Children.Last();
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
                    if (form.Invoker == forms.First().Children.Last())
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
        public void RecursiveFromLastInvokerTest()
        {

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
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
