﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class IsonatedFormsNodesTest
    {
        public class ChildForm1 : BaseForm
        {

        }

        public class ChildForm2 : BaseForm
        {

        }

        public class ChildForm3 : BaseForm
        {

        }

        private List<BaseForm> _form_list;

        private Command[] _default_commands;
        private bool _was_validation = false;
        private bool _was_finalize = false;
        private bool _was_error = false;

        public IsonatedFormsNodesTest()
        {
            _form_list = new List<BaseForm>()
            {
                new ChildForm1()
                {
                    Text = "First Text, ChildForm1" 
                },
                new ChildForm2()
                {
                    Text = "First Text, ChildForm2-1"
                },
                new ChildForm2()
                {
                    Text = "First Text, ChildForm2-2"
                },
                new ChildForm3()
                {
                    Text = "First Text, ChildForm3"
                }
            };

            _default_commands = new Command[]
            {
                new GenericCommand<ChildForm1, TextItem>()
                {
                    Invoker = _form_list[0],
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
            };
        }


        [TestMethod]
        public void RunTest()
        {
            var given_form_obj = new GivenFormsManagement(_form_list);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.AreEqual(_form_list.First().Text, "Validation Text - ChildForm1");
        }

        [TestMethod]
        public void ValidationErrorTest()
        {
            foreach (var command in ((Command[])_default_commands))
            {
                if( command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                {
                    ((GenericCommand<ChildForm1, TextItem>)command).Validation = (item) =>
                    {
                        item.Next = "Validation Text";
                        _was_validation = true;
                        return false;
                    };
                }
            }

            var given_form_obj = new GivenFormsManagement(_form_list);
            given_form_obj.Run(_default_commands);

            Assert.IsTrue(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsTrue(_was_error);
            Assert.AreEqual(_form_list.First().Text, "First Text, ChildForm1");
        }

        [TestMethod]
        public void ValidationNullCheckTest()
        {
            foreach (var command in ((Command[]) _default_commands))
            {
                if (command.GetType() == typeof(GenericCommand<ChildForm1, TextItem>))
                {
                    ((GenericCommand<ChildForm1, TextItem>) command).Validation = null;
                }
            }

            var given_form_obj = new GivenFormsManagement(_form_list);
            given_form_obj.Run(_default_commands);

            Assert.IsFalse(_was_validation);
            Assert.IsFalse(_was_finalize);
            Assert.IsFalse(_was_error);
            Assert.AreEqual(_form_list.First().Text, "First Text, ChildForm1");
        }

    }
}
