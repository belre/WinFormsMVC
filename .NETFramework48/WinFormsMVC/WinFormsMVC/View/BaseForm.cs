
using System.Windows.Forms;
using WinFormsMVC.Facade;


namespace WinFormsMVC.View
{
    public partial class BaseForm : Form
    {
        /// <summary>
        /// ViewとFacadeの窓口クラスを表します。
        /// </summary>
        public ViewFacade Facade { get; set; }

        /// <summary>
        /// このフォームがどのフォームから作られたかを表します。
        /// </summary>
        public BaseForm Invoker { get; set; }

        

        /// <summary>
        /// このフォームの先祖にtargetがいるかどうかをチェックします。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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
