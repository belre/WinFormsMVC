using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class SingleNodeTreeTest
    {
        [TestMethod]
        public void SingleNode()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            Assert.IsFalse(single_node.IsAncestor(single_node));
            Assert.IsFalse(single_node.IsAncestor(new WinFormsMVC.View.BaseForm()));
            Assert.IsFalse(single_node.IsAncestor(null));
            Assert.AreEqual(single_node.Children.Count(), 0);
        }

        [TestMethod]
        public void AnyInvoker()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            single_node.Invoker = new WinFormsMVC.View.BaseForm();
            Assert.IsTrue(single_node.IsAncestor(single_node.Invoker));
            Assert.AreEqual(single_node.Children.Count(), 0);
            Assert.IsTrue(single_node.Invoker.Children.Contains(single_node));
        }

        [TestMethod]
        public void InvokerSubstitutesNull()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            single_node.Invoker = null;
        }
        
        [TestMethod]
        public void SelfInvoker()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            Assert.ThrowsException<InvalidOperationException>( () => single_node.Invoker = single_node);
            Assert.IsNull(single_node.Invoker);
        }
    }
}
