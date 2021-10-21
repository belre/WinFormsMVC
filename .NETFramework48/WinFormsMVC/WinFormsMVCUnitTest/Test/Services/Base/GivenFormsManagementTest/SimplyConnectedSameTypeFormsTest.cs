using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View.BaseForm;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class SimplyConnectedSameTypeFormsTest : SimplyConnectedNodesTreeTest
    {
        private Command[] _default_commands;
        private bool _was_validation = false;
        private bool _was_finalize = false;
        private bool _was_error = false;

        public SimplyConnectedSameTypeFormsTest()
        {
            _default_commands = new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = ListFormsOrderedFromRoot.First(),
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
            }; 
        }

        [TestMethod, TestCategory("正常系")]
        public void BeCalledBySelf_InvokerRoot_Test()
        {
            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)_default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                if (form == ListFormsOrderedFromRoot.First())
                {
                    Assert.AreEqual("Validation Text", form.Text);
                }
                else
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }
        }



        [TestMethod, TestCategory("正常系")]
        public void BeCalledByInvokerRootTest()
        {
            ((GenericCommand<BaseForm, TextItem>) _default_commands.First()).IsForSelf = false;

            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)_default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                if (form == ListFormsOrderedFromRoot.Skip(1).First())
                {
                    Assert.AreEqual("Validation Text", form.Text);
                }
                else
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }
        }

        [TestMethod, TestCategory("正常系")]
        public void BeRetrievedByInvokerRootTest()
        {
            ((GenericCommand<BaseForm, TextItem>)_default_commands.First()).IsForSelf = false;
            ((GenericCommand<BaseForm, TextItem>)_default_commands.First()).IsRetrieved = true;

            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)_default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                if (form == ListFormsOrderedFromRoot.First())
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
                else
                {
                    Assert.AreEqual("Validation Text", form.Text);
                }
            }
        }



        [TestMethod, TestCategory("正常系")]
        public void BeCalledBySelf_InvokerLast_Test()
        {
            ((GenericCommand<BaseForm, TextItem>) _default_commands.First()).Invoker = ListFormsOrderedFromRoot.Last();

            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)_default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                if (form == ListFormsOrderedFromRoot.Last())
                {
                    Assert.AreEqual("Validation Text", form.Text);
                }
                else
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }
            }
        }                

        [TestMethod, TestCategory("正常系")]
        public void BeCalledByInvokerLastTest()
        {
            ((GenericCommand<BaseForm, TextItem>) _default_commands.First()).IsForSelf = false;
            ((GenericCommand<BaseForm, TextItem>)_default_commands.First()).Invoker = ListFormsOrderedFromRoot.Last();

            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)_default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                Assert.AreEqual(DefaultBaseForm.Text, form.Text);
            }
        }

        [TestMethod, TestCategory("正常系")]
        public void BeRetrievedByInvokerLastTest()
        {
            ((GenericCommand<BaseForm, TextItem>)_default_commands.First()).IsForSelf = false;
            ((GenericCommand<BaseForm, TextItem>)_default_commands.First()).IsRetrieved = true;
            ((GenericCommand<BaseForm, TextItem>)_default_commands.First()).Invoker = ListFormsOrderedFromRoot.Last();

            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>)_default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                Assert.AreEqual(DefaultBaseForm.Text, form.Text);
            }
        }


        [TestMethod, TestCategory("異常系")]
        public void BeCalledByNullInvokerTest()
        {
            foreach (var command in ((Command[]) _default_commands))
            {
                if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                {
                    ((GenericCommand<BaseForm, TextItem>) command).Invoker = null;
                    ((GenericCommand<BaseForm, TextItem>) command).IsForSelf = false;
                }
            }

            var given_form_obj = new GivenFormsManagement(ListFormsOrderedFromRoot);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation); // Validationはされる
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.IsTrue(((GenericCommand<BaseForm, TextItem>) _default_commands[0]).WasThroughValidation);

            foreach (var form in ListFormsOrderedFromRoot)
            {
                Assert.AreEqual(DefaultBaseForm.Text, form.Text);
            }
        }        
    }
}
