using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Services
{
    public class CommandMementoManagement
    {
        public readonly int MAX_MEMEMTO_NUMBER = 10;

        public List<IEnumerable<Request.AbstractCommand>> MememtoCommand { get; }

        public CommandMementoManagement()
        {
            MememtoCommand = new List<IEnumerable<Request.AbstractCommand>>();
        }

        public void PushCommand(IEnumerable<Request.AbstractCommand> abstractCommand)
        {
            MememtoCommand.Add(abstractCommand);

            if (MememtoCommand.Count > MAX_MEMEMTO_NUMBER)
            {
                MememtoCommand.RemoveRange(0, MememtoCommand.Count - MAX_MEMEMTO_NUMBER);
            }
        }

        public IEnumerable<Request.AbstractCommand> PopCommand()
        {
            if (MememtoCommand.Count != 0)
            {
                var target = MememtoCommand.Last();
                MememtoCommand.Remove(target);
                return target;
            }
            else
            {
                return null;
            }
        }

        public bool IsAvalableUndo()
        {
            return MememtoCommand.Count != 0;
        }
    }
}