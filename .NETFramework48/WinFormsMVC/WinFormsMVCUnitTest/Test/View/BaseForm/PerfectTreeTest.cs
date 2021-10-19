using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class PerfectTreeTest
    {
        private void MakeBinaryTree(WinFormsMVC.View.BaseForm target, int count = 1)
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

            MakeBinaryTree(child1, count + 1);
            MakeBinaryTree(child2, count + 1);
        }

        private void CheckBinaryTree(WinFormsMVC.View.BaseForm target, int count = 1)
        {
            foreach (var child in target.Children)
            {
                Assert.AreEqual(child.Invoker, target);
                Assert.IsTrue(target.Children.Contains(child));

                CheckBinaryTree(child, count + 1);
            }

            if (count == WinFormsMVC.View.BaseForm.MaxDepthTree)
            {
                Assert.AreEqual(target.Children.Count(), 0);
            }
        }

        [TestMethod]
        public void PerfectBinaryTree()
        {
            var root = new WinFormsMVC.View.BaseForm();
            MakeBinaryTree(root);

            Assert.IsNull(root.Invoker);
            CheckBinaryTree(root);
        }
    }
}
