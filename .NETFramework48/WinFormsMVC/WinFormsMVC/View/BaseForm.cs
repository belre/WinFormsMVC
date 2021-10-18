
using System.Windows.Forms;
using WinFormsMVC.Facade;


namespace WinFormsMVC.View
{
    public partial class BaseForm : Form
    {
        public ViewFacade Facade { get; set; }

        public BaseForm Invoker { get; set; }

        public bool IsOriginatingFromTarget(BaseForm target)
        {
            if (target == null || Invoker == null)
            {
                return false;
            } 
            else if (target == Invoker)
            {
                return true;
            }
            else
            {
                return Invoker.IsOriginatingFromTarget(target);
            }

        }
    }
}
