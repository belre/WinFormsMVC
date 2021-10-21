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

            for (int i = 0; i < number-1; i++)
            {
                var child = CreateDefaultBaseForm(original);
                child.Invoker = base_target;
                list_form.Add(child);

                base_target = child;
            }

            return list_form;
        }

        public static List<WinFormsMVC.View.BaseForm> CreatePerfectTreeForms(WinFormsMVC.View.BaseForm original, int depth)
        {
            var list_form = new List<WinFormsMVC.View.BaseForm>();

            var root = CreateDefaultBaseForm(original);

            int count = WinFormsMVC.View.BaseForm.MaxDepthTree - depth + 1;
            var list = new List<WinFormsMVC.View.BaseForm>();
            MakeBinaryTree(root, original, list, count);

            list_form.AddRange(list);

            return list_form;
        }

        private static void MakeBinaryTree(WinFormsMVC.View.BaseForm target, WinFormsMVC.View.BaseForm original, List<WinFormsMVC.View.BaseForm> list, int count = 1) 
        {
            if (count >= WinFormsMVC.View.BaseForm.MaxDepthTree)
            {
                return;
            }

            var child1 = CreateDefaultBaseForm(original);
            child1.Invoker = target;

            var child2 = CreateDefaultBaseForm(original);
            child2.Invoker = target;

            list.Add(target);

            MakeBinaryTree(child1, original, list, count + 1);
            MakeBinaryTree(child2, original, list, count + 1);
        }
    }
}
