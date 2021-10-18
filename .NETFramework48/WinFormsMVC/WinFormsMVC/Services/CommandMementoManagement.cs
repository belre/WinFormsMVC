using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Services
{
    /// <summary>
    /// Mementoパターンを使用して、コマンドの履歴を参照します。
    /// </summary>
    public class CommandMementoManagement
    {
        /// <summary>
        /// 最大で記録するコマンド数です。
        /// </summary>
        public readonly int MAX_MEMEMTO_NUMBER = 10;

        /// <summary>
        /// Mementoのコマンドの一覧を表します。
        /// </summary>
        public List<IEnumerable<Request.AbstractCommand>> MememtoCommand { get; }

        public CommandMementoManagement()
        {
            MememtoCommand = new List<IEnumerable<Request.AbstractCommand>>();
        }

        /// <summary>
        /// コマンドを追加します。
        /// </summary>
        /// <param name="abstractCommand"></param>
        public void PushCommand(IEnumerable<Request.AbstractCommand> abstractCommand)
        {
            MememtoCommand.Add(abstractCommand);

            if (MememtoCommand.Count > MAX_MEMEMTO_NUMBER)
            {
                MememtoCommand.RemoveRange(0, MememtoCommand.Count - MAX_MEMEMTO_NUMBER);
            }
        }

        /// <summary>
        /// コマンドを削除します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Request.AbstractCommand> PopCommand()
        {
            if (MememtoCommand.Count != 0)
            {
                var target = MememtoCommand.Last();
                MememtoCommand.Remove(target);
                return target;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 「元に戻す」が可能かどうかを表します。
        /// </summary>
        /// <returns></returns>
        public bool IsAvalableUndo()
        {
            return MememtoCommand.Count != 0;
        }
    }
}