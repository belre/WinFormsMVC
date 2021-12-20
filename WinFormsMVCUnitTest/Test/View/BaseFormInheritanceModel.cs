using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVCUnitTest.Test.View
{
    public class BaseFormInheritanceModel : BaseFormModel
    {
        public class GrandChildForm2 : ChildForm2
        {

        }


        public class GrandGrandChildForm2 : GrandChildForm2
        {

        }

        public override IEnumerable<Type> DefinedChildForms
        {
            get
            {
                return new Type[]
                {
                    typeof(ChildForm1), typeof(ChildForm2), typeof(ChildForm3), typeof(GrandChildForm2), typeof(ChildForm5), typeof(GrandGrandChildForm2)
                };
            }
        }
    }
}
