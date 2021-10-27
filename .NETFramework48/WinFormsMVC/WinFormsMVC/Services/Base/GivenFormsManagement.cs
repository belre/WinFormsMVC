﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            var recent_commands = ManagedMemento.PopCommand();

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
                command.Invalidate();
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
            if (command.IsForSelf)
            {
                return form == command.Invoker;
            }
            else if (command.IsRecursive)
            {
                return form.IsAncestor(command.Invoker);
            }
            else
            {
                return command.Invoker != null && form.Invoker == command.Invoker;
            }
        }

        protected virtual bool IsMatchType(BaseForm form, Command command)
        {
            if (command.IsIncludingInheritedType)
            {
                return form.GetType().IsSubclassOf(command.FormType);

            }
            else
            {
                return form.GetType() == command.FormType;
            }
        }
    }
}
