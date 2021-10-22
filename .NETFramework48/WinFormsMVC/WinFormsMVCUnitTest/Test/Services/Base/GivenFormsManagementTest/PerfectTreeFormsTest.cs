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
    public class PerfectTreeFormsTest : GivenFormManagementTestFormat
    {
        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
            set;
        }

        private void Define<T>(ref T instance, T default_instance) where T : class
        {
            if (instance == null)
            {
                instance = default_instance;
            }
        }

        private bool IsValidTestCalling(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (modified == null && assert == null && GetType() != typeof(PerfectTreeFormsTest))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public PerfectTreeFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreatePerfectTreeForms(DefaultBaseForm, BaseForm.MaxDepthTree, false);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseForm>(forms.First(), "Validation Text")
            });
        }

        // ---RootInvoker--- ///

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf_RootInvoker_Test(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {

            });

            Define( ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByRootInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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

            AssertForms<GivenFormsManagement>( modified, null, assert );
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromRootInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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
                Assert.AreEqual(forms.Count() - 1, throw_count);
            });


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        // --- SecondRoot Invoker ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySecondLeftInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.First().Children.First();
                (list.First()).IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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



            AssertForms<GivenFormsManagement>(modified, null, assert);
        }




        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromSecondLeftRootInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.First().Children.First();
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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

                int all_nodes_number = (int)(Math.Pow(2, BaseForm.MaxDepthTree - 2) - 1);
                Assert.AreEqual(all_nodes_number - 1, throw_count);
            });

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        // --- SecondRightInvoker --- ///

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySecondRightInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {


            Define(ref modified, (list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.First().Children.Last();
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }



        // --- LastInvoker ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf_LastInvoker_Test(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            
            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.Last();
            });

            Define(ref assert, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }                

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByLastInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).Invoker = forms.Last();
            });

            Define(ref assert, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromLastInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
                (list.First()).Invoker = forms.Last();
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }

            });

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }


        // --- First and Last Invokers ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByFirstAndLastInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.First();
                (list.First()).IsForSelf = false;

                list.Add(CreateDefaultCommand<BaseForm>(forms.Last(), "Validation Text - 2"));
                list.Last().IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        // ---All Left Invokers---

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf_AllLeftInvokers_Test(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            Define(ref modified, (list, forms) =>
            {
                list.First().IsForSelf = true;
                was_searched_left_method[forms.First()] = true;

                foreach (var form in forms)
                {
                    if (form != forms.First())
                    {
                        if (form.Children.Count() != 0 && form.Invoker.Children.First() == form
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
            });

            Define(ref assert, (list, forms) =>
            {

                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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

                Assert.AreEqual(BaseForm.MaxDepthTree - 1, throw_count);
            });


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByAllLeftInvokersTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            Define(ref modified, (list, forms) =>
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
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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

        // --- All Right Invokers ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByAllRightInvokersTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {


            var was_searched_left_method = new Dictionary<BaseForm, bool>();
            Define(ref modified, (list, forms) =>
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
            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

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


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }





        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByNullInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        (command).Invoker = null;
                        (command).IsForSelf = false;
                    }
                }

            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation); // Validationはされる
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }

            });

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }


        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void ValidationErrorTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
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

            });

            Define(ref assert, (list, forms) =>
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

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void ValidationNullCheckTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {


            Define(ref modified, (list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        ((GenericCommand<BaseForm, TextItem>)command).Validation = null;
                    }
                }

            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
            });


            AssertForms<GivenFormsManagement>(modified, null, assert);
        }
    }
}
