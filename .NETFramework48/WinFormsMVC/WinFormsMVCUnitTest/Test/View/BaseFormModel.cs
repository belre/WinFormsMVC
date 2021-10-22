using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVCUnitTest.Test.View
{
    public class BaseFormModel
    {
        public class ChildForm1 : WinFormsMVC.View.BaseForm
        {

        }

        public class ChildForm2 : WinFormsMVC.View.BaseForm
        {

        }

        public class ChildForm3 : WinFormsMVC.View.BaseForm
        {

        }

        public class ChildForm4 : WinFormsMVC.View.BaseForm
        {

        }

        public class ChildForm5 : WinFormsMVC.View.BaseForm
        {

        }

        public static IEnumerable<Type> DefinedChildForms
        {
            get
            {
                return new Type[]
                {
                    typeof(ChildForm1), typeof(ChildForm2), typeof(ChildForm3), typeof(ChildForm4), typeof(ChildForm5)
                };
            }
        }


        public static WinFormsMVC.View.BaseForm CreateDefaultBaseForm(WinFormsMVC.View.BaseForm original, Type inherited_class) 
        {
            var form = (WinFormsMVC.View.BaseForm)Activator.CreateInstance(inherited_class.Assembly.GetName().Name,
                inherited_class.FullName, false,
                BindingFlags.CreateInstance | BindingFlags.SetField, null,
                Array.Empty<object>(), CultureInfo.CurrentCulture, null).Unwrap();

            form.Text = original.Text;

            return form;
        }

        public static List<WinFormsMVC.View.BaseForm> CreateSimplyConnectedForms(WinFormsMVC.View.BaseForm original, int number, bool are_child_forms=false)
        {
            var list_form = new List<WinFormsMVC.View.BaseForm>();

            // 単連結リストの作成
            var root = are_child_forms ? CreateDefaultBaseForm(original, DefinedChildForms.First()) : 
                                                CreateDefaultBaseForm(original, typeof(WinFormsMVC.View.BaseForm));

            var base_target = root;
            list_form.Add(root);

            for (int i = 0; i < number-1; i++)
            {
                var child = are_child_forms ? CreateDefaultBaseForm(original, typeof(WinFormsMVC.View.BaseForm)) : 
                                                    CreateDefaultBaseForm(original, typeof(WinFormsMVC.View.BaseForm));
                child.Invoker = base_target;
                list_form.Add(child);

                base_target = child;
            }

            return list_form;
        }

        public static List<WinFormsMVC.View.BaseForm> CreatePerfectTreeForms(WinFormsMVC.View.BaseForm original, int connected_node_number, bool are_child_forms=false)
        {
            var list_form = new List<WinFormsMVC.View.BaseForm>();

            var root = CreateDefaultBaseForm(original, typeof(WinFormsMVC.View.BaseForm));

            int count = WinFormsMVC.View.BaseForm.MaxDepthTree - connected_node_number + 1;
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

            var child1 = CreateDefaultBaseForm(original, typeof(WinFormsMVC.View.BaseForm));
            child1.Invoker = target;

            var child2 = CreateDefaultBaseForm(original, typeof(WinFormsMVC.View.BaseForm));
            child2.Invoker = target;

            list.Add(target);

            MakeBinaryTree(child1, original, list, count + 1);
            MakeBinaryTree(child2, original, list, count + 1);
        }
    }
}
