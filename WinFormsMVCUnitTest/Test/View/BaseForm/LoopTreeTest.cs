using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class LoopTreeTest
    {
        [TestMethod]
        public void ClosedSimpleLoopTree()
        {

            var node1 = new WinFormsMVC.View.BaseForm();
            var node2 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = node1
            };
            var node3 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = node2
            };
            var node4 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = node3
            };
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                node1.Invoker = node4;
            });
        }

        [TestMethod]
        public void ClosedHalfLoopTree()
        {
            var node1 = new WinFormsMVC.View.BaseForm();
            var node2 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = node1
            };
            var node3 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = node2
            };
            var node4 = new WinFormsMVC.View.BaseForm()
            {
                Invoker = node3
            };
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                node2.Invoker = node4;
            });
        }


    }
}
