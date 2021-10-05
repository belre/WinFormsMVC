using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Model.Services
{
    public class MementoManager
    {
        public List<IEnumerable<Command.AbstractCommand>> MememtoCommand { get; }

        public MementoManager()
        {
            MememtoCommand = new List<IEnumerable<Command.AbstractCommand>>();
        }

        public void PushCommand(IEnumerable<Command.AbstractCommand> abstractCommand)
        {
            MememtoCommand.Add(abstractCommand);
        }

        public IEnumerable<Command.AbstractCommand> PopCommand()
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
    }
}