using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
                CreateDefaultCommand<BaseFormModel.ChildForm2>(forms.First(), DefaultValidationText)
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
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.First(), DefaultValidationText);
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
                        Assert.AreEqual(DefaultValidationText, form.Text);
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
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm3>(forms.First().Children.First(), DefaultValidationText);
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
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm3>(forms[0].Children.Last(), DefaultValidationText);
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
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm5>(forms.Last(), DefaultValidationText);
                list[0].IsForSelf = true;
            }, null);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_AllLeftInvokers_Test(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            // 型生成
            foreach (var form in BaseFormList)
            {
                var objtype = BaseFormModel.DefinedChildForms.Single(type => type == form.GetType());
                var type_list = new List<Type>();

                if (objtype != null )
                {
                    type_list.Add(objtype);
                }

                TypeDictionary[form] = type_list;
            }


            base.CalledBySelf_AllLeftInvokers_Test(null, null);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledByAllLeftInvokersTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            // 型生成
            foreach (var form in BaseFormList)
            {
                var obj = BaseFormModel.DefinedChildForms.Single(type => type == form.GetType() );
                var type_list = new List<Type>();
                
                if (obj != null && obj != BaseFormList.Last().GetType())
                {
                    var next = BaseFormModel.DefinedChildForms.SkipWhile(type => type != form.GetType()).Skip(1).First();
                    type_list.Add(next);
                }

                TypeDictionary[form] = type_list;
            }

            base.CalledByAllLeftInvokersTest(null, null);
        }
        
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledByAllRightInvokersTest(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            // 型生成

            foreach (var form in BaseFormList)
            {
                var obj = BaseFormModel.DefinedChildForms.Single(type => type == form.GetType());
                var type_list = new List<Type>();

                if (obj != null && obj != BaseFormList.Last().GetType())
                {
                    var next = BaseFormModel.DefinedChildForms.SkipWhile(type => type != form.GetType()).Skip(1).First();
                    type_list.Add(next);
                }

                TypeDictionary[form] = type_list;
            }

            base.CalledByAllLeftInvokersTest(null, null);
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
                            item.Next = DefaultValidationText;
                            _was_validation = true;
                            return false;
                        };
                    }
                }
            }), null);
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
            }), null);

        }
    }
}
