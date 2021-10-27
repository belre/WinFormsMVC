﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest.TestCase
{
    [TestClass]
    public class IsolatedGivenFormsTest : GivenFormManagementTestFormat
    {
        protected IEnumerable<string> FormText
        {
            get;
        }

        protected Dictionary<BaseForm, string> DefaultTextDictionary
        {
            get;
        }

        protected string ValidationText
        {
            get
            {
                return "Validation Text";
            }
        }

        protected override void AssertAction(
            Action<List<Command>, List<BaseForm>> modified,
            Action<IEnumerable<Command>, IEnumerable<BaseForm>> assert)
        {
            AssertSimpleAction(modified, assert);
        }

        public IsolatedGivenFormsTest()
        {
            DefaultTextDictionary = new Dictionary<BaseForm, string>();

            Func<BaseForm, string, BaseForm> allocator = (form, s) =>
            {
                DefaultTextDictionary[form] = s;
                form.Text = s;
                return form;
            };

            var forms = new List<BaseForm>();
            forms.Add(allocator(new BaseFormModel.ChildForm1(), "First Text, ChildForm1"));
            forms.Add(allocator(new BaseFormModel.ChildForm2(), "First Text, ChildForm2-1"));
            forms.Add(allocator(new BaseFormModel.ChildForm2(), "First Text, ChildForm2-2"));
            forms.Add(allocator(new BaseFormModel.ChildForm3(), "First Text, ChildForm3"));
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.First(), ValidationText)
            });

            TestActionMode = ActionMode.SIMPLE_ACTION;
        }


        [TestMethod, TestCategory("正常系")]
        public virtual void CalledBySelf()
        {
            AssertAction((list, forms) =>
            {

            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidated();
                Assert.IsTrue(list.First().WasThroughValidation);
                foreach (var form in forms)
                {
                    if (form == forms.First())
                    {
                        Assert.AreEqual(ValidationText, form.Text);
                    }
                    else
                    {
                        Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                    }
                }
            });
        }

        [TestMethod, TestCategory("正常系")]
        public virtual void CalledBy2Invokers()
        {
            AssertAction((list, forms) =>
            {
                list.First().Invoker = forms.First();
                list.First().IsForSelf = false;

                list.Add(CreateDefaultCommand<BaseFormModel.ChildForm1>(forms.Last(), "Validation Text - 2"));
                list.Last().IsForSelf = false;
            }, (list, forms) =>
            {

                CommonCommandStatus.AssertValidatedButNotTarget();

                Assert.IsTrue((list.First()).WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("異常系")]
        public virtual void ValidationError()
        {
            AssertAction((list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = (item) =>
                {
                    item.Next = "Validation Text";
                    CommonCommandStatus.WasValidation = true;
                    return false;
                };

            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidationError();

                Assert.IsTrue(list.First().WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("異常系")]
        public virtual void ValidationNullCheck()
        {
            AssertAction((list, forms) =>
            {
                ((CommandValidator<TextItem>)list.First()).Validation = null;
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertNotValidating();
                Assert.IsFalse(list.First().WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });
        }

        [TestMethod, TestCategory("異常系")]
        public virtual void CalledBySelf_NullInvoker()
        {
            AssertAction((list, forms) =>
            {
                list.First().Invoker = null;

            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();
                Assert.IsTrue(list.First().WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);        // 該当データがいないのでテキストは同じ
                }
       
            });
        }

        [TestMethod, TestCategory("異常系")]
        public virtual void CalledByNullInvoker()
        {
            AssertAction((list, forms) =>
            {
                list.First().Invoker = null;
                list.First().IsForSelf = false;
 
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();

                Assert.IsTrue((list.First()).WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);        // 該当データがいないのでテキストは同じ
                }
            });
        }

        [TestMethod, TestCategory("正常系")]
        public virtual void CalledByExistedInvoker()
        {
            AssertAction((list, forms) =>
            {
                list.First().Invoker = forms.Last();
                list.First().IsForSelf = false;
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();

                Assert.IsTrue((list.First()).WasThroughValidation);

                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);       
                }
            });
        }

        [TestMethod, TestCategory("正常系")]
        public virtual void RecursiveFromExistedInvoker()
        {
            AssertAction((list, forms) =>
            {
                list.First().Invoker = forms.Last();
                list.First().IsForSelf = false;
                list.First().IsRecursive = true;
            }, (list, forms) =>
            {
                CommonCommandStatus.AssertValidatedButNotTarget();

                Assert.IsTrue((list.First()).WasThroughValidation);
                foreach (var form in forms)
                {
                    Assert.AreEqual(DefaultTextDictionary[form], form.Text);
                }
            });

        }

    }
}
