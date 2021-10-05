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
        public List<Command.AbstractCommand> MememtoCommand { get; }

        public MementoManager()
        {
            MememtoCommand = new List<Command.AbstractCommand>();
        }

        public void PushCommand(Command.AbstractCommand abstractCommand)
        {
            MememtoCommand.Add(abstractCommand);
        }

        public Command.AbstractCommand PopCommand()
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