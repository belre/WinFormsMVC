using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCTraining.View;

namespace MVCTraining.Controller
{
    public class Form3Controller : Controller
    {
        private FormManager _manager;
        private OperationManager _operation_manager;

        public Form3Controller(FormManager manager, OperationManager operation_manager)
        {
            _manager = manager;
            _operation_manager = operation_manager;
        }

        public void Test(Form3 form3)
        {
            _manager.LaunchForm(form3, new Form2());
        }
    }
}
