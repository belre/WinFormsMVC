using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Request
{
    public class ValidationStatus
    {

        public enum Operations
        {
            NO_VALIDATION,
            VALIDATED,
            ERROR_WITH_VALIDATING,
            DONE_FINALIZE
        };


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
        
        public Operations PreviousOperation
        {
            get;
            protected set;
        }
        

        public string Comment
        {
            get;
            set;
        }

        public ValidationStatus()
        {
            ExecutedCount = 0;
            PreviousOperation = Operations.NO_VALIDATION;
        }

        internal void StageNextValidation(Operations operation_status)
        {
            ExecutedCount++;
            PreviousOperation = operation_status;
        }

        internal void CommitValidation()
        {
            PreviousExecutedCount = ExecutedCount;
        }

        
    }
}
