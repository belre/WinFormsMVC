using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVC.Services.Base
{
    public class GivenFormsManagement
    {
        protected virtual IEnumerable<BaseForm> ManagedBaseForms
        {
            get;
        }


        /// <summary>
        /// コマンド履歴のオブジェクトです。
        /// </summary>
        public MementoManagement ManagedMemento
        {
            get;
        }

        /// <summary>
        /// 新たなFormManagementを作成します。
        /// ※ このクラス単独では、新たなFormのLaunch, Closeの処理を検知しません
        /// </summary>
        /// <param name="_managed_base_forms"></param>
        public GivenFormsManagement(IEnumerable<BaseForm> _managed_base_forms)
        {
            ManagedBaseForms = _managed_base_forms;
            ManagedMemento = new MementoManagement();
        }

        /// <summary>
        /// コマンド履歴に従って、フォームを更新します。
        /// </summary>
        /// <param name="command_list"></param>
        /// <param name="is_record_memento"></param>
        public virtual void Run(IEnumerable<Request.Command> command_list)
        {
            foreach (var command in command_list)
            {
                if (command.Validate())
                {
                    Update(command);
                }
            }
        }


        public virtual void RunAndRecord(IEnumerable<Request.Command> command_list)
        {
            Run(command_list);

            ManagedMemento.PushCommand(command_list);
        }


        /// <summary>
        /// Memento一覧に従ってフォームを更新します。
        /// </summary>
        /// <param name="command"></param>
        protected void Update(Command command)
        {
            var target_forms = new List<BaseForm>();
            foreach (var form in ManagedBaseForms)
            {
                if (IsMatchInvoker(form, command) && IsMatchType(form, command))
                {
                    target_forms.Add(form);
                }
            }

            foreach (var target in target_forms)
            {
                command.Next(target);
            }
        }

        /// <summary>
        /// Memento一覧に従って、フォームを戻します。
        /// </summary>
        public void Undo()
        {
            var recent_commands = ManagedMemento.PopLatestCommand();

            if (recent_commands == null)
            {
                return;
            }

            foreach (var command in recent_commands.Reverse())
            {
                foreach (var form in ManagedBaseForms)
                {
                    if (IsMatchInvoker(form, command) && IsMatchType(form, command))
                    {
                        command.Prev(form);
                    }
                }
            }
        }

        /// <summary>
        /// RemovingMementoの一覧に従って、フォームの戻した結果を再反映させます。
        /// </summary>
        public void Redo()
        {
            var adapt_command = ManagedMemento.RestoreCommand();

            if (adapt_command == null)
            {
                return;
            }

            foreach (var command in adapt_command)
            {
                var target_forms = new List<BaseForm>();
                foreach (var form in ManagedBaseForms)
                {
                    if (IsMatchInvoker(form, command) && IsMatchType(form, command))
                    {
                        target_forms.Add(form);
                    }
                }

                foreach (var target in target_forms)
                {
                    command.Restore(target);
                }
            }
        }

        /// <summary>
        /// フォームの発行元とコマンドの発行元が一致しているかを返します。
        /// * IsForSelfがtrueの場合は、コマンドは自分自身に送られるので、フォーム自身と一致するかが返される。
        /// * IsRetrievedがtrueの場合は、formがcommand.Invokerの孫フォームであるかが返される。
        /// * それ以外の場合は、formがcommand.Invokerの子フォームであるかが返される。
        /// </summary>
        /// <param name="form">対象のフォーム</param>
        /// <param name="command">コマンド</param>
        /// <returns></returns>
        protected virtual bool IsMatchInvoker(BaseForm form, Command command)
        {
            if (command.NodeSearchMode == Command.NodeSearchMethod.Self)
            {
                return form == command.Invoker;
            }
            else if (command.NodeSearchMode == Command.NodeSearchMethod.All)
            {
                return true;
            }
            else if (command.NodeSearchMode == Command.NodeSearchMethod.RecursiveDeeper)
            {
                return form.IsChildOf(command.Invoker);
            }
            else if (command.NodeSearchMode == Command.NodeSearchMethod.RecursiveShallower)
            {
                return command.Invoker.IsChildOf(form) && form.Invoker != command.Invoker;
            }
            else if(command.NodeSearchMode == Command.NodeSearchMethod.OnlyMyChildren)
            {
                return command.Invoker != null && form.Invoker == command.Invoker;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool IsMatchType(BaseForm form, Command command)
        {
            if (form.GetType().GetInterfaces().Contains(command.FormType))
            {
                // インタフェースが定義されている場合は、
                // 常にtrueを返却する。
                return true;
            }
            else
            {
                // クラスを指定している場合は型によって判定する
                if (command.IsIncludingInheritedSubclass)
                {
                    return form.GetType().IsSubclassOf(command.FormType) ||
                           form.GetType() == command.FormType;

                }
                else
                {
                    return form.GetType() == command.FormType;
                }
            }
        }
    }
}
