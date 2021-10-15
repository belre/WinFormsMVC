
using System.Windows.Forms;
using WinFormsMVC.Facade;


namespace WinFormsMVC.View
{
    public partial class BaseForm : Form
    {
        public ViewFacade Facade { get; set; }

        public BaseForm Invoker { get; set; }

        public BaseForm()
        {
            InitializeComponent();
        }
    }
}
