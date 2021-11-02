using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Controller.CommandController
{
    [TestClass]
    public class SendStoredMessageAndUndoTest : FacadeAndFormManagementTestFormat
    {
        private int _count_notified_after_action = 0;
        private bool _is_available_undo = false;

        public SendStoredMessageAndUndoTest()
        {
            
        }


        [TestMethod]
        public void SendJustStoredMessage()
        {
            var form_initiate = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            controller.SendStoredMessage(new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    Validation = (item) =>
                    {
                        item.Next = form_initiate.Text;
                        return true;
                    },
                    NextOperation = (item, form) =>
                    {
                        form.Text = item.Next;
                    }
                }
            }, NotifyAfterSomeAction);

            Assert.AreEqual(1, _count_notified_after_action);
            Assert.AreEqual(form_initiate.Text, form_second.Text);

            controller.Undo(NotifyAfterSomeAction);

            Assert.AreEqual(2, _count_notified_after_action);
            Assert.IsFalse(_is_available_undo);
        }

        [TestMethod]
        public void SendStoredMessageTwice()
        {
            var form_initiate = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);
            form_second.Text = "That's great";

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            controller.SendStoredMessage(new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    Validation = (item) =>
                    {
                        item.Next = "Excuse me";
                        return true;
                    },
                    NextOperation = (item, form) =>
                    {
                        item[form] = form.Text;
                        form.Text = item.Next;
                    },
                    PrevOperation = (item, form) =>
                    {
                        form.Text = item[form];
                    }
                }
            }, NotifyAfterSomeAction);
            controller.SendStoredMessage(new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    Validation = (item) =>
                    {
                        item.Next = form_initiate.Text;
                        return true;
                    },
                    NextOperation = (item, form) =>
                    {
                        item[form] = form.Text;
                        form.Text = item.Next;
                    },
                    PrevOperation = (item, form) =>
                    {
                        form.Text = item[form];
                    }
                }
            }, NotifyAfterSomeAction);

            Assert.AreEqual(form_initiate.Text, form_second.Text);

            controller.Undo(NotifyAfterSomeAction);

            Assert.AreEqual(3, _count_notified_after_action);
            Assert.AreEqual("Excuse me", form_second.Text);
            Assert.IsTrue(_is_available_undo);
            Assert.IsTrue(controller.IsAvailableUndo);
        }

        [TestMethod]
        public void UndoNull()
        {
            var form_initiate = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            controller.SendStoredMessage(new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    Validation = (item) =>
                    {
                        item.Next = form_initiate.Text;
                        return true;
                    },
                    NextOperation = (item, form) =>
                    {
                        form.Text = item.Next;
                    }
                }
            }, NotifyAfterSomeAction);

            Assert.AreEqual(1, _count_notified_after_action);
            Assert.AreEqual(form_initiate.Text, form_second.Text);

            controller.Undo(null);

            Assert.AreEqual(1, _count_notified_after_action);
            Assert.IsTrue(_is_available_undo);              // Undoのときに実行していないのでtrueとなる
            Assert.IsFalse(controller.IsAvailableUndo);
        }

        [TestMethod]
        public void SendTwoStoredMessageTwice()
        {
            var form_initiate = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);
            form_second.Text = "That's great";

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            controller.SendStoredMessage(new Command[]
            {
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    Validation = (item) =>
                    {
                        item.Next = "abcdefg";
                        return true;
                    },
                    NextOperation = (item, form) =>
                    {
                        item[form] = form.Text;
                        form.Text = item.Next;
                    }, 
                    PrevOperation = (item, form) =>
                    {
                        form.Text = item[form];
                    }
                },
                new GenericCommand<BaseForm, TextItem>()
                {
                    Invoker = form_initiate,
                    Validation = (item) =>
                    {
                        item.Next = form_initiate.Text;
                        return true;
                    },
                    NextOperation = (item, form) =>
                    {
                        item[form] = form.Text;
                        form.Text = item.Next;
                    },
                    PrevOperation = (item, form) =>
                    {
                        form.Text = item[form];
                    }
                }
            }, NotifyAfterSomeAction);

            Assert.AreEqual(1, _count_notified_after_action);
            Assert.AreEqual(form_initiate.Text, form_second.Text);

            controller.Undo(NotifyAfterSomeAction);

            Assert.AreEqual(2, _count_notified_after_action);
            Assert.AreEqual("That's great", form_second.Text);
            Assert.IsFalse(_is_available_undo);
            Assert.IsFalse(controller.IsAvailableUndo);
        }

        [TestMethod]
        public void SendStoredMessageMaxTimes()
        {
            var form_initiate = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);
            form_second.Text = "That's great";

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            for (int i = 0; i < Manager.ManagedMemento.MAX_MEMEMTO_NUMBER+1; i++)
            {
                controller.SendStoredMessage(new Command[]
                {
                    new GenericCommand<BaseForm, TextItem>()
                    {
                        Invoker = form_initiate,
                        Validation = (item) =>
                        {
                            item.Next = string.Format("Message {0}", i);
                            return true;
                        },
                        NextOperation = (item, form) =>
                        {
                            item[form] = form.Text;
                            form.Text = item.Next;
                        },
                        PrevOperation = (item, form) =>
                        {
                            form.Text = item[form];
                        }
                    }
                }, NotifyAfterSomeAction);
            }

            Assert.AreEqual(Manager.ManagedMemento.MAX_MEMEMTO_NUMBER + 1, _count_notified_after_action);
            Assert.AreEqual(string.Format("Message {0}", Manager.ManagedMemento.MAX_MEMEMTO_NUMBER ), form_second.Text);


            for (int i = 0; i < Manager.ManagedMemento.MAX_MEMEMTO_NUMBER; i++)
            {
                int no = Manager.ManagedMemento.MAX_MEMEMTO_NUMBER - i;
                Assert.AreEqual(string.Format("Message {0}", no), form_second.Text);

                controller.Undo(NotifyAfterSomeAction);
                Assert.AreEqual(Manager.ManagedMemento.MAX_MEMEMTO_NUMBER + 1 + i + 1, _count_notified_after_action);

                Assert.AreEqual(string.Format("Message {0}", no-1), form_second.Text);
            }

            Assert.AreEqual(string.Format("Message {0}", 0), form_second.Text);
            Assert.IsFalse(_is_available_undo);
            Assert.IsFalse(controller.IsAvailableUndo);
        }



        [TestMethod]
        public void SendNullAsStoredMessage()
        {
            var form_initiate = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_initiate, false);
            Manager.LaunchForm(null, form_initiate, false);
            form_initiate.Text = "Hello";

            var form_second = new BaseForm();
            BaseFormModel.AddInitialAttributes(form_second, false);
            Manager.LaunchForm(form_initiate, form_second, false);

            var controller = Facade.GetController<WinFormsMVC.Controller.CommandController>(form_initiate);
            Assert.ThrowsException<NullReferenceException>(() => controller.SendStoredMessage(null, null));
        }

        private void NotifyAfterSomeAction(bool is_available_undo)
        {
            _count_notified_after_action++;
            _is_available_undo = is_available_undo;
        }

    }
}
