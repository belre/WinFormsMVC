
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WinFormsMVC.Facade;


namespace WinFormsMVC.View
{
    public partial class BaseForm : Form
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
                return int.Parse(Properties.Resources.MAX_DEPTH_TREEFORM);
            }
        }

        private BaseForm _invoker;
        private List<BaseForm> _children;

        /// <summary>
        /// ViewとFacadeの窓口クラスを表します。
        /// </summary>
        public ViewFacade Facade { get; set; }

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
                else
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
        /// このフォームの先祖にtargetがいるかどうかをチェックします。
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsOriginatingFromParent(BaseForm parent, int count=1)
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
                return Invoker.IsOriginatingFromParent(parent, count+1);
            }
        }
    }
}
