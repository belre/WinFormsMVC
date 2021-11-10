using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class SimplyConnectedNodesTreeTest
    {
        protected List<WinFormsMVC.View.BaseForm> ListFormsOrderedFromRoot
        {
            get;
        }

        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
        }



        public SimplyConnectedNodesTreeTest()
        {
            ListFormsOrderedFromRoot = new List<WinFormsMVC.View.BaseForm>();

            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            ListFormsOrderedFromRoot = BaseFormModel.CreateSimplyConnectedForms(DefaultBaseForm, WinFormsMVC.View.BaseForm.MaxDepthTree);
        }

        [TestMethod]
        public void TheSameLimitDepthTest()
        {
            // 単連結リストの作成
            var root = new WinFormsMVC.View.BaseForm();

            var base_target = root;
            var all_forms_ordered_from_root = new List<WinFormsMVC.View.BaseForm>();
            all_forms_ordered_from_root.Add(root);

            for (int i = 0; i < WinFormsMVC.View.BaseForm.MaxDepthTree - 1; i++)
            {
                var child = new WinFormsMVC.View.BaseForm();
                child.Invoker = base_target;
                all_forms_ordered_from_root.Add(child);

                base_target = child;
            }

            // 先祖-孫の関係が満たされているかをチェックする
            for (int i = 0; i < all_forms_ordered_from_root.Count; i++)
            {
                foreach (var child in all_forms_ordered_from_root.Skip(i + 1))
                {
                    Assert.IsTrue(child.IsAncestor(all_forms_ordered_from_root[i]));
                    Assert.IsFalse(all_forms_ordered_from_root[i].IsAncestor(child));
                }

                foreach (var parent in all_forms_ordered_from_root.Take(i))
                {
                    Assert.IsFalse(parent.IsAncestor(all_forms_ordered_from_root[i]));
                    Assert.IsTrue(all_forms_ordered_from_root[i].IsAncestor(parent));
                }

                if (i >= 1)
                {
                    Assert.IsTrue(all_forms_ordered_from_root[i - 1].Children.Contains(all_forms_ordered_from_root[i]));
                }
            }
        }


        [TestMethod]
        public void ExceedLimitDepthTest()
        {
            var root = new WinFormsMVC.View.BaseForm();
            var base_target = root;

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                BaseFormModel.CreateSimplyConnectedForms(DefaultBaseForm, WinFormsMVC.View.BaseForm.MaxDepthTree+1);
            });
        }
    }
}
