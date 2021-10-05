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

        public Action<Command, BaseForm> InitOperation { get; set; }

        public Action<Command, BaseForm> FreeOperation { get; set; }

        public Action<Command, BaseForm> NextOperation { get; set; }

        public Action<Command, BaseForm> PrevOperation { get; set; }
    }

}
