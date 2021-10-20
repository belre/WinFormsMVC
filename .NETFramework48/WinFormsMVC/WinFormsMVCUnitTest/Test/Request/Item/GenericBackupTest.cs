using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Windows.Forms;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Request.Item
{
    [TestClass]
    public class GenericBackupTest<T, U> where T : class where U : GenericCommandItem<T>
    {
        protected virtual T FormValue
        {
            get;
        }

        protected virtual T NextValue
        {
            get;
        }


        [TestMethod]
        public void YetStored()
        {
            var item = (U)typeof(U).GetConstructor(new Type[0]).Invoke(new object[0]);
            var form = new BaseForm();

            Assert.IsNull(item[form]);

            item.Next = NextValue;
            Assert.AreEqual(item.Next, NextValue);
        }

        [TestMethod]
        public void Stored()
        {
            var item = (U)typeof(U).GetConstructor(new Type[0]).Invoke(new object[0]);
            var form = new BaseForm();

            item[form] = FormValue;
            Assert.AreEqual(item[form], FormValue);

            item.Next = NextValue;
            Assert.AreEqual(item.Next, NextValue);
        }
    }
}
