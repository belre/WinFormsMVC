using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVCSample.Model.TestComm
{
    public class TestTimeStampCommunication
    {
        public string GetTimeStamp()
        {
            System.Threading.Thread.Sleep(5000);
            return DateTime.Now.ToLongTimeString();
        }

    }
}
