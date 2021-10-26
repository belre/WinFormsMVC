using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;
using WinFormsMVCUnitTest.Test.View.BaseForm;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class PerfectTreeFormsTest : GivenFormManagementTestFormat
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


        protected void Define<T>(ref T instance, T default_instance) where T : class
        {
            if (instance == null)
            {
                instance = default_instance;
            }
        }

        private bool IsValidTestCalling(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            if (modified == null && assert == null && GetType() != typeof(PerfectTreeFormsTest))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected Dictionary<BaseForm, List<Type>> TypeDictionary
        {
            get;
            set;
        }


        protected Command[] CreateDefaultCommandByTypeDictionary(BaseForm invoker, string validation_text)
        {
            List<Command> command_list = new List<Command>();

            if (TypeDictionary.Keys.Contains(invoker))
            {

                foreach (var type in TypeDictionary[invoker])
                {
                    var methodinfos = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var methodinfo in methodinfos)
                    {
                        // リフレクションでメソッドを生成
                        if (methodinfo.Name == "CreateDefaultCommand")
                        {
                            MethodInfo generic_mi = methodinfo.MakeGenericMethod(new Type[] { type });
                            var return_obj = generic_mi.Invoke(this, new object[] { invoker, validation_text });
                            command_list.Add((Command)return_obj);
                        }
                    }
                }

            }

            return command_list.ToArray();
    }


        public PerfectTreeFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreatePerfectTreeForms(DefaultBaseForm, BaseForm.MaxDepthTree, false);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseForm>(forms.First(), DefaultValidationText(0))
            });

            TypeDictionary = new Dictionary<BaseForm, List<Type>>();
            var form_list = BaseFormList;
            foreach (var form in form_list)
            {
                TypeDictionary[form] = new List<Type>()
                {
                    typeof(BaseForm)
                };
            }


            TestActionMode = ActionMode.SIMPLE_ACTION;
        }

        protected override void AssertAction(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.AssertSimpleAction(modified, assert);
        }


        // ---RootInvoker--- ///

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {

            });

            Define( ref assert, (list, forms) =>
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


            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
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

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (forms.First() == form.Invoker)
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2, throw_count);
            });

            AssertAction( modified, assert );
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                }
                Assert.AreEqual(forms.Count() - 1, throw_count);
            });


            AssertAction(modified, assert);
        }

        // --- SecondRoot Invoker ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySecondLeftInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.First().Children.First();
                (list.First()).IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {

                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker == forms.First().Children.First())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2, throw_count);
            });



            AssertAction(modified, assert);
        }




        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromSecondLeftRootInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.First().Children.First();
                (list.First()).IsForSelf = false;
                (list.First()).IsRecursive = true;
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                var is_ancestor_target = new Dictionary<BaseForm, bool>();
                is_ancestor_target[forms.First()] = false;
                is_ancestor_target[forms.First().Children.First()] = true;

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form == forms.First() || form == forms.First().Children.First())
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                    else if (is_ancestor_target[form.Invoker])
                    {
                        is_ancestor_target[form] = true;
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        is_ancestor_target[form] = false;
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                int all_nodes_number = (int)(Math.Pow(2, BaseForm.MaxDepthTree - 1) - 1);
                Assert.AreEqual(all_nodes_number - 1, throw_count);
            });

            AssertAction(modified, assert);
        }

        // --- SecondRightInvoker --- ///

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySecondRightInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {


            Define(ref modified, (list, forms) =>
            {
                ((GenericCommand<BaseForm, TextItem>)list.First()).Invoker = forms.First().Children.Last();
                ((GenericCommand<BaseForm, TextItem>)list.First()).IsForSelf = false;
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker == forms.First().Children.Last())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2, throw_count);

            });

            AssertAction(modified, assert);
        }



        // --- LastInvoker ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            
            Define(ref modified, (list, forms) =>
            {
                (list.First()).Invoker = forms.Last();
            });

            Define(ref assert, (list, forms) =>
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
        [DataRow(null, null)]
        public virtual void CalledByLastInvoker(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                (list.First()).IsForSelf = false;
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

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
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


        // --- First and Last Invokers ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
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

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker == forms.First())
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
                Assert.AreEqual(2, throw_count);
            });


            AssertAction(modified, assert);
        }

        // ---All Left Invokers---

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledBySelf_AllLeftInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            Define(ref modified, (list, forms) =>
            {
                was_searched_left_method[forms.First()] = true;

                foreach (var form in forms)
                {

                    {
                        if (form == forms.First() || form.Children.Count() != 0 && form.Invoker.Children.First() == form
                                                       && was_searched_left_method[form.Invoker])
                        {
                            var command_list = CreateDefaultCommandByTypeDictionary(form, DefaultValidationText(0));
                            foreach (var com in command_list)
                            {
                                com.IsForSelf = true;
                                list.Add(com);
                            }
                            was_searched_left_method[form] = true;
                        }
                        else
                        {
                            was_searched_left_method[form] = false;
                        }

                    }

                }
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (was_searched_left_method[form])
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(BaseForm.MaxDepthTree - 1, throw_count);
            });

            AssertAction(modified, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByAllLeftInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            var was_searched_left_method = new Dictionary<BaseForm, bool>();

            Define(ref modified, (list, forms) =>
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
                            var command_list = CreateDefaultCommandByTypeDictionary(form, DefaultValidationText(0));
                            foreach (var com in command_list)
                            {
                                com.IsForSelf = false;
                                com.Invoker = form;
                                list.Add(com);
                            }

                            was_searched_left_method[form] = true;
                        }
                        else
                        {
                            was_searched_left_method[form] = false;
                        }
                    }
                }
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker != null && was_searched_left_method[form.Invoker])
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2 * (BaseForm.MaxDepthTree - 1), throw_count);
            });

            AssertAction(modified, assert);
        }

        // --- All Right Invokers ---//

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void CalledByAllRightInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {


            var was_searched_right_method = new Dictionary<BaseForm, bool>();
            Define(ref modified, (list, forms) =>
            {
                list.First().IsForSelf = false;
                was_searched_right_method[forms.First()] = true;

                foreach (var form in forms)
                {
                    if (form != forms.First())
                    {
                        if (form.Children.Count() != 0 && form.Invoker.Children.Last() == form
                                                       && was_searched_right_method[form.Invoker])
                        {
                            var command_list = CreateDefaultCommandByTypeDictionary(form, DefaultValidationText(0));
                            foreach (var com in command_list)
                            {
                                com.IsForSelf = false;
                                com.Invoker = form;
                                list.Add(com);
                            }
                            was_searched_right_method[form] = true;
                        }
                        else
                        {
                            was_searched_right_method[form] = false;
                        }
                    }
                }
            });

            Define(ref assert, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue((list.First()).WasThroughValidation);

                int throw_count = 0;
                foreach (var form in forms)
                {
                    if (form.Invoker != null && was_searched_right_method[form.Invoker])
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }

                Assert.AreEqual(2 * (BaseForm.MaxDepthTree - 1), throw_count);

            });


            AssertAction(modified, assert);
        }





        [TestMethod, TestCategory("異常系")]
        [DataTestMethod]
        [DataRow(null, null)]
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
        [DataRow(null, null)]
        public virtual void ValidationError(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {

            Define(ref modified, (list, forms) =>
            {
                foreach (var command in list)
                {
                    if (command.GetType() == typeof(GenericCommand<BaseForm, TextItem>))
                    {
                        ((GenericCommand<BaseForm, TextItem>)command).Validation = (item) =>
                        {
                            item.Next = DefaultValidationText(0);
                            CommonCommandStatus.WasValidation = true;
                            return false;
                        };
                    }
                }

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
        [DataRow(null, null)]
        public virtual void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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
                CommonCommandStatus.AssertNotValidating();

                Assert.IsFalse((list.First()).WasThroughValidation);
                Assert.AreEqual(DefaultBaseForm.Text, forms.First().Text);
            });


            AssertAction(modified, assert);
        }
    }
}
