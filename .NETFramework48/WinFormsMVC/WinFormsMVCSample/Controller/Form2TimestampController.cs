using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Controller;
using WinFormsMVC.Services;
using WinFormsMVCSample.Model.TestComm;

namespace WinFormsMVCSample.Controller
{
    public class Form2TimestampController : CommandController
    {
        public delegate void NotifyTimeStamp(string timestamp);

        private NotifyTimeStamp _notify_function;


        public Form2TimestampController(FormsManagement manager)
            : base(manager)
        {
        }


        public void TriggerAsyncTimeStamp(NotifyTimeStamp action)
        {
            _notify_function += action;
            Task.Run(TimeStampAsync);
        }

        public void TimeStampAsync()
        {
            var timestamp_object = new TestTimeStampCommunication();
            _notify_function(timestamp_object.GetTimeStamp());
        }
    }
}
