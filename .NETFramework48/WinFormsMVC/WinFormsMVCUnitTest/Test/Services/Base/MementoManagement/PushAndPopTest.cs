using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.MementoManagement
{
    [TestClass]
    public class PushAndPopTest
    {
        private WinFormsMVC.Services.Base.MementoManagement _managed_memento;

        protected Command[] SingleDefaultCommand
        {
            get
            {
                return new List<Command>()
                {
                    new GenericCommand<BaseForm, TextItem>()
                    {
                        Validation = (item) =>
                        {
                            item.Next = "Test Text 1";
                            return true;
                        }
                    }
                }.ToArray();
            }
        }

        public PushAndPopTest()
        {
            _managed_memento = new WinFormsMVC.Services.Base.MementoManagement();
        }

        [TestMethod]
        public void PushSomeQueries()
        {
            _managed_memento.PushCommand(SingleDefaultCommand);
            _managed_memento.PushCommand(SingleDefaultCommand);
            _managed_memento.PushCommand(SingleDefaultCommand);

            Assert.AreEqual(3, _managed_memento.Mememtoes.Count);
        }

        [TestMethod]
        public void PushManyQueries()
        {
            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER-1; i++)
            {
                _managed_memento.PushCommand(SingleDefaultCommand);
            }

            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER-1, _managed_memento.Mememtoes.Count);

            _managed_memento.PushCommand(SingleDefaultCommand);
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER, _managed_memento.Mememtoes.Count);

            _managed_memento.PushCommand(SingleDefaultCommand);
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER, _managed_memento.Mememtoes.Count);

        }

    }
}
