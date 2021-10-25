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
    public class SimplyConnectedFormsTest : GivenFormManagementTestFormat
    {

        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
            set;
        }

        protected string DefaultValidationText
        {
            get
            {
                return "Validation Text";
            }
        }

        private void Define<T>( ref T instance, T default_instance) where T : class
        {
            if (instance == null)
            {
                instance = default_instance;
            }
        }

        private bool IsValidTestCalling(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if( modified == null && assert == null && GetType() != typeof(SimplyConnectedFormsTest))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public SimplyConnectedFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreateSimplyConnectedForms(DefaultBaseForm, BaseForm.MaxDepthTree);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseForm>(forms.First(), DefaultValidationText)
            });
        }

        // --- RootInvoker --- //

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledBySelf_RootInvoker_Test(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            
            Define( ref modified, (list, forms) =>
            {

            });

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(DefaultValidationText, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            AssertForms<GivenFormsManagement>( modified, null, assert);
        }


        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
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

                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreEqual(DefaultValidationText, form.Text);
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
        [DataRow(null,null)]
        public virtual void RecursiveFromRootInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, ((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            }));

            Define(ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (forms.First() == form)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultValidationText, form.Text);
                    }
                }
            });

            AssertForms<GivenFormsManagement>(modified, null, assert);
        }


        // --- LastInvoker --- //

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledBySelf_LastInvoker_Test(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, ((list, forms) =>
            {
                list.First().Invoker = forms.Last();
            }));

            Define( ref assert, (list, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Last())
                    {
                        Assert.AreEqual(DefaultValidationText, form.Text);
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
        [DataRow(null,null)]
        public virtual void CalledByLastInvokerTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).Invoker = forms.Last();
            });

            Define( ref assert, (list, forms) =>
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
        [DataRow(null,null)]
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

                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreEqual(DefaultValidationText, form.Text);
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
        [DataRow(null,null)]
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


        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null,null)]
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
        [DataRow(null,null)]
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
                            item.Next = DefaultValidationText;
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
        [DataRow(null,null)]
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
