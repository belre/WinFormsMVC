using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Main.Command
{
    public class Command
    {
        public string NextTemporary
        {
            get;
            set;
        }

        public string PrevTemporary
        {
            get;
            set;
        }

        public BaseForm Invoker { get; set; }

        public Func<Command, BaseForm, bool> InitOperation { get; set; }

        public Action<Command, BaseForm> NextOperation { get; set; }


        public Action<Command, BaseForm> PrevOperation { get; set; }

        public Action<Command, BaseForm> FinalOperation { get; set; }

        public Action<Command, BaseForm> ErrorOperation { get; set; }
    }

}
