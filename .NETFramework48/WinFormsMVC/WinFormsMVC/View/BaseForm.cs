
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
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsOriginatingFromParent(BaseForm parent)
        {
            if (parent == null || Invoker == null)
            {
                return false;
            } 
            else if (parent == Invoker)
            {
                return true;
            }
            else
            {
                return Invoker.IsOriginatingFromParent(parent);
            }

        }
    }
}
