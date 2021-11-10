using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Controller.Attribute;

namespace WinFormsMVCUnitTest.Test.Controller.BaseController
{
    [TestClass]
    public class NoAttributeReflectionTest
    {
        public class NoCtorClass
        {
        }

        public class SingleClass
        {
            public SingleClass()
            {

            }
        }

        // 引数2つクラス
        public class DualClass1
        {
            public DualClass1()
            {

            }

            public DualClass1(int x)
            {

            }
        }

        // 引数2つクラス
        public class DualClass2
        {

            public DualClass2(int x)
            {

            }

            public DualClass2()
            {

            }
        }


        // 可変引数クラス
        public class MulpipleClass
        {

            public MulpipleClass(params int[] array)
            {

            }


            public MulpipleClass()
            {

            }
        }

        public void GetRuntimeConstructor<T>(Type[] check_types) where T : class

        {
            var ctor = WinFormsMVC.Controller.BaseController.GetRuntimeConstructor(typeof(T));
            Assert.AreEqual(ctor, typeof(T).GetConstructor(check_types));
        }

        [TestMethod]
        public void GetRuntimeConstructor()
        {
            GetRuntimeConstructor<NoCtorClass>(Type.EmptyTypes);
            GetRuntimeConstructor<SingleClass>(Type.EmptyTypes);
            GetRuntimeConstructor<DualClass1>(Type.EmptyTypes);
            GetRuntimeConstructor<DualClass2>(new Type[]{typeof(int)});
            GetRuntimeConstructor<MulpipleClass>(new Type[] { typeof(int[]) });
        }
    }
}
