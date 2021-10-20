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

        [TestMethod]
        public void ProcessingTest()
        {
            bool was_validated = false;
            bool was_done_next = false;
            bool was_done_prev = false;
            bool was_done_finalize = false;
            bool was_done_error_handling = false;
            var base_form = new BaseForm();

            var new_command = new GenericCommand<BaseForm, TextItem>()
            {
                Validation = ((item) =>
                {
                    was_validated = true;
                    return true;
                }),
                NextOperation = ((item, form) =>
                {
                    form.Text = "Next Text";
                    was_done_next = true;
                }),
                PrevOperation = ((item, form) =>
                {
                    form.Text = "Previous Text";
                    was_done_prev = true;
                }),
                FinalOperation = ((item) => { was_done_finalize = true; }),
                ErrorOperation = ((item) => { was_done_error_handling = true; })
            };

            new_command.Validate();
            Assert.IsTrue(was_validated);
            Assert.IsFalse(was_done_next);
            Assert.IsFalse(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsFalse(was_done_error_handling);

            new_command.Next(base_form);
            Assert.IsTrue(was_validated);
            Assert.IsTrue(was_done_next);
            Assert.IsFalse(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsFalse(was_done_error_handling);
            Assert.AreEqual(base_form.Text, "Next Text");

            new_command.Prev(base_form);
            Assert.IsTrue(was_validated);
            Assert.IsTrue(was_done_next);
            Assert.IsTrue(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsFalse(was_done_error_handling);
            Assert.AreEqual(base_form.Text, "Previous Text");

            new_command.Invalidate();
            Assert.IsTrue(was_validated);
            Assert.IsTrue(was_done_next);
            Assert.IsTrue(was_done_prev);
            Assert.IsTrue(was_done_finalize);
            Assert.IsFalse(was_done_error_handling);
        }


        [TestMethod]
        public void ValidationErrorTest()
        {
            bool was_validated = false;
            bool was_done_next = false;
            bool was_done_prev = false;
            bool was_done_finalize = false;
            bool was_done_error_handling = false;
            var base_form = new BaseForm();
            base_form.Text = "First Text";

            var new_command = new GenericCommand<BaseForm, TextItem>()
            {
                Validation = ((item) =>
                {
                    was_validated = true;
                    return false;
                }),
                NextOperation = ((item, form) =>
                {
                    form.Text = "Next Text";
                    was_done_next = true;
                }),
                PrevOperation = ((item, form) =>
                {
                    form.Text = "Previous Text";
                    was_done_prev = true;
                }),
                FinalOperation = ((item) => { was_done_finalize = true; }),
                ErrorOperation = ((item) => { was_done_error_handling = true; })
            };

            new_command.Validate();
            Assert.IsTrue(was_validated);
            Assert.IsFalse(was_done_next);
            Assert.IsFalse(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsTrue(was_done_error_handling);

            new_command.Next(base_form);
            Assert.IsTrue(was_validated);
            Assert.IsFalse(was_done_next);
            Assert.IsFalse(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsTrue(was_done_error_handling);
            Assert.AreEqual(base_form.Text, "First Text");

            new_command.Prev(base_form);
            Assert.IsTrue(was_validated);
            Assert.IsFalse(was_done_next);
            Assert.IsFalse(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsTrue(was_done_error_handling);
            Assert.AreEqual(base_form.Text, "First Text");

            new_command.Invalidate();
            Assert.IsTrue(was_validated);
            Assert.IsFalse(was_done_next);
            Assert.IsFalse(was_done_prev);
            Assert.IsFalse(was_done_finalize);
            Assert.IsTrue(was_done_error_handling);

        }

    }
}
