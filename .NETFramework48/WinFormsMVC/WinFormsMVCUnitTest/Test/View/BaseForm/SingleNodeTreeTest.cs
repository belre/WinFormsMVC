using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class SingleNodeTreeTest
    {
        [TestMethod]
        public void SingleNode()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            Assert.IsFalse(single_node.IsOriginatingFromParent(single_node));
            Assert.IsFalse(single_node.IsOriginatingFromParent(new WinFormsMVC.View.BaseForm()));
            Assert.IsFalse(single_node.IsOriginatingFromParent(null));
        }

        [TestMethod]
        public void AnyInvoker()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            single_node.Invoker = new WinFormsMVC.View.BaseForm();
            Assert.IsTrue(single_node.IsOriginatingFromParent(single_node.Invoker));
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
