using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WinFormsMVCUnitTest.Test.View.BaseForm
{
    [TestClass]
    public class SingleNodeTreeTest
    {
        [TestMethod]
        public void SingleNodeTest()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            Assert.IsFalse(single_node.IsOriginatingFromParent(single_node));
            Assert.IsFalse(single_node.IsOriginatingFromParent(new WinFormsMVC.View.BaseForm()));
            Assert.IsFalse(single_node.IsOriginatingFromParent(null));
        }

        [TestMethod]
        public void AnyInvokerTest()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            single_node.Invoker = new WinFormsMVC.View.BaseForm();
        }
        
        [TestMethod]
        public void SelfInvokerTest()
        {
            var single_node = new WinFormsMVC.View.BaseForm();
            Assert.ThrowsException<InvalidOperationException>( () => single_node.Invoker = single_node);
            Assert.IsNull(single_node.Invoker);
        }

    }
}
