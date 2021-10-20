using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class SingleFormNodesTest
    {
        public class ChildForm1 : BaseForm
        {

        }

        private List<BaseForm> _form_list;

        private Command[] _default_commands;
        private bool _was_validation = false;
        private bool _was_finalize = false;
        private bool _was_error = false;

        public SingleFormNodesTest()
        {
            _form_list = new List<BaseForm>()
            {
                new ChildForm1()
            };

            _default_commands = new Command[]
            {
                new GenericCommand<ChildForm1, TextItem>()
                {
                    Invoker = _form_list.First(),
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


        [TestMethod]
        public void RunTest()
        {
            _form_list.First().Text = "First Text";

            var given_form_obj = new GivenFormsManagement(_form_list);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.AreEqual(_form_list.First().Text, "First Text");
        }

        [TestMethod]
        public void ValidationErrorTest()
        {
            _form_list.First().Text = "First Text";

            ((GenericCommand<ChildForm1, TextItem>)_default_commands[0]).Validation = (item) =>
            {
                item.Next = "Validation Text";
                _was_validation = true;
                return false;
            };

            var given_form_obj = new GivenFormsManagement(_form_list);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsTrue(_was_error);
            Assert.AreEqual(_form_list.First().Text, "First Text");
        }

    }
}
