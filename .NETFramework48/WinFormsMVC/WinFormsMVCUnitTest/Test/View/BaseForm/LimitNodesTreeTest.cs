using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            for (int i = 0; i < WinFormsMVC.View.BaseForm.MaxDepthTree - 1; i++)
            {
                base_target.Invoker = new WinFormsMVC.View.BaseForm();
                base_target = base_target.Invoker;
            }
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
