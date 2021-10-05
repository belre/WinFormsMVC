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
        public Stack<Command.Command> MememtoCommand { get; }

        public OperationManager()
        {
            MememtoCommand = new Stack<Command.Command>();
        }

        public Command.Command PopRedoCommand()
        {
            if (MememtoCommand.Count != 0)
            {
                return MememtoCommand.Pop();
            }
            else
            {
                return null;
            }
        }

    }

}