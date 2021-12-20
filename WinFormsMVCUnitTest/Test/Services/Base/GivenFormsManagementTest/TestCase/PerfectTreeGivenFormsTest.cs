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

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
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

            var forms = (new BaseFormModel()).CreatePerfectTreeForms(DefaultBaseForm, BaseForm.MaxDepthTree, true);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseFormModel.ChildForm2>(forms.First(), DefaultValidationText(0))
            });


            TestActionMode = ActionMode.SIMPLE_ACTION;
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_RootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySelf_RootInvoker((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.First(), DefaultValidationText(0));
            }, null);
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromRootInvoker((list, forms) =>
            {
                list.Clear();
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm2>(DefaultBaseForm, DefaultValidationText(0)));
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm3>(DefaultBaseForm, DefaultValidationText(0)));
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm4>(DefaultBaseForm, DefaultValidationText(0)));
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm5>(DefaultBaseForm, DefaultValidationText(0)));

                foreach (var com in list)
                {
                    com.Invoker = forms.First();
                    com.IsForSelf = false;
                    com.IsRecursiveToChildren = true;
                }

            }, null);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromRootInvokerInSingleLevel(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();
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
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                        throw_count++;
                    }
                }

                Assert.AreEqual(2, throw_count);
            });

            base.RecursiveFromRootInvoker(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromLastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromLastInvoker(((list, forms) =>
            {
                var command = CreateDefaultCommand<BaseFormModel.ChildForm5>(DefaultBaseForm, DefaultValidationText(0));
                command.IsForSelf = false;
                command.IsRecursiveToChildren = true;
                command.Invoker = forms.Last();

                list.Clear();
                list.Add(command);
            }), assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void AllNodesFromLastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromLastInvoker(((list, forms) =>
            {
                var command = CreateDefaultCommand<BaseFormModel.ChildForm5>(DefaultBaseForm, DefaultValidationText(0));
                command.IsForSelf = false;
                command.IsAll = true;
                command.Invoker = forms.Last();

                list.Clear();
                list.Add(command);
            }), (commands, forms) =>
            {
                CommonCommandStatus.AssertValidated();

                Assert.IsTrue((commands.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    if (form.GetType().Equals(typeof(BaseFormModel.ChildForm5)))
                    {
                        Assert.AreEqual(DefaultValidationText(0), form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultBaseForm.Text, form.Text);
                    }
                }
            });
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveForAncestorFromLastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveForAncestorFromLastInvoker(((list, forms) =>
            {
                var command = CreateDefaultCommand<BaseFormModel.ChildForm1>(DefaultBaseForm, DefaultValidationText(0));
                command.IsForSelf = false;
                command.IsRecursiveForAncestor = true;
                command.Invoker = forms.Last();

                list.Clear();
                list.Add(command);
            }), (list, forms) =>
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
            } );
        }


        // --- SecondRoot Invoker ---//
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySecondLeftInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySecondLeftInvoker((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm3>(forms.First().Children.First(), DefaultValidationText(0));
                list[0].IsForSelf = false;
            }, assert);
        }


        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void RecursiveFromSecondLeftRootInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.RecursiveFromSecondLeftRootInvoker((list, forms) =>
            {
                list.Clear();
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm2>(DefaultBaseForm, DefaultValidationText(0)));
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm3>(DefaultBaseForm, DefaultValidationText(0)));
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm4>(DefaultBaseForm, DefaultValidationText(0)));
                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm5>(DefaultBaseForm, DefaultValidationText(0)));

                foreach (var com in list)
                {
                    com.Invoker = forms.First().Children.First();
                    com.IsForSelf = false;
                    com.IsRecursiveToChildren = true;
                }

            }, assert);
        }

        [TestMethod, TestCategory("正常系")]
        [DataTestMethod]
        [DataRow(null, null)]
        public virtual void RecursiveFromSecondLeftRootInvokerInSingleLevel(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref assert, (commands, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
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

            base.RecursiveFromSecondLeftRootInvoker(modified, assert);
        }

        // --- SecondRightInvoker--//
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySecondRightInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySecondRightInvoker((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm3>(forms[0].Children.Last(), DefaultValidationText(0));
                list[0].IsForSelf = false;
            }, assert);
        }


        // --- LastInvoker --- //

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_LastInvoker(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            base.CalledBySelf_LastInvoker((list, forms) =>
            {
                list[0] = CreateDefaultCommand<BaseFormModel.ChildForm5>(forms.Last(), DefaultValidationText(0));
                list[0].IsForSelf = true;
            }, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledBySelf_AllLeftInvokers(Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            // 型生成
            foreach (var form in BaseFormList)
            {
                var objtype = (new BaseFormModel()).DefinedChildForms.Single(type => type == form.GetType());
                var type_list = new List<Type>();

                if (objtype != null )
                {
                    type_list.Add(objtype);
                }

                TypeDictionary[form] = type_list;
            }


            base.CalledBySelf_AllLeftInvokers(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledByAllLeftInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            // 型生成
            foreach (var form in BaseFormList)
            {
                var obj = (new BaseFormModel()).DefinedChildForms.Single(type => type == form.GetType() );
                var type_list = new List<Type>();
                
                if (obj != null && obj != BaseFormList.Last().GetType())
                {
                    var next = (new BaseFormModel()).DefinedChildForms.SkipWhile(type => type != form.GetType()).Skip(1).First();
                    type_list.Add(next);
                }

                TypeDictionary[form] = type_list;
            }

            base.CalledByAllLeftInvokers(modified, assert);
        }
        
        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void CalledByAllRightInvokers(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            // 型生成

            foreach (var form in BaseFormList)
            {
                var obj = (new BaseFormModel()).DefinedChildForms.Single(type => type == form.GetType());
                var type_list = new List<Type>();

                if (obj != null && obj != BaseFormList.Last().GetType())
                {
                    var next = (new BaseFormModel()).DefinedChildForms.SkipWhile(type => type != form.GetType()).Skip(1).First();
                    type_list.Add(next);
                }

                TypeDictionary[form] = type_list;
            }

            base.CalledByAllRightInvokers(modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationError(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
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

            base.ValidationError( modified, assert);
        }

        [TestMethod, TestCategory("差分")]
        [DataTestMethod]
        [DataRow(null, null)]
        public override void ValidationNullCheck(Action<List<Command>, List<BaseForm>> modified, Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            Define(ref modified, (list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = null;
            });


            base.ValidationNullCheck(modified, assert);

        }
    }
}
