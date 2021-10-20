using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Request.GenericCommand
{
    [TestClass]
    public class OperationTest
    {

        private GenericCommand<BaseForm, TextItem> _default_command;
        private bool _was_validated = false;
        private bool _was_done_next = false;
        private bool _was_done_prev = false;
        private bool _was_done_finalize = false;
        private bool _was_done_error_handling = false;


        public OperationTest()
        {
            _default_command = new GenericCommand<BaseForm, TextItem>()
            {
                Validation = ((item) =>
                {
                    _was_validated = true;
                    return true;
                }),
                NextOperation = ((item, form) =>
                {
                    form.Text = "Next Text";
                    _was_done_next = true;
                }),
                PrevOperation = ((item, form) =>
                {
                    form.Text = "Previous Text";
                    _was_done_prev = true;
                }),
                FinalOperation = ((item) => { _was_done_finalize = true; }),
                ErrorOperation = ((item) => { _was_done_error_handling = true; })
            };
        }

        [TestMethod]
        public void ProcessingTest()
        {
            var base_form = new BaseForm();

            _default_command.Validate();
            Assert.IsTrue(_was_validated);
            Assert.IsFalse(_was_done_next);
            Assert.IsFalse(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsFalse(_was_done_error_handling);

            _default_command.Next(base_form);
            Assert.IsTrue(_was_validated);
            Assert.IsTrue(_was_done_next);
            Assert.IsFalse(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsFalse(_was_done_error_handling);
            Assert.AreEqual("Next Text", base_form.Text);

            _default_command.Prev(base_form);
            Assert.IsTrue(_was_validated);
            Assert.IsTrue(_was_done_next);
            Assert.IsTrue(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsFalse(_was_done_error_handling);
            Assert.AreEqual("Previous Text", base_form.Text);

            _default_command.Invalidate();
            Assert.IsTrue(_was_validated);
            Assert.IsTrue(_was_done_next);
            Assert.IsTrue(_was_done_prev);
            Assert.IsTrue(_was_done_finalize);
            Assert.IsFalse(_was_done_error_handling);
        }


        [TestMethod]
        public void ValidationErrorTest()
        {
            var base_form = new BaseForm();
            base_form.Text = "First Text";

            ((GenericCommand<BaseForm, TextItem>) _default_command).Validation = (item) =>
            {
                _was_validated = true;
                return false;
            };

            _default_command.Validate();
            Assert.IsTrue(_was_validated);
            Assert.IsFalse(_was_done_next);
            Assert.IsFalse(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsTrue(_was_done_error_handling);

            _default_command.Next(base_form);
            Assert.IsTrue(_was_validated);
            Assert.IsFalse(_was_done_next);
            Assert.IsFalse(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsTrue(_was_done_error_handling);
            Assert.AreEqual("First Text", base_form.Text);

            _default_command.Prev(base_form);
            Assert.IsTrue(_was_validated);
            Assert.IsFalse(_was_done_next);
            Assert.IsFalse(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsTrue(_was_done_error_handling);
            Assert.AreEqual( "First Text", base_form.Text);

            _default_command.Invalidate();
            Assert.IsTrue(_was_validated);
            Assert.IsFalse(_was_done_next);
            Assert.IsFalse(_was_done_prev);
            Assert.IsFalse(_was_done_finalize);
            Assert.IsTrue(_was_done_error_handling);

        }

    }
}
