using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void PushingSomeQueries()
        {
            _managed_memento.PushCommand(SingleDefaultCommand);
            _managed_memento.PushCommand(SingleDefaultCommand);
            _managed_memento.PushCommand(SingleDefaultCommand);

            Assert.AreEqual(3, _managed_memento.Mememtoes.Count);
            Assert.AreEqual(0, _managed_memento.RemovingMememtoes.Count);
        }

        [TestMethod]
        public void PushingManyQueries()
        {
            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER-1; i++)
            {
                _managed_memento.PushCommand(SingleDefaultCommand);
            }

            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER-1, _managed_memento.Mememtoes.Count);
            Assert.AreEqual(0, _managed_memento.RemovingMememtoes.Count);

            _managed_memento.PushCommand(SingleDefaultCommand);
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER, _managed_memento.Mememtoes.Count);
            Assert.AreEqual(0, _managed_memento.RemovingMememtoes.Count);

            _managed_memento.PushCommand(SingleDefaultCommand);
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER, _managed_memento.Mememtoes.Count);
            Assert.AreEqual(0, _managed_memento.RemovingMememtoes.Count);
        }

        [TestMethod]
        public void EqualToFirstCommand()
        {
            var first_data = SingleDefaultCommand;
            _managed_memento.PushCommand(first_data);

            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
            {
                _managed_memento.PushCommand(SingleDefaultCommand);
            }

            var last_data = _managed_memento.Mememtoes.Last();

            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
            {
                _managed_memento.PopLatestCommand();
            }

            Assert.IsTrue(_managed_memento.IsAvalableUndo());
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER - 1, _managed_memento.RemovingMememtoes.Count);

            Assert.AreEqual(first_data, _managed_memento.PopLatestCommand());
            Assert.IsFalse(_managed_memento.IsAvalableUndo());
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER, _managed_memento.RemovingMememtoes.Count);
            Assert.IsTrue(_managed_memento.RemovingMememtoes.Contains(last_data));

            Assert.IsNull(_managed_memento.PopLatestCommand());
            Assert.AreEqual(first_data, _managed_memento.RemovingMememtoes.First());
            Assert.IsTrue(_managed_memento.RemovingMememtoes.Contains(last_data));
        }



        [TestMethod]
        public void PoppedFirstCommand()
        {
            var first_data = SingleDefaultCommand;
            _managed_memento.PushCommand(first_data);

            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER; i++)
            {
                _managed_memento.PushCommand(SingleDefaultCommand);
            }
            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
            {
                _managed_memento.PopLatestCommand();
            }

            Assert.IsTrue(_managed_memento.IsAvalableUndo());
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER - 1, _managed_memento.RemovingMememtoes.Count);

            var pop_data = _managed_memento.PopLatestCommand();
            Assert.AreNotEqual(pop_data, first_data);
            Assert.IsNull(_managed_memento.PopLatestCommand());
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER, _managed_memento.RemovingMememtoes.Count);
            Assert.IsFalse(_managed_memento.IsAvalableUndo());
        }

        [TestMethod]
        public void PushAndPopSameQueries()
        {
            for (int k = 0; k < 5; k++)
            {
                for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER-1; i++)
                {
                    _managed_memento.PushCommand(SingleDefaultCommand);
                }

                Assert.AreEqual(0, _managed_memento.RemovingMememtoes.Count);

                for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
                {
                    _managed_memento.PopLatestCommand();
                }
            }
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER - 1, _managed_memento.RemovingMememtoes.Count);

            Assert.IsNull(_managed_memento.PopLatestCommand());
            Assert.AreEqual(_managed_memento.MAX_MEMEMTO_NUMBER - 1, _managed_memento.RemovingMememtoes.Count);
            Assert.IsFalse(_managed_memento.IsAvalableUndo());
        }

    }
}
