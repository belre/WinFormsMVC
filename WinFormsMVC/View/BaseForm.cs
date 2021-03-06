
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using WinFormsMVC.Facade;
using System.Resources;

namespace WinFormsMVC.View
{
    public partial class BaseForm : Form, IMvcForm
    {

        protected enum FormTreeStatus
        {
            OK,
            TREE_CORRUPTED,
            OVER_DEPTH
        }


        public static int MaxDepthTree
        {
            get
            {
#if NETFRAMEWORK
                return int.Parse(WinFormsMVC.Properties.Resources.MAX_DEPTH_TREEFORM);
#else
                return int.Parse(WinFormsMVCDotnet6.Properties.ConfigDotnet6.MAX_DEPTH_TREEFORM);
#endif
            }
        }

        private BaseForm _invoker;
        private List<BaseForm> _children;

        /// <summary>
        /// ViewとFacadeの窓口クラスを表します。
        /// </summary>
        public ViewFacadeCore FacadeCore { get; set; }



        public IEnumerable<BaseForm> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// このフォームがどのフォームから作られたかを表します。
        /// </summary>
        public BaseForm Invoker {
            get
            {
                return _invoker;
            }
            set
            {
                _invoker = value;

                var formtree_validity = IsFormTreeValid(new List<BaseForm>());
                if (formtree_validity != FormTreeStatus.OK)
                {
                    _invoker = null;

                    if (formtree_validity == FormTreeStatus.TREE_CORRUPTED)
                    {
                        throw new InvalidOperationException("Invokerの構造が不正な形式で実行されています.");
                    } 
                    else if (formtree_validity == FormTreeStatus.OVER_DEPTH)
                    {
                        throw new InvalidOperationException("BaseFormの階層が深すぎます.");
                    }
                }
                else if(_invoker != null)
                {
                    // privateなクラス間であれば子クラスを定義できる
                    _invoker._children.Add(this);
                }
            }
        }

        public BaseForm()
        {
            InitializeComponent();

            _children = new List<BaseForm>();
        }

        /// <summary>
        /// BaseFormが構成するツリー構造が正常かどうかを表します.
        /// </summary>
        /// <param name="seeked_list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected FormTreeStatus IsFormTreeValid(List<BaseForm> seeked_list, int count=1)
        {
            if (Invoker == null)
            {
                return FormTreeStatus.OK;
            }
            else if (seeked_list.Contains(Invoker) ) 
            {
                return FormTreeStatus.TREE_CORRUPTED;
            } 
            else if (count >= MaxDepthTree)           // 再帰防止(MAX_NUMBER_INVOKER_LOOP)
            {
                return FormTreeStatus.OVER_DEPTH;
            }
            else
            {
                seeked_list.Add(this);
                return Invoker.IsFormTreeValid(seeked_list, count+1);
            }
        }

        /// <summary>
        /// このフォームの先祖にparentがいるかどうかをチェックします。
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsChildOf(BaseForm parent, int count=1)
        {
            if (parent == null || Invoker == null || count >= MaxDepthTree)
            {
                return false;
            } 
            else if (parent == Invoker)
            {
                return true;
            }
            else
            {
                return Invoker.IsChildOf(parent, count+1);
            }
        }


    }
}
