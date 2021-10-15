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
        public List<IEnumerable<Request.AbstractCommand>> MememtoCommand { get; }

        public CommandMementoManagement()
        {
            MememtoCommand = new List<IEnumerable<Request.AbstractCommand>>();
        }

        public void PushCommand(IEnumerable<Request.AbstractCommand> abstractCommand)
        {
            MememtoCommand.Add(abstractCommand);
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
    }
}