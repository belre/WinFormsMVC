using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Main.Services
{
    public class OperationManager
    {
        public List<Command.Command> MememtoCommand { get; }

        public OperationManager()
        {
            MememtoCommand = new List<Command.Command>();
        }

        public void PushRedoCommand(Command.Command command)
        {
            MememtoCommand.Add(command);
        }

        public Command.Command PopUndoCommand()
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