using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCTraining.View;

namespace MVCTraining
{
    public class Command
    {
        public string _temporary;

        public BaseForm Invoker
        {
            get;
            set;
        }

        public Func<Command, BaseForm, bool> NextOperation
        {
            get;
            set;
        }

        public Func<Command, BaseForm, bool> PrevOperation
        {
            get;
            set;
        }
    }
}
