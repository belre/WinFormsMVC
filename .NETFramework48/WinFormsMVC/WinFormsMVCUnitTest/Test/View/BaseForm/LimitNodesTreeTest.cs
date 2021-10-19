using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class LimitNodesTreeTest
    {
        [TestMethod]
        public void TheSameLimitDepthTest()
        {
            var root = new WinFormsMVC.View.BaseForm();

            var base_target = root;
            var all_forms_ordered_from_root = new List<WinFormsMVC.View.BaseForm>();
            all_forms_ordered_from_root.Add(root);

            for (int i = 0; i < WinFormsMVC.View.BaseForm.MaxDepthTree - 1; i++)
            {
                base_target.Invoker = new WinFormsMVC.View.BaseForm();
                base_target = base_target.Invoker;

                all_forms_ordered_from_root.Add(base_target);
            }

            // 先祖-孫の関係が満たされているかをチェックする
            for (int i = 0; i < all_forms_ordered_from_root.Count; i++)
            {
                foreach (var child in all_forms_ordered_from_root.Skip(i + 1))
                {
                    Assert.IsTrue(all_forms_ordered_from_root[i].IsOriginatingFromParent(child));
                    Assert.IsFalse(child.IsOriginatingFromParent(all_forms_ordered_from_root[i]));
                }

                foreach (var parent in all_forms_ordered_from_root.Take(i))
                {
                    Assert.IsFalse(all_forms_ordered_from_root[i].IsOriginatingFromParent(parent));
                    Assert.IsTrue(parent.IsOriginatingFromParent(all_forms_ordered_from_root[i]));
                }
            }
        }

        private void MakeBinaryTree(WinFormsMVC.View.BaseForm target, int count=1)
        {
            if (count >= WinFormsMVC.View.BaseForm.MaxDepthTree)
            {
                return;
            }

            var child1 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = target
            };

            var child2 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = target
            };

            MakeBinaryTree(child1, count+1);
            MakeBinaryTree(child2, count+1);
        }

        [TestMethod]
        public void PerfectBinaryTree()
        {
            var root = new WinFormsMVC.View.BaseForm();
            MakeBinaryTree(root);
            Console.WriteLine("TEST");


        }

        [TestMethod]
        public void ExceedLimitDepthTest()
        {
            var root = new WinFormsMVC.View.BaseForm();
            var base_target = root;

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                for (int i = 0; i < WinFormsMVC.View.BaseForm.MaxDepthTree; i++)
                {
                    var child = new WinFormsMVC.View.BaseForm();
                    child.Invoker = base_target;
                    base_target = child;
                }
            });
        }
    }
}
