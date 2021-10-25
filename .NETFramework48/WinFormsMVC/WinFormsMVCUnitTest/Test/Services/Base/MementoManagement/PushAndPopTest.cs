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
        public void PushingSomeQueries()
        {
            _managed_memento.PushCommand(SingleDefaultCommand);
            _managed_memento.PushCommand(SingleDefaultCommand);
            _managed_memento.PushCommand(SingleDefaultCommand);

            Assert.AreEqual(3, _managed_memento.Mememtoes.Count);
        }

        [TestMethod]
        public void PushingManyQueries()
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

        [TestMethod]
        public void EqualToFirstCommand()
        {
            var first_data = SingleDefaultCommand;
            _managed_memento.PushCommand(first_data);

            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
            {
                _managed_memento.PushCommand(SingleDefaultCommand);
            }
            for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
            {
                _managed_memento.PopCommand();
            }

            Assert.IsTrue(_managed_memento.IsAvalableUndo());
            Assert.AreEqual(first_data, _managed_memento.PopCommand());
            Assert.IsFalse(_managed_memento.IsAvalableUndo());

            Assert.IsNull(_managed_memento.PopCommand());
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
                _managed_memento.PopCommand();
            }

            Assert.IsTrue(_managed_memento.IsAvalableUndo());

            var pop_data = _managed_memento.PopCommand();
            Assert.AreNotEqual(pop_data, first_data);
            Assert.IsNull(_managed_memento.PopCommand());
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
                for (int i = 0; i < _managed_memento.MAX_MEMEMTO_NUMBER - 1; i++)
                {
                    _managed_memento.PopCommand();
                }
            }
            Assert.IsNull(_managed_memento.PopCommand());
            Assert.IsFalse(_managed_memento.IsAvalableUndo());
        }

    }
}
