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
    public class SimplyConnectedTestFormat : GivenFormManagementTestFormat
    {

        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
            set;
        }



        // --- RootInvoker --- //

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void CalledBySelf_RootInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
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
                };
            }

            AssertForms<GivenFormsManagement>((list, forms) =>
            {

            }, null, assert);
        }


        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void CalledByRootInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);

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
                };
            }

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
            }, null, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void RecursiveFromRootInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);

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
                };
            }

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            }, null, assert);
        }


        // --- LastInvoker --- //

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void CalledBySelf_LastInvoker_Test(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
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
                };
            }


            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).Invoker = forms.Last();
            }, null, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void CalledByLastInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);

                    foreach (var form in forms)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                } ;
            }

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).Invoker = forms.Last();
            }, null, assert);
        }





        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void CalledByFirstAndLastInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);

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
                };
            }


            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).Invoker = forms.First();
                (list.First()).IsForSelf = false;

                list.Add(CreateDefaultCommand<BaseForm>(forms.Last(), "Validation Text - 2"));
                list.Last().IsForSelf = false;
            }, null, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void RecursiveFromLastInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);

                    foreach (var form in forms)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                };
            }

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
                (list.First()).Invoker = forms.Last();
            }, null, assert);
        }


        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void CalledByNullInvokerTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation); // Validationはされる
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);

                    foreach (var form in forms)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                };
            }

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

            }, null, assert);
        }


        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void ValidationErrorTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsTrue(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsTrue(_was_error);
                    Assert.IsTrue((list.First()).WasThroughValidation);
                    foreach (var form in forms)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
                    }
                };
            }

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

            }, null, assert);
        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null)]
        public virtual void ValidationNullCheckTest(Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (assert == null)
            {
                assert = (list, forms) =>
                {
                    Assert.IsFalse(_was_validation);
                    Assert.IsFalse(_was_finalize);
                    Assert.IsFalse(_was_error);
                    Assert.IsFalse((list.First()).WasThroughValidation);
                    Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
                };
            }

            AssertForms<GivenFormsManagement>((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        ((GenericCommand<BaseForm, TextItem>)command).Validation = null;
                    }
                }

            }, null, assert);
        }

    }
}
