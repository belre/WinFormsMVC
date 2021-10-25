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
    public class PerfectTreeGivenFormsTest : PerfectTreeFormsTest
    {
        public PerfectTreeGivenFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreatePerfectTreeForms(DefaultBaseForm, BaseForm.MaxDepthTree, true);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseFormModel.ChildForm2>(forms.First(), "Validation Text")
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_RootInvoker_Test(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySelf_RootInvoker_Test((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.First(), "Validation Text");
            }, null);
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromRootInvokerTest(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromRootInvokerTest(null, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (!forms.First().Children.Contains(form))
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                }

                Assert.AreEqual(2, throw_count);
            });

        }


        // --- SecondRoot Invoker ---//
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySecondLeftInvokerTest(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySecondLeftInvokerTest((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm3>(forms.First().Children.First(), "Validation Text");
                list[0].IsForSelf = false;
            }, null);
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromSecondLeftRootInvokerTest(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromSecondLeftRootInvokerTest(null, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                var is_ancestor_target = new Dictionary<BaseForm, bool>();
                is_ancestor_target[forms.First()] = false;
                is_ancestor_target[forms.First().Children.First()] = true;

                int throw_count = 0;
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                }

                Assert.AreEqual(0, throw_count);
            });

        }

        // --- SecondRightInvoker--//
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySecondRightInvokerTest(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySecondRightInvokerTest((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm3>(forms[0].Children.Last(), "Validation Text");
                list[0].IsForSelf = false;
            }, null);
        }


        // --- LastInvoker --- //

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_LastInvoker_Test(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySelf_LastInvoker_Test((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm5>(forms.Last(), "Validation Text");
                list[0].IsForSelf = true;
            }, null);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_AllLeftInvokers_Test(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            base.CalledBySelf_AllLeftInvokers_Test((list, forms) =>
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
                            var com = CreateDefaultCommand<BaseFormModel.ChildForm2>(form, "Validation Text");
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
            }, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form == forms.Skip(1).First())
                    {
                        Assert.AreEqual("Validation Text", form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(1, throw_count);
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledByAllLeftInvokersTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            base.CalledByAllLeftInvokersTest((list, forms) =>
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
                            var com = CreateDefaultCommand<BaseFormModel.ChildForm2>(form, "Validation Text");
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
            }, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if ( forms.First().Children.Contains(form))
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
        }
        
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledByAllRightInvokersTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            base.CalledByAllLeftInvokersTest((list, forms) =>
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
            }, (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (forms.First().Children.Contains(form))
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
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationErrorTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.ValidationErrorTest( ((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm2, TextItem>))
                    {
                        ((GenericCommand<BaseFormModel.ChildForm2, TextItem>)command).Validation = (item) =>
                        {
                            item.Next = "Validation Text";
                            _was_validation = true;
                            return false;
                        };
                    }
                }
            }), (commands, forms) =>
            {
                Assert.IsTrue(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsTrue(_was_error);
                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
                }

            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationNullCheckTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.ValidationNullCheckTest(((list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseFormModel.ChildForm2, TextItem>))
                    {
                        ((GenericCommand<BaseFormModel.ChildForm2, TextItem>)command).Validation = null;
                    }
                }
            }), (commands, forms) =>
            {
                Assert.IsFalse(_was_validation);
                Assert.IsFalse(_was_finalize);
                Assert.IsFalse(_was_error);
                Assert.IsFalse((commands.First()).WasThroughValidation);
                Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
            });

        }
    }
}
