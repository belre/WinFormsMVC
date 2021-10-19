using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Controller.Attribute;

namespace WinFormsMVCUnitTest.Test.Controller.BaseController
{
    [TestClass]
    public class BothAttributeReflectionTest
    {
        /// *** 以降、GetRuntimeConstructorのテスト *** ///



        // 引数2つクラス
        public class DualClass1
        {
            [CalledAsController]

            public DualClass1()
            {

            }
            [CalledAsController]

            public DualClass1(int x)
            {

            }
        }

        // 引数2つクラス
        public class DualClass2
        {
            [CalledAsController]
            public DualClass2(int x)
            {

            }
            [CalledAsController]
            public DualClass2()
            {

            }
        }

        // 可変引数クラス
        public class MulpipleClass
        {
            [CalledAsController]

            public MulpipleClass(params int[] array)
            {

            }

            [CalledAsController]

            public MulpipleClass()
            {

            }
        }

        public void GetRuntimeConstructorTest<T>() where T : class

        {
            var ctor = WinFormsMVC.Controller.BaseController.GetRuntimeConstructor(typeof(T));
            Assert.IsNull(ctor);
        }

        [TestMethod]
        public void GetRuntimeConstructorTest()
        {
            GetRuntimeConstructorTest<DualClass1>();
            GetRuntimeConstructorTest<DualClass2>();
            GetRuntimeConstructorTest<MulpipleClass>();
        }
    }
}
