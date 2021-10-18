using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using WinFormsMVC.Controller;
using WinFormsMVC.Controller.Attribute;
using WinFormsMVC.Request;
using WinFormsMVC.Services;
using WinFormsMVCSample.Model;
using WinFormsMVCSample.Model.TestComm;
using WinFormsMVCSample.View;

namespace WinFormsMVCSample.Controller
{
    public class Form2Controller : CommandController
    {
        public delegate void NotifyImage(Image image);

        public delegate void NotifyTimeStamp(string timestamp);


        public Form2Controller(FormsManagement manager)
            : base(manager)
        {
        }


        public void GetTimeStamp(NotifyTimeStamp action)
        {
            Task.Run(() =>
            {
                var time_stamp_operation = new TimeStampOperation();
                action(time_stamp_operation.GetTimeStamp());
            });
        }

        public void StampTextIntoImage(NotifyImage action, Image original)
        {
            Task.Run(() =>
            {
                var image_stamp_operation = new ImageStampOperation();
                action(image_stamp_operation.GetNewImage((Image)original.Clone()));
            });
        }
    }
}
