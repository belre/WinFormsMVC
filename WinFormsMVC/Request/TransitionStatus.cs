using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Request
{
    public class TransitionStatus
    {
        public int ExecutedCount
        {
            get;
            protected set;
        }

        public int PreviousExecutedCount
        {
            get;
            protected set;
        }

        
        public TransitionStatus()
        {
            ExecutedCount = 0;
        }

        internal void StageNextValidation()
        {
            ExecutedCount++;
        }

        internal void CommitNextValidation()
        {
            PreviousExecutedCount = ExecutedCount;
        }
    }
}
