﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.Controller.Attribute;

namespace WinFormsMVCUnitTest.Test.Controller.BaseController
{
    [TestClass]
    public class SecondAttributeReflectionTest
    {
        /// *** 以降、GetRuntimeConstructorのテスト *** ///



        // 引数2つクラス
        public class DualClass1
        {
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
            public MulpipleClass(params int[] array)
            {

            }

            [CalledAsController]

            public MulpipleClass()
            {

            }
        }

        public void GetRuntimeConstructorTest<T>(Type[] check_types) where T : class

        {
            var ctor = WinFormsMVC.Controller.BaseController.GetRuntimeConstructor(typeof(T));
            Assert.AreEqual(ctor, typeof(T).GetConstructor(check_types));
        }

        [TestMethod]
        public void GetRuntimeConstructorTest()
        {
            GetRuntimeConstructorTest<DualClass1>(new Type[1]{typeof(int)});
            GetRuntimeConstructorTest<DualClass2>(Type.EmptyTypes);
            GetRuntimeConstructorTest<MulpipleClass>(Type.EmptyTypes);
        }
    }
}
