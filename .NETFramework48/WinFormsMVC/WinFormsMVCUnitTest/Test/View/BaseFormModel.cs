using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVCUnitTest.Test.View
{
    public class BaseFormModel
    {

        public static WinFormsMVC.View.BaseForm CreateDefaultBaseForm(WinFormsMVC.View.BaseForm original)
        {
            return new WinFormsMVC.View.BaseForm()
            {
                Text = original.Text
            };
        }

        public static List<WinFormsMVC.View.BaseForm> CreateSimplyConnectedForms(WinFormsMVC.View.BaseForm original, int number)
        {
            var list_form = new List<WinFormsMVC.View.BaseForm>();

            // 単連結リストの作成
            var root = CreateDefaultBaseForm(original);

            var base_target = root;
            list_form.Add(root);

            for (int i = 0; i < number - 1; i++)
            {
                var child = CreateDefaultBaseForm(original);
                child.Invoker = base_target;
                list_form.Add(child);

                base_target = child;
            }

            return list_form;
        }

    }
}
