using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class SimplyConnectedFormsTest : GivenFormManagementTestFormat
    {

        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
            set;
        }



        protected string DefaultValidationText(int seq)
        {
            return string.Format("{0} - {1}", "Validation Text", seq);
        }


        protected void Define<T>( ref T instance, T default_instance) where T : class
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
                CreateDefaultCommand<BaseForm>(forms.First(), DefaultValidationText(0))
            });


            TestActionMode = ActionMode.SIMPLE_ACTION;
        }

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertSimpleAction(modified, assert);
        }

        // --- RootInvoker --- //

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            
            Define( ref modified, (list, forms) =>
            {

            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            AssertAction( modified, assert);
        }


        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledByRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, ((list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            }));

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (forms.First() == form)
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                }
            });

            AssertAction(modified, assert);
        }


        // --- LastInvoker --- //

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, ((list, forms) =>
            {
                list.First().Invoker = forms.Last();
            }));

            Define( ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Last())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledByLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).Invoker = forms.Last();
            });

            Define( ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            AssertAction(modified, assert);
        }





        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledByFirstAndLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });

            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void RecursiveFromLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
                (list.First()).Invoker = forms.Last();
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });


            AssertAction(modified, assert);

        }


        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void CalledByNullInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            });

            AssertAction(modified, assert);
        }


        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void ValidationError(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = (item) =>
                {
                    item.Next = DefaultValidationText(0);
                    CommonCommandStatus.WasValidation = true;
                    return false;
                };
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidationError();
                Assert.IsTrue((list.First()).WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
                }
            });


            AssertAction(modified, assert);

        }

        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null,null)]
        public virtual void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = null;
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
                Assert.IsFalse((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
            });


            AssertAction(modified, assert);
        }

    }
}
