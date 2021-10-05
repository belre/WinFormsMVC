using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCTraining.View;

namespace MVCTraining
{
    public class OperationManager
    {
        public Stack<Command> MememtoCommand
        {
            get;
        }

        public OperationManager()
        {
            MememtoCommand = new Stack<Command>();
        }

        public Command PopRedoCommand()
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
