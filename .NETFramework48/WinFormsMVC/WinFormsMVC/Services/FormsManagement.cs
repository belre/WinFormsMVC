using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Facade;
using WinFormsMVC.Request;
using WinFormsMVC.Services.Base;
using WinFormsMVC.View;

namespace WinFormsMVC.Services
{
    /// <summary>
    /// BaseFormを管理します。
    /// </summary>
    public class FormsManagement : GivenFormsManagement
    {
        /// <summary>
        /// 固定の数のフォーム一覧を表します。
        /// </summary>
        protected override IEnumerable<BaseForm> ManagedBaseForms
        {
            get
            {
                return _managed_baseform;
            }
        }


        /// <summary>
        /// 管理されているBaseFormの一覧です。
        /// </summary>
        private readonly List<BaseForm> _managed_baseform;

        /// <summary>
        /// BaseFormとControllerを接続する窓口役(Facade)を表します。
        /// </summary>
        private ViewFacade _facade;



        /// <summary>
        /// 窓口を表すクラスです。
        /// </summary>
        public ViewFacade Facade
        {
            get { return _facade; }
            set { _facade = value; }
        }


        /// <summary>
        /// Form管理を生成します。
        /// </summary>
        public FormsManagement()
            : base(new BaseForm[0])
        {
            _managed_baseform = new List<BaseForm>();
        }

        /// <summary>
        /// 新しいフォームを立ち上げます。
        /// </summary>
        /// <typeparam name="TargetForm"></typeparam>
        /// <param name="source">どのフォームが立ち上げるか</param>
        /// <param name="target">なんのフォームを立ち上げるか</param>
        public void LaunchForm<TargetForm>(BaseForm source, TargetForm target, bool is_modal)
            where TargetForm : BaseForm
        {
            _managed_baseform.Add(target);
            target.Invoker = source;
            target.Facade = _facade;
            target.Closed += OnFormClosed;

            if (is_modal)
            {
                target.ShowDialog();
            }
            else
            {
                target.Show();
            }
        }


        public virtual void RunAndRecord(IEnumerable<Request.Command> command_list) 
        {
            base.Run(command_list);

            ManagedMemento.PushCommand(command_list);
        }

        /// <summary>
        /// フォームが閉じられた時に自動的に実行されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, EventArgs e)
        {
            // 自分自身
            BaseForm form = (BaseForm) sender;

            // 子フォームを探す
            var children_form = new List<BaseForm>();
            foreach (var any_form in _managed_baseform)
            {
                if (any_form.Invoker == form)
                {
                    children_form.Add(any_form);
                }
            }

            // 削除
            _managed_baseform.Remove(form);
            foreach (var child in children_form)
            {
                child.Close();
                _managed_baseform.Remove(child);
            }
        }
    }
}
